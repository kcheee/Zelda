using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region 목표
 // 태어날 때 대기한다.
 // 링크가 감지거리 내로 다가오면 쫒아간다.
 // 링크가 공격거리 내로 다가오면 공격시간이 될 때까지 공격준비를 한다.
 // 공격시간이 되면 링크를 향해 3번 연속 공격을 한다.
 // 공격할 때는 링크의 공격을 무시하고 끝까지 한다.
 // 마지막 공격 이후 다시 공격 대기를 하거나 쫒아가거나 한다.
 // 공격 이후에 5~10초 동안 약점 포인트를 보여준다.
 // 약점 포인트가 0이 되면 기절상태가 된다.
 // 기절상태가 끝나면 다시 링크를 향해 3번 연속 공격을 한다.
 // 공격하는 중이 아닐 때 링크에게 맞으면 피격모션을 하고 다시 공격 대기를 하거나 쫒아가거나 한다.
 // 체력이 0이 되면 죽는다. 게임 클리어 UI 나온다.
#endregion

public class Moblin : MonoBehaviour
{
    static public Moblin instance = null;

    private void Awake()
    {
        instance = this;
    }

    // 상태
    public MoblinState state;
    // 상태 열거
    public enum MoblinState
    {
        Idle, Move, Dodge, Attack, Damaged, Die
    }

    #region 변수
    // 이동속도
    public float speed = 5;

    // 애니메이터
    public Animator anim;
    // 거리
    float distance;
    public float detectDistance;
    public float attackDistance;
    public float dodgeDistance;

    // 시간
    float currentTime;
    public float waitTime;

    // 플레이어(링크)
    GameObject link;

    // 체력
    public int currentHP;
    public int maxHP = 10;

    // 리지드바디
    Rigidbody rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Link");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        if (state == MoblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MoblinState.Move)
        {
            UpdateMove();
        }
        else if(state == MoblinState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MoblinState.Attack)
        {
            UpdateAttack();
        }
        else if (state == MoblinState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MoblinState.Die)
        {
            UpdateDie();
        }
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
            // 링크의 위치벡터를 구해서
            // 발밑을 바라볼 때 몸이 돌아가는 문제를 예방하기 위해 X 로테이션을 0으로 고정한다. 
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
            // 링크가 있는 곳을 바라본다.
            transform.LookAt(dir);
            // 다 놀랐으면 
            if (currentTime > 1)
            {
                // 상태를 Move 로 변환한다.
                state = MoblinState.Move;
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
        if (distance > detectDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MoblinState.Idle;
            anim.SetBool("Move", false);
        }
        // 만약 링크와의 거리가 공격거리보다 멀면 
        else if (distance > attackDistance)
        {
            // 링크와의 방향을 구해서
            Vector3 linkDir = link.transform.position - transform.position;
            linkDir.y = 0;
            linkDir.Normalize();
            // 링크가 있는 곳으로 이동한다.
            transform.position += linkDir * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, rink.transform.position, 0.1f);
        }
        // 링크와의 거리가 공격거리보다 가까우면
        else
        {
            // 공격상태로 전환한다.
            state = MoblinState.Attack;
            anim.SetBool("Move", false);
        }
    }

    public Transform dodgePos;
    void UpdateDodge()
    {
        // 확률로 뒤로 회피하기
        int rValue = Random.Range(0, 10);
        // 30% 확률로
        if (rValue < 5)
        {
            anim.SetBool("Move", false);
            anim.SetTrigger("Dodge");
            // print("휙 휙 피했지롱");
            currentTime += Time.deltaTime;
            rb.AddForce(transform.forward * -2, ForceMode.Impulse);
            //transform.position = Vector3.Lerp(transform.position, dodgePos.position, 0.8f);
            state = MoblinState.Idle;
        }
        // 70% 확률로
        else
        {
            // 공격한다.
            print("dkfajl;kdf;adfdsfsdfdsfds");
            anim.SetBool("Move", false);
            state = MoblinState.Attack;
        }
    }

    private void UpdateAttack()
    {
        // 링크가 회피거리보다 가까워지면
        if (distance < dodgeDistance)
        {
            state = MoblinState.Dodge;
        }

        // 공격 중에는 체력은 닳아도 피격애니메이션은 X
        isAttack = true;

        // 1. 링크와의 거리 측정
        distance = Vector3.Distance(link.transform.position, transform.position);

        // 2. 현재시간을 흐르게 한다.
        currentTime += Time.deltaTime;

        // 애니메이션
        anim.SetBool("Wait", true);

        // 3. 기다리는 시간 동안 링크와의 거리가 멀어지면
        if (currentTime < waitTime && distance > attackDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = MoblinState.Idle;
            print("도망간다~~~~");
            currentTime = 0;
            anim.SetBool("Wait", false);
        }

        // 3-1. 공격시간이 됐다 그리고 현재 체력이 최대체력이라면
        if (currentTime > waitTime && currentHP == maxHP)   
        {
            // 공격패턴 1 : 발차기
            anim.SetBool("Wait", false);
            anim.SetTrigger("Attack1");
            print("111111111111111111111111111111");
            currentTime = 0;
        }

        // 3-2. 공격시간이 됐다 그리고 현재 체력이 최대체력의 반 이상이라면
        else if(currentTime > waitTime && currentHP >= 5 )
        {
            // 공격패턴 2 : 내려찍기
            anim.SetBool("Wait", false);
            // 내려찍은 도끼가 1초동안 빠지지않아야한다.
            print("2222222222222222222222222222222");
            currentTime = 0;
        }

        // 3-3. 공격시간이 됐다 그리고 현재 체력이 최대체력의 반 미만이라면
        else if (currentTime > waitTime && currentHP < 5)
        {
            // 공격패턴 3 : 3번 연속 공격
            anim.SetBool("Wait", false);
            // 이때에는 링크의 공격을 무시한다
            print("333333333333333333333333333333333333333");
            currentTime = 0;
        }
    }

    bool isAttack;
    public void UpdateDamaged()
    {
        // 공격하고 있지 않을 때 : Idle, Move
        if(isAttack == false)
        {
            currentHP--;
            // 피격 애니메이션
            // anim.SetTrigger("Damaged");
        }
        // 공격하고 있을 때
        else if(isAttack == true)
        {
            currentHP--;
        }
    }

    private void UpdateDie()
    {
        // 1초 후에 파괴한다.
        Destroy(gameObject);
        // 파괴할 때 검은 먼지 파티클시스템을 실행한다.

    }
}
