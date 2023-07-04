using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 태어날 때 대기한다.
/// 링크가 감지거리 내로 다가오면 쫒아간다.
/// 링크가 공격거리 내로 다가오면 공격시간이 될 때까지 공격준비를 한다.
/// 공격시간이 되면 링크를 공격한다.
/// 공격을 한 뒤 공격 딜레이 시간동안 기다린다.
/// 반복
/// 공격 시간이 되기 전에 링크가 공격 거리보다 멀어지면 대기하거나 쫒아간다.
/// 공격 시간이 되기 전에 링크가 공격해서 피격하면 대기 하거나 쫒아가거나 다시 공격 준비를 한다.
/// 링크의 공격을 맞으면 뒤로 날아간다.
/// 체력이 0이 아니라면 다시 일어나고,
/// 보코블린의 체력이 0이 되면 죽고 싶다.
/// </summary>


public class Bocoblin : MonoBehaviour
{
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
        Idle, Move, Attack, Damaged, Die
    }

    // 이동속도
    public float speed = 5;

    // 거리
    float distance;
    public float detectDistance;
    public float attackDistance;

    // 시간
    float currentTime;
    public float waitTime;

    // 내비게이션


    // 플레이어(링크)
    GameObject link;

    // 애니메이션
    Animator anim;

    // 체력
    public int currentHP;
    public int maxHP = 10;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        link = GameObject.Find("Link");
        currentHP = maxHP;
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(state);
        if(state == BocoblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == BocoblinState.Move)
        {
            UpdateMove();
        }
        else if (state == BocoblinState.Attack)
        {
            UpdateAttack();
        }
        else if (state == BocoblinState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == BocoblinState.Die)
        {
            UpdateDie();
        }
        Debug.Log(state);
    }

    private void UpdateIdle()
    {
        // 링크와의 거리를 구한다.
        distance = Vector3.Distance(link.transform.position, transform.position);
        // 만약 링크와의 거리가 감지 거리보다 가까우면
        if (distance < detectDistance)
        {
            // 링크를 보며 놀란다(1초)
            print("저건... 링크!!");
            // 현재시간을 흐르게 한다.
            currentTime += Time.deltaTime;
            // 다 놀랐으면 
            if (currentTime > 1)
            {
                // 상태를 Move 로 변환한다.
                state = BocoblinState.Move;
                anim.SetBool("Move", true);
                currentTime = 0;
            }
        }
        else
        {
            // Idle
        }
    }

    private void UpdateMove()
    {
        // 거리를 구한다.
         distance = Vector3.Distance(link.transform.position, transform.position);
        // 만약 거리가 감지 거리보다 크면 
        if(distance > detectDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
            anim.SetBool("Move", false);
        }
        // 만약 링크와의 거리가 공격거리보다 멀면 
        else if (distance > attackDistance)
        {
            // transform.LookAt(new Vector3(rink.transform.position.x,0,rink.transform.position.z));
            // 링크의 위치벡터를 구해서
            // 발밑을 바라볼 때 몸이 돌아가는 문제를 예방하기 위해 X 로테이션을 0으로 고정한다. 
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
            // 링크가 있는 곳을 바라본다.
            transform.LookAt(dir);
            // 링크와의 방향을 구해서
            Vector3 rinkDir = link.transform.position - transform.position;
            rinkDir.y = 0;
            rinkDir.Normalize();
            // 링크가 있는 곳으로 이동한다.
            transform.position += rinkDir * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, rink.transform.position, 0.1f);
        }
        // 링크가 공격 거리 안으로 들어오면
        else
        {
            // 공격상태로 전환한다.
            state = BocoblinState.Attack;
            anim.SetBool("Move", false);
        }
    }
    
    private void UpdateAttack()
    {
        anim.SetBool("Buff", true);
        distance = Vector3.Distance(link.transform.position, transform.position);
        // 현재시간을 흐르게 한다.
        currentTime += Time.deltaTime;
        // 만약 현재시간이 대기 시간보다 길어지면
        if (currentTime > waitTime)
        {
            // 공격!!!!!!!!!!!!!
            print("@@@@@@@@@@@@ 공격 @@@@@@@@@@@@");
            anim.SetBool("Buff", false);
            // 애니메이션 ( 대기 -> 공격 ) 실행
            anim.SetTrigger("Attack");
            // 링크의 데미지 함수를 호출한다.
            // link.gameObject.GetComponent<HP>().Ondamaged();

            // 현재시간을 초기화한다.
            currentTime = 0;

            // 상태를 초기화한다.
            state = BocoblinState.Idle;
        }
        // 대기 시간 중에 링크가 공격거리 보다 멀어진다면
        else if (currentTime < waitTime && distance > attackDistance)
        {
            // 버프 애니메이션을 중단하고
            anim.SetBool("Buff", false);

            print("도망간다~~~~");

            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
        }
    }

    public void UpdateDamaged()
    {
        // 체력을 감소시킨다.
        currentHP--;

        // 다른 애니메이션 중지, 피격애니메이션
        anim.SetBool("Damaged", true);
        anim.SetBool("Buff", false);
        anim.SetBool("Move", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (currentHP <= 0)
            {
                // 상태를 Die 로 전환한다.
                state = BocoblinState.Die;
            }
            return;
        }
    }

    private void UpdateDie()
    {
        // 1초 후에 파괴한다.
        Destroy(gameObject, 2);
        // 파괴할 때 검은 먼지 파티클시스템을 실행한다.
    }
}
