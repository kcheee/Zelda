using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public MolblinnState state;

    // 상태 열거
    public enum MolblinnState
    {
        Idle, 
        Move, 
        Dodge, 
        AttackChoice, 
        Kick,
        TwoHandsAttack,
        ComboAttack,
        Damaged,
        Die
    }

    // 이동속도
    public float speed = 3;

    // 거리
    float distance;
    public float detectDistance;
    public float attackPossibleDistance;
    public float attackDistance;
    public Transform dodgePos;

    // 시간
    float currentTime;
    public float waitTime;

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
    bool isDodge;
    bool isAttack;
    bool isDisturb = true;
    bool isTwoHands;
    bool isComboAttack;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }
    #endregion

    #region Update
    void Update()
    {
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

        // 발차기
        if(distance <= 3)
        {
            isDodge = true;
            anim.SetBool("Move", false);
            anim.SetTrigger("Kick");
            state = MolblinnState.Kick;
        }

        #region 상태함수
        if (state == MolblinnState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MolblinnState.Move)
        {
            UpdateMove();
        }
        else if (state == MolblinnState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MolblinnState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        else if (state == MolblinnState.Kick)
        {
            Kick();
        }
        else if(state == MolblinnState.TwoHandsAttack)
        {
            TwoHandsAttack();
        }
        else if(state == MolblinnState.ComboAttack)
        {
            ComboAttack();
        }
        else if (state == MolblinnState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MolblinnState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region Update States
    bool isPohyo = false;
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
                state = MolblinnState.Move;
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
            state = MolblinnState.Idle;
            anim.SetBool("Move", false);
        }

        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면 이동한다.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            // 링크가 있는 곳으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;
            anim.SetBool("Move", true);
        }

        // 링크가 공격 거리 안으로 들어오면
        else if (distance < attackPossibleDistance)                                 
        {
            // 공격선택상태로 전환한다.
            state = MolblinnState.AttackChoice;
            anim.SetBool("Move", false);
            anim.SetBool("Wait", true);

        }
    }

    private void UpdateAttackChoice()
    {
        // 선택 시간 중에 링크가 공격가능 거리 보다 멀어진다면 Idle        
        if (distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinnState.Idle;
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
                state = MolblinnState.TwoHandsAttack;
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
                state = MolblinnState.ComboAttack;
            }
        }
    }

    public float pattern2Distance = 7;
    public float pattern3Distance = 5;

    private void Kick()
    {
        print("발차기");
        state = MolblinnState.Idle;
    }

    private void UpdateDodge()
    {
        // 애니메이션 실행
        anim.SetTrigger("Dodge");
        anim.SetBool("Wait", false);
        state = MolblinnState.Idle;
        isDodge = false;
    }

    private void TwoHandsAttack()
    {
        if (distance < 6 && isDodge == false)
        {
            state = MolblinnState.Dodge;
            isDodge = true;
        }
        else
        {
            // 시간을 흐르게 한다.
            currentTime += Time.deltaTime;
            // 1초 후에
            if (currentTime > 1)
            {
                print("양손 공격");

                isDisturb = false;
                isAttack = true;

                // 양손 공격을 한다.
                anim.SetTrigger("TwoHands");

                // 공격이 끝나는 시간이 되면
                if (currentTime > 4)
                {
                    isDisturb = true;
                    isTwoHands = false;

                    // 상태를 공격선택으로 바꾼다.
                    state = MolblinnState.AttackChoice;
                    anim.SetBool("Wait", true);

                    currentTime = 0;

                    isDodge = false;
                }
            } 
        }
    }

    private void ComboAttack()
    {
        if (distance < 3 && isDodge == false)
        {
            state = MolblinnState.Dodge;
            anim.SetBool("ComboAttack", false);
            isDodge = true;
        }
        else
        {
            currentTime += Time.deltaTime;

            if (currentTime > 1)
            {
                print("콤보 공격");

                isDisturb = false;
                isAttack = true;

                // 양손 공격을 한다.
                anim.SetTrigger("ComboAttack");

                if (currentTime > 6)
                {

                    isDisturb = true;
                    isComboAttack = false;

                    // 상태를 초기화 한다.
                    state = MolblinnState.AttackChoice;
                    anim.SetBool("Wait", true);

                    currentTime = 0;

                    isDodge = false;
                }
            }            
        }
    }

    public void UpdateDamaged()
    {
        // 체력을 감소시킨다.
        currentHP--;

        if (isDisturb == true)
        {
            anim.SetTrigger("Damage");
        }
    }

    private void UpdateDie()
    {
        GetComponent<Rigidbody>().mass = 500;
        // 1초 후에 파괴한다.
        Destroy(gameObject, 2);
        // 파괴할 때 검은 먼지 파티클시스템을 실행한다.

    }
    #endregion
}