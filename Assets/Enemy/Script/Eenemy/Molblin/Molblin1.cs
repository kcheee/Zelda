using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.AI;
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
    public float delayTime = 2;

    // 플레이어(링크)
    GameObject link;
    Vector3 linkDir;

    // 애니메이션
    Animator anim;

    // 체력
    public int currentHP;
    public int maxHP = 30;
    public Slider sliderHP;

    public int HP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            sliderHP.value = currentHP;
        }
    }

    // 모리블린
    SkinnedMeshRenderer[] mesh;
    NavMeshAgent agent;

    // bool
    bool isPohyo;
    bool isDodge;
    bool isTwoHands;
    bool isComboAttack;
    bool isKick;
    public bool isDamaged;
    bool isChosen;

    static public bool anim_rotation = false;

    // 다른 스크립트에서 데미지 관리변수
    static public int Damage = 1;
    #endregion

    #region Start
    void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
        link = GameObject.Find("Link");
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);
        mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Update
    void Update()
    {
        #region 링크 필살기
        // 링크가 필살기를 쓸 때 멈추기
        if (animation_T.instance.state == animation_T.ani_state.FinishAttack)
        {
            agent.speed = 0.1f;
            agent.acceleration = 0.1f;
            agent.velocity = Vector3.zero;
            anim.speed = 0.1f;
        }
        else
        {
            agent.speed = 4.5f;
            agent.acceleration = 8f;
            anim.speed = 1f;
        }
        #endregion

        #region 발차기 or 회피
        if (!isDamaged && !isComboAttack && !isTwoHands)
        {
            anim.SetBool("Move", false);

            if(7 > distance && distance > 4 && isDodge == false)
            {
                // 회피
                Dodge();
                isDodge = true;
            }
            else if(distance <= 4 && isKick == false)
            {
                // 발차기
                Kick();
                isKick = true;
            }
            //int randomValue = Random.Range(0, 10);
            //if (randomValue < 6 && isKick == false)
            //{
            //    // 발차기
            //    Kick();
            //    isKick = true;
            //}

            //else if (randomValue >= 6 && isDodge == false)
            //{
            //    // 회피
            //    Dodge();
            //    isDodge = true;
            //}
        }
        #endregion

        #region 거리재기
        // 거리를 구한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;
        distance = Vector3.Distance(linktransform, transform.position);
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
        //else if (state == MolblinState.Dodge)
        //{
        //    Dodge();
        //}
        else if (state == MolblinState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        //else if (state == MolblinState.Kick)
        //{
        //    Kick();
        //}
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
        else if (state == MolblinState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region States
    private void UpdateIdle()
    {
        isChosen = false;
        agent.isStopped = true;

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
            // 1초가 지나면
            if (currentTime > 1 && isPohyo)
            {
                agent.isStopped = false;
                // 상태를 Move 로 변환한다.
                isDodge = false;
                state = MolblinState.Move;
                currentTime = 0;
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

    private void Dodge()
    {
        // 애니메이션 실행
        anim.SetTrigger("Dodge");
        state = MolblinState.Idle;
        isDodge = false;
    }

    private void UpdateMove()
    {
        agent.isStopped = false;
        // 만약 링크와의 거리가 감지 거리보다 멀어지면 Idle 상태로 돌아간다.
        if (distance > detectDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinState.Idle;
            anim.SetBool("Move", false);
            agent.isStopped = true;
        }

        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면 이동한다.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            agent.isStopped = false;
            anim.SetBool("Move", true);
            // 링크가 있는 곳으로 이동한다.
            agent.destination = link.transform.position;
        }

        // 링크가 공격 거리 안으로 들어오면
        else if (distance < attackPossibleDistance)
        {
            // 공격선택상태로 전환한다.
            state = MolblinState.AttackChoice;
        }
        return;
    }

    private void UpdateAttackChoice()
    {
        // 선택 시간 중에 링크가 공격가능 거리 보다 멀어진다면 Idle        
        if (distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinState.Idle;
            anim.SetBool("Wait", false);
            anim.SetBool("Move", false);
            agent.isStopped = true;
        }

        if (isChosen == false)
        {
            isChosen = true;

            // 50% 확률로 양손공격 실행
            int attackRandom = Random.Range(0, 2);
            if (attackRandom == 0)
            {
                print("TwoHandsAttack");
                state = MolblinState.TwoHandsAttack;
            }
            // 50% 확률로 콤보공격  실행
            else if (attackRandom == 1)
            {
                print("ComboAttack");
                state = MolblinState.ComboAttack;
            }
        }
    }

    // 양손공격
    public void TwoHandsAttack()
    {
        anim.SetBool("Move", true);
        agent.destination = link.transform.position;

        // 링크와의 거리가 패턴 2 거리 이하가 되면
        if (distance < pattern2Distance)
        {
            isTwoHands = true;
            isDodge = true;

            // agent 멈추기
            agent.isStopped = true;

            // 애니메이션 실행
            anim.SetBool("Move", false);
            anim.SetBool("TwoHands", true);
        }
    }

    // 콤보공격
    private void ComboAttack()
    {
        anim.SetBool("Move", true);
        agent.destination = link.transform.position;

        // 링크와의 거리가 패턴 2 거리 이하가 되면
        if (distance < pattern3Distance)
        {
            isComboAttack = true;
            isDodge = true;

            // 애니메이션
            anim.SetBool("Move", false);
            anim.SetBool("ComboAttack", true);

            // agent 멈추기
            agent.isStopped = true;
        }
    }

    private void UpdateAttackDelay()
    {
        anim.SetBool("Move", false);

        isComboAttack = false;
        isTwoHands = false;
        isDodge = false;

        agent.isStopped = true;

        currentTime += Time.deltaTime;

        
        if (currentTime >= delayTime)
        {
            // 상태를 공격선택으로 바꾼다.
            state = MolblinState.Idle;
            isKick = false;
            isDodge = false;

            currentTime = 0;
        }
    }

    public void UpdateDamaged()
    {
        isDamaged = true;

        // 체력 감소
        HP -= Damage;

        // 만약 체력이 0 보다 크다면
        if (currentHP > 0)
        {
            if (isTwoHands || isComboAttack)
            {
                Damage = 1;
            }
            else
            {                
                Damage = 1;

                // 모리블린 색 변화
                MaterialChange.instance.DoDamage();

                // 애니메이션
                anim.SetTrigger("Damage");

                // 상태 초기화
                state = MolblinState.AttackDelay;
            }

            isDamaged = false;
        }

        // 그게 아니라 체력이 0 이하가 되면
        else if (currentHP <= 0)
        {
            // 상태를 사망으로 바꾼다.
            state = MolblinState.Die;
        }
    }

    #region Die Porcess
    public float power = 5;
    Rigidbody[] rbs;
    bool isDie;
    bool isEffect;
    public GameObject dieEffectFactory;
    public SkinnedMeshRenderer molClub;
    Transform hipBone;

    private void UpdateDie()
    {
        if (isDie == false)
        {
            isDie = true;

            SoundManagerMolblin.instance.OnMyDieSound();

            GameManager.instance.KillcntUpdate();

            anim.enabled = false;

            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);

                rb.AddForce(Vector3.up * power + Vector3.back * 4, ForceMode.Impulse);
            }

            // 보스전일때 보코블린 죽으면 점령게이지 줄어듦.
            if (GameManager.instance.state == GameManager.State.Boss)
            {
                // 점령게이지 줄어듦
                GameManager.instance.BossGage.GetComponent<Slider>().value -= 80;
            }

            // 색깔을 검게 바꾸고
            Invoke("DieColor", 3f);
            Invoke("BoomSound", 3.7f);
            // 사망이펙트와 함께 게임오브젝트를 파괴한다.
            Invoke("DieEffect", 4);
        }
    }

    public void DieColor()
    {
        // 모리블린의 몸을 까맣게 한다.
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i] == molClub)     // 모리블린 무기는 까맣게 하지 않는다.
            {
                continue;
            }
            mesh[i].materials[0].color = Color.black;
        }
    }

    public void BoomSound()
    {
        SoundManagerMolblin.instance.OnMyBoomSound();
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

    #region Events
    public BoxCollider footBoxCollider;
    public BoxCollider clubBoxCollider;
    public TrailRenderer trailRenderer;

    public void OnmyAttackEnd()
    {
        anim.SetBool("TwoHands", false);
        anim.SetBool("ComboAttack", false);

        state = MolblinState.AttackDelay;
    }

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

    public void StartTrail()
    {
        trailRenderer.enabled = true;
    }

    public void EndTrail()
    {
        trailRenderer.enabled = false;
    }

    public GameObject hitBoomFactory;

    public void PlayHitEffect()
    {
        SoundManagerMolblin.instance.OnMyClubBoomSound();
        GameObject hitBoom = Instantiate(hitBoomFactory);
        hitBoom.transform.position = trailRenderer.transform.position;
    }
    #endregion

    #endregion
}