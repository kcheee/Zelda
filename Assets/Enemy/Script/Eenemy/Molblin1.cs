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
        Wait, 
        Attack, 
        AttackWait, 
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
    bool isWait;
    bool isAttack;
    bool isDisturb = true;
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

    #region Upadate
    // Update is called once per frame
    void Update()
    {
        #region 바라보기
        // 링크가 있는 방향을 찾는다.
        linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();

        // 그 방향을 바라본다.
        transform.forward = linkDir;
        #endregion

        #region 거리재기
        // 거리를 구한다.
        Vector3 y = link.transform.position;
        y.y = 0;
        distance = Vector3.Distance(y, transform.position);
        #endregion


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
        else if (state == MolblinnState.Wait)
        {
            UpdateWait();
        }
        else if (state == MolblinnState.Attack)
        {
            UpdateAttack();
        }
        else if (state == MolblinnState.AttackWait)
        {
            UpdateAttackWait();
        }
        else if (state == MolblinnState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MolblinnState.Die)
        {
            UpdateDie();
        }
    }
    #endregion

    #region Updates
    private void UpdateDodge()
    {
        // rb.AddForce(transform.forward * -3, ForceMode.Impulse);
        transform.position = Vector3.Lerp(transform.position, dodgePos.position, 0.8f);
        state = MolblinnState.Idle;

        // 애니메이션 실행
        //anim.SetBool("Dodge", false);
    }

    private void UpdateIdle()
    {
        // 만약 링크와의 거리가 감지 거리보다 가까우면
        if (distance <= detectDistance)
        {
            currentTime += Time.deltaTime;

            // 2초가 지나면
            if (currentTime > 2)
            {
                // 상태를 Move 로 변환한다.
                state = MolblinnState.Move;
                // anim.SetBool("Move", true);
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
            //anim.SetBool("Move", false);
        }

        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면 이동한다.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            // 링크가 있는 곳으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;
        }

        // 링크가 공격 거리 안으로 들어오면 기다린다.
        else if (distance <= attackPossibleDistance)
        {
            // 공격대기상태로 전환한다.
            state = MolblinnState.Wait;

            // 애니메이션
            //anim.SetBool("Wait", true);
            //anim.SetBool("Move", false);
        }
    }

    private void UpdateWait()
    {
        // 시간을 흐르게 한다.
        currentTime += Time.deltaTime;

        // 대기 시간 중에 링크가 공격거리 보다 멀어진다면 Idle
        if (distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MolblinnState.Idle;

            // 애니메이션 실행
            //anim.SetBool("Wait", false);
            
            isWait = false;
        }

        // 30% 확률로 회피한다.
        int dodgeValue = Random.Range(0, 10);
        if (isWait)
        {
            // 아무일도 일어나지 않는다.
            return;
        }
        
        if (dodgeValue < 0 && isWait == false)
        {
            isWait = true;
            state = MolblinnState.Dodge;
            currentTime = 0;
            // 애니메이션 실행
            //anim.SetBool("Dodge", true);
        }
        else
        {
            // 대기 시간이 지나면 Dodge or Attack
            if (currentTime >= waitTime)
            {
                // 링크가 있는 곳까지 이동한다.
                transform.position += linkDir * speed * Time.deltaTime;

                if (distance <= attackDistance)
                {
                    // 공격 패턴 중 하나를 골라 실행한다.
                    state = MolblinnState.Attack;

                    // 시간을 초기화한다.
                    currentTime = 0;
                }
            }
        }        
    }

    private void UpdateAttack()
    {
        int attackValue = Random.Range(0, 5);
        // 공격패턴 1 실행
        if (attackValue == 0 || attackValue == 1 || attackValue == 2)
        {
            print("1");

        }
        // 공격패턴 2 실행
        else if(attackValue == 3)
        {
            print("2");
            isDisturb = false;
        }
        // 공격패턴 3 실행
        else if(attackValue == 4)
        {
            print("3");
            isDisturb = false;
        }

        state = MolblinnState.AttackWait;
    }

    private void UpdateAttackWait()
    {
        isDisturb = true;

        currentTime += Time.deltaTime;

        // 다음 공격시간까지 대기하는 도중에 링크가 공격거리에서 멀어지면 Idle
        if (currentTime < 3 && distance > attackDistance)
        {
            // 현재시간을 초기화한다.
            currentTime = 0;
            
            // 상태를 Idle 로 바꾼다.
            state = MolblinnState.Idle;

            //anim.SetBool("AttackWait", false);
        }

        // 2초가 지나면 다시 공격
        else if (currentTime >= 2)
        {
            // 현재시간을 초기화한다.
            currentTime = 0;

            // 애니메이션
            anim.SetBool("AttackWait", true);
            anim.SetBool("Attack", true);

            // 상태를 Attack 으로 바꾼다.
            state = MolblinnState.Attack;
        }
    }

    public void UpdateDamaged()
    {
        if (isDisturb)
        {
            // 체력을 감소시킨다.
            currentHP--;

            anim.SetTrigger("Damaged");
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