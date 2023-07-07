using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region 목표
// 태어날 때 대기한다.
// 링크가 감지거리 내로 다가오면 쫒아간다.
// 링크가 공격거리 내로 다가오면 공격시간이 될 때까지 공격준비를 한다.
// 공격시간이 되면 링크를 공격한다.
// 공격을 한 뒤 공격 딜레이 시간동안 기다린다.
// 반복
// 공격 시간이 되기 전에 링크가 공격 거리보다 멀어지면 대기하거나 쫒아간다.
// 공격 시간이 되기 전에 링크가 공격해서 피격하면 대기 하거나 쫒아가거나 다시 공격 준비를 한다.
// 링크의 공격을 맞으면 뒤로 날아간다.
// 체력이 0이 아니라면 다시 일어나고,
// 보코블린의 체력이 0이 되면 죽고 싶다.
#endregion

public class Bocoblin : MonoBehaviour
{
    #region 변수
    static public Bocoblin instance = null;
    private void Awake()
    {
        instance = this;
    }

    // 상태
    public BocoblinState state;
    // 상태 열거
    public enum BocoblinState
    {
        Idle, Move, Air, Dodge, Wait, Attack, AttackWait, Damaged, Die
    }

    // 이동속도
    public float speed = 5;
    public float runSpeed = 8;

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

    // 애니메이션
    Animator anim;

    // 체력
    public int currentHP;
    public int maxHP = 10;
    Rigidbody rb;
    
    // bool
    bool isWait;
    bool isAir;

    // 공중상태를 위한 레이
    RaycastHit hitinfo;

    Vector3 linkTransform;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Link");
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        linkTransform = link.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        //print(state);
        if (state == BocoblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == BocoblinState.Move)
        {
            UpdateMove();
        }
        else if (state == BocoblinState.Air)
        {
            UpdateAir();
        }
        else if(state == BocoblinState.Dodge)
        {
            UpdateDodge();
        }
        else if(state == BocoblinState.Wait)
        {
            UpdateWait();
        }
        else if (state == BocoblinState.Attack)
        {
            UpdateAttack();
        }
        else if (state == BocoblinState.AttackWait)
        {
            UpdateAttackWait();
        }
        else if (state == BocoblinState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == BocoblinState.Die)
        {
            UpdateDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 공중상태에 있다가 추락해서 바닥에 닿았을 때
        if (collision.gameObject.CompareTag("Floor"))
        {
            isAir = false;
            // 만약 체력이 0 이상이라면
            if (currentHP > 0)
            {
                // Idle 상태로 전환한다.
                state = BocoblinState.Idle;
                anim.SetTrigger("Idle");
                currentTime = 0;
            }
            // 만약 체력이 0 이하가 되면 
            else if (currentHP <= 0)
            {
                // 상태를 Die 로 전환한다.
                state = BocoblinState.Die;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (isAir)
        {
            return;
        }
        else if ( collision.gameObject.CompareTag("Floor"))
        { 
            // 상태를 Air 로 변환한다.
            state = BocoblinState.Air;
            // Air 애니메이션 실행
            anim.SetTrigger("Air");
            isAir = true;
        }
    }


    private void UpdateIdle()
    {
        // 보코볼린의 x,z rotation 을 0으로 해준다. = > 똑바로 설 수 있도록
        if (currentHP > 0)
        {
            Vector3 t = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, t.y, 0);
            
        }

        // 링크와의 거리를 구한다.
        distance = Vector3.Distance(link.transform.position, transform.position);
        // 만약 링크와의 거리가 감지 거리보다 가까우면
        if (distance <= detectDistance)
        {
            // 링크가 있는 방향을 찾는다.
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);

            // 그 방향을 바라본다.
            transform.LookAt(dir);

            currentTime += Time.deltaTime;
            
            // 2초가 지나면
            if (currentTime > 2)
            {
                // 상태를 Move 로 변환한다.
                state = BocoblinState.Move;
                anim.SetTrigger("Move");
                currentTime = 0;
            }
        }

    }

    private void UpdateAir()
    {
        print("######################");
    }

