using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Molblin1 : MonoBehaviour
{
    #region 변수
    static public Molblin1 instance = null;
    private void Awake()
    {
        instance = this;
    }

    // 상태
    public MolblinState state;

    // 상태 열거
    public enum MolblinState
    {
        Idle,
        Move,
        Dodge,
        AttackChoice,
        Kick,
        TwoHandsAttack,
        ComboAttack,
        AttackDelay,
        Damaged,
        Die
    }

    // 이동속도
    public float speed = 3;

    // 거리
    float distance;
    public float detectDistance;
    public float attackPossibleDistance;
    public float pattern2Distance = 7;
    public float pattern3Distance = 5;

    // 시간
    float currentTime;
    public float waitTime;
    public float delayTime = 2;

    // 플레이어(링크)
    GameObject link;
    Vector3 linkDir;

    // 애니메이션
    Animator anim;

    // 체력
    public int currentHP;
    public int maxHP = 30;

    Rigidbody rb;

    // bool
    bool isPohyo;
    bool isDodge;
    bool isAttack;
    bool isDisturb = true;
    bool isTwoHands;
    bool isComboAttack;
    bool isKick;
    bool isPohyo;
    public bool isDamaged;

    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Link");
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);
    }
    #endregion

    #region Update
    void Update()
    {
        // print("isAttack" + isAttack);
        // print("isDisturb" + isDisturb);

        #region 바라보기
        // 링크가 있는 방향을 찾는다.
        linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();
        Quaternion linkRotate = Quaternion.LookRotation(linkDir);

        // 그 방향을 바라본다.
        transform.rotation = Quaternion.Lerp(transform.rotation, linkRotate, Time.deltaTime * 5);
        #endregion

        #region 거리재기
        // 거리를 구한다.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);
        #endregion

        #region 상태함수
        if (state == MolblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MolblinState.Move)
        {
            UpdateMove();
        }
        else if (state == MolblinState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MolblinState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        else if (state == MolblinState.Kick)
        {
            Kick();
        }
        else if (state == MolblinState.TwoHandsAttack)
        {
            TwoHandsAttack();
        }
        else if (state == MolblinState.ComboAttack)
        {
            ComboAttack();
        }
        else if (state == MolblinState.AttackDelay)
        {
            UpdateAttackDelay();
        }
        else if (state == MolblinState.Damaged)
        {
            DamageProcess();
        }
        else if (state == MolblinState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region Update States

    private void UpdateIdle()
    {
        // 만약 링크와의 거리가 감지 거리보다 가까우면
        if (distance < detectDistance)
        {
            if (isPohyo == false)
            {
                // 포효를 하고 
                isPohyo = true;
                anim.SetTrigger("Buff");
            }
            currentTime += Time.deltaTime;
            // 2초가 지나면
            if (currentTime > 2 && isPohyo)
            {
                // 상태를 Move 로 변환한다.
                isDodge = false;
                state = MolblinState.Move;
                currentTime = 0;
            }
        }
    }

    private void UpdateMove()
    {
        // 만약 링크와의 거리가 감지 거리보다 멀어지면 Idle 상태로 돌아간다.
        if (distance > detectDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinState.Idle;
            anim.SetBool("Move", false);
        }

        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면 이동한다.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            anim.SetBool("Move", true);
            // 링크가 있는 곳으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;
        }

        // 링크가 공격 거리 안으로 들어오면
        else if (distance < attackPossibleDistance)
        {
            // 공격선택상태로 전환한다.
            state = MolblinState.AttackChoice;
        }
    }

    private void UpdateAttackChoice()
    {
        anim.SetBool("Move", true);
        // 선택 시간 중에 링크가 공격가능 거리 보다 멀어진다면 Idle        
        if (distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinState.Idle;
            anim.SetBool("Wait", false);
        }

        // 50% 확률로 양손공격 실행
        int attackRandom = Random.Range(0, 2);
        if (attackRandom == 0 && isComboAttack == false)
        {
            isTwoHands = true;

            // 링크 방향으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;

            // 링크와의 거리가 패턴 2 거리 이하가 되면
            if (distance < pattern2Distance)
            {
                anim.SetBool("Wait", true);
                anim.SetBool("Move", false);
                state = MolblinState.TwoHandsAttack;
            }
        }

        // 50% 확률로 콤보공격  실행
        else if (attackRandom == 1 && isTwoHands == false)
        {
            isComboAttack = true;

            // 링크 방향으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;

            // 링크와의 거리가 패턴 2 거리 이하가 되면
            if (distance < pattern3Distance)
            {
                anim.SetBool("Wait", true);
                anim.SetBool("Move", false);
                state = MolblinState.ComboAttack;
            }
        }
    }

    private void Kick()
    {
        print("발차기");
        anim.SetTrigger("Kick");
        state = MolblinState.Idle;
        isKick = false;
    }

    private void UpdateDodge()
    {
        // 애니메이션 실행
        anim.SetTrigger("Dodge");
        state = MolblinState.Idle;
        isDodge = false;
    }

    private void TwoHandsAttack()
    {

            // 시간을 흐르게 한다.
            currentTime += Time.deltaTime;
            // 1초 후에
            if (currentTime >= 1)
            {
                print("양손 공격");
                anim.SetBool("TwoHands", true);

                //isDisturb = false;
                isAttack = true;

                // 양손 공격을 한다.
                anim.SetBool("Wait", false);
            }
            // 공격이 끝나는 시간이 되면
            if (currentTime >= 1.5f)
            {
                //isDisturb = true;
                isTwoHands = false;
                isDodge = false;

                anim.SetTrigger("AttackDelay");
                state = MolblinState.AttackDelay;

                currentTime = 0;
            }
        
    }

    private void ComboAttack()
    {
  
            currentTime += Time.deltaTime;

            if (currentTime >= 1)
            {
                print("콤보 공격");

                //isDisturb = false;
                isAttack = true;

                // 콤보 공격을 한다.
                anim.SetBool("Wait", false);
                anim.SetBool("ComboAttack", true);
            }
            if (currentTime >= 4)
            {

                isDisturb = true;
                isComboAttack = false;
                isDodge = false;

                anim.SetTrigger("AttackDelay");
                state = MolblinState.AttackDelay;

                currentTime = 0;
            }
        
    }

    private void UpdateAttackDelay()
    {
        anim.SetBool("TwoHands", false);
        anim.SetBool("ComboAttack", false);

        currentTime += Time.deltaTime;

        if (distance < 4)
        {
            anim.SetBool("Move", false);
            anim.SetBool("Wait", false);

            int randomValue = Random.Range(0, 10);
            if (randomValue < 5)
            {
                // 발차기
                state = MolblinState.Kick;
            }
            else if (randomValue >= 5 && isDodge == false)
            {
                // 회피
                state = MolblinState.Dodge;
                isDodge = true;
            }

            return;
        }

        if (currentTime >= delayTime)
        {
            // 상태를 공격선택으로 바꾼다.
            state = MolblinState.Idle;

            currentTime = 0;
    }

    public void DamageProcess()
    {
            currentHP--;
        if (currentHP > 0)
        {
            if (isDisturb == true)
            {
                anim.SetTrigger("Damage");
                state = MolblinnState.Idle;
            }
        }

        //if (currentHP > 0)
        //{
        //    if (isDisturb == true)
        //    {
        //        // 체력을 감소시킨다.
        //        currentHP--;

        //        anim.SetTrigger("Damage");
        //        state = MolblinState.Idle;
        //    }
        //}

        else if (currentHP <= 0)
        {
            state = MolblinnState.Die;
        }      
    }

    public float power = 5;
    Rigidbody[] rbs;
    bool isDie;
    bool isEffect;
    public GameObject dieEffectFactory;
    public SkinnedMeshRenderer molClub;
    Transform hipBone;

    #region Die Porcess
    private void UpdateDie()
    {
        if (isDie == false)
        {
            isDie = true;

            // SoundManager.instance.OnMyDieSound();

            // GameManager.instance.KillcntUpdate();

            anim.enabled = false;

            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);

                rb.AddForce(Vector3.up * power, ForceMode.Impulse);
            }

            // 보스전일때 보코블린 죽으면 점령게이지 줄어듦.
            if (GameManager.instance.state == GameManager.State.Boss)
            {
                // 점령게이지 줄어듦
                GameManager.instance.BossGage.GetComponent<Slider>().value -= 80;
            }

            // 색깔을 검게 바꾸고
            Invoke("DieColor", 3);
            // 사망이펙트와 함께 게임오브젝트를 파괴한다.
            Invoke("DieEffect", 4);
        }

    }

    public void DieColor()
    {
        // 모리블린의 몸을 까맣게 한다.
        SkinnedMeshRenderer[] mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i] == molClub)     // 모리블린 무기는 까맣게 하지 않는다.
            {
                continue;
            }
            mesh[i].materials[0].color = Color.black;
        }
    }

    public void DieEffect()
    {
        if (isEffect == false)
        {
            // 파괴할 때 검은 먼지 파티클시스템을 실행한다.
            GameObject dieEffect = Instantiate(dieEffectFactory);
            dieEffect.transform.position = hipBone.position;
            isEffect = true;    // 한번만 실행할 수 있게

            // 게임오브젝트를 파괴한다.
            Destroy(gameObject);
        }
    }
    #endregion

    #region Attack Event
    public BoxCollider footBoxCollider;
    public BoxCollider clubBoxCollider;
    public void StartKick()
    {
        // 발 콜라이더 활성화
        footBoxCollider.enabled = true;
    }
    public void EndKick()
    {
        // 발 콜라이더 비활성화
        footBoxCollider.enabled = false;
    }

    public void StartTwoHandAttack()
    {
        // 무기 콜라이더 활성화
        clubBoxCollider.enabled = true;
    }
    public void EndTwoHandAttack()
    {
        // 무기 콜라이더 비활성화
        clubBoxCollider.enabled = false;
    }

    public void StartComboAttack()
    {
        // 무기 콜라이더 활성화
        clubBoxCollider.enabled = true;
    }
    public void EndComboAttack()
    {
        // 무기 콜라이더 비활성화
        clubBoxCollider.enabled = false;
    }
    #endregion

    #endregion
}