    private void UpdateMove()
    {
        // 거리를 구한다.
        distance = Vector3.Distance(link.transform.position, transform.position);

        // 만약 링크와의 거리가 감지 거리보다 멀어지면
        if (detectDistance < distance)
        {
            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
            anim.SetTrigger("Idle");
        }
        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면
        else if (detectDistance >= distance && distance > attackPossibleDistance)
        {
            // transform.LookAt(new Vector3(rink.transform.position.x,0,rink.transform.position.z));
            // 링크의 위치벡터를 구해서(발밑을 바라볼 때 몸이 돌아가는 문제를 예방하기 위해 X 로테이션을 0으로 고정)
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
            // 링크가 있는 곳을 바라본다.
            transform.LookAt(dir);

            // 링크와의 방향을 구해서
            linkTransform.y = 0;
            Vector3 linkDir = linkTransform - transform.position;
            linkDir.y = 0;
            linkDir.Normalize();

            // 링크가 있는 곳으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, rink.transform.position, 0.1f);
        }
        // 링크가 공격 거리 안으로 들어오면
        else if(distance <= attackPossibleDistance)
        {
            // 공격대기상태로 전환한다.
            state = BocoblinState.Wait;
            anim.SetTrigger("Wait");
        }
    }

    private void UpdateDodge()
    {
        //rb.AddForce(transform.forward * -2, ForceMode.Impulse);
        transform.position = Vector3.Lerp(transform.position, dodgePos.position, 0.8f);
        state = BocoblinState.Idle;
        anim.SetTrigger("Dodge");
    }

    private void UpdateWait()
    {
        // 시간을 흐르게 한다.
        currentTime += Time.deltaTime;

        // 거리를 구한다.
        distance = Vector3.Distance(link.transform.position, transform.position);

        // 대기 시간 중에 링크가 공격거리 보다 멀어진다면
        if (currentTime < waitTime && distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
            anim.SetTrigger("Idle");
            currentTime = 0;
        }
        // 대기 시간이 지나면
        else if (currentTime >= waitTime)
        {
            currentTime = 0;

            int rValue = Random.Range(0, 10);
            // 30% 확률로 회피
            if (rValue < 3 && isWait == false)
            {
                state = BocoblinState.Dodge;
                anim.SetTrigger("Dodge");
            }
            // 70% 확률로 공격하러 감
            else
            {
                isWait = true;

                anim.SetTrigger("Run");

                // 거리를 구한다.
                distance = Vector3.Distance(link.transform.position, transform.position);

                // AttackDistance 까지 달려감
                Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
                transform.LookAt(dir);

                linkTransform.y = 0;
                Vector3 linkDir = linkTransform - transform.position;
                linkDir.y = 0;
                linkDir.Normalize();
                transform.position += linkDir * runSpeed * Time.deltaTime;

                // 만약 공격거리보다 가까워지면
                if (distance <= attackDistance)
                {
                    state = BocoblinState.Attack;
                    // 애니메이션 실행
                    anim.SetTrigger("Attack");
                    // 시간을 초기화한다.
                    currentTime = 0;
                    isWait = false;
                }
            }
        }
    }

    bool isAttack;
    private void UpdateAttack()
    {
        // 링크의 데미지 함수를 호출한다.
        // link.gameObject.GetComponent<HP>().Ondamaged();

        state = BocoblinState.AttackWait;
        anim.SetBool("AttackWait", false);
    }

    void UpdateAttackWait()
    {
        // 거리를 구한다.
        linkTransform.y = 0;
        Vector3 linkDir = linkTransform - transform.position;

        currentTime += Time.deltaTime;

        if (currentTime < 2 && distance > attackDistance)
        {
            state = BocoblinState.Idle;
            anim.SetTrigger("Idle");
            currentTime = 0;
            currentTime = 0;
        }
        else if (currentTime > 2)
        {
            // 공격 대기 했다가 다시 때린다.
            anim.SetBool("AttackWait", true);
            state = BocoblinState.Attack;
            currentTime = 0;
        }
    }

    public void UpdateDamaged()
    {
        // 체력을 감소시킨다.
        currentHP--;
        state = BocoblinState.Air;
        anim.SetTrigger("Air");
    }

    private void UpdateDie()
    {
        GetComponent<Rigidbody>().mass = 500;
        // 1초 후에 파괴한다.
        Destroy(gameObject, 2);
        // 파괴할 때 검은 먼지 파티클시스템을 실행한다.
    }
}
