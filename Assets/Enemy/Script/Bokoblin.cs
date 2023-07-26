using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bokoblin : MonoBehaviour
{
    public static Bokoblin instance;
    private void Awake()
    {
        instance = this;
    }

    public enum State
    {
        Idle,
        Patrol,
        Move,
        Chase,
        Attack,
        Damaged,
        Die
    }
    public State state;

    Animator anim;
    GameObject link;
    NavMeshAgent navi;
    public Transform patrolPos;
    public Transform patrolPos2;

    // 시간
    float currentTime;
    float waitTime;
    float attackTime;

    // Distance
    float distance;
    public float DetectDistace = 80;     // 감지 거리
    public float chaceDistance = 60;     // 추적 거리
    public float attackDistance = 1;      // 공격 거리

    void Start()
    {
        link = GameObject.Find("Link");                     // "Link" 라는 이름의 게임오브젝트를 찾아온다.
        anim = GetComponent<Animator>();                    // Animator 컴포넌트를 가져온다.
        navi = GetComponentInChildren<NavMeshAgent>();      // 자식 오브젝트의 컴포넌트 중 NavMeshAgent 를 가져온다.
    }

    public GameObject buffSound;
    public GameObject attackSound;
    public GameObject dieSound;


    // Update is called once per frame
    void Update()
    {
        print("dkfjsdkjfl" + distance);

        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Patrol: UpdatePatrol(); break;
            case State.Move: UpdateMove(); break;
            case State.Chase: UpdateChase(); break;
            case State.Attack: UpdateAttack(); break;
            case State.Die: UpdateDie(); break;
        }
    }

    private void UpdateIdle()
    {
        #region 거리재기
        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // 만약 링크와의 거리가 70 보다 크다면
        if (distance > DetectDistace)
        {
            // 확률적으로
            int patrolRandomValue = Random.Range(0, 2);
            if (patrolRandomValue == 0)
            {
                // 패트롤 애니메이션을 실행한다.
                anim.SetBool("Move", true);
                // 상태를 Patrol로 전이한다.
                state = State.Patrol;
            }
            else if(patrolRandomValue == 1)
            {
                // 댄스 애니메이션을 실행한다.
                anim.SetTrigger("Dance");

                // 링크와의 거리를 잰다.
                distance = Vector3.Distance(linktransform, transform.position);
                // 만약 링크와의 거리가 공격 거리 이하가 되면
                if (distance <= DetectDistace)
                {
                    state = State.Move;
                    anim.SetBool("Move", true);
                }
            }
        }
    }

    private void UpdatePatrol()
    {
        // 일정 범위 안에서 랜덤한 좌표값을 가져온다.
        // 그 좌표를 네비게이션의 목적지로 설정한다.

        // navi.SetDestination(patrolPos.position);

        #region 거리재기
        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // 목적지로 이동했다면 다음 목적지를 설정하여 이동한다.

        // 만약 링크와의 거리가 공격 거리 이하가 되면
        if (distance < DetectDistace)
        {
            // 상태를 Move 로 전이한다.
            state = State.Move;           
        }
    }

    private void UpdateMove()
    {
        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        Vector3 lookrotation = navi.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // 링크와의 거리가 공격거리 보다 멀다면
        if (distance > chaceDistance)
        {
            // 패트롤 상태로 전이한다.
            state = State.Patrol;
        }
        else if(distance <= chaceDistance && distance > attackDistance)
        {
            // 링크를 네비게이션의 목적지로 설정한다.
            navi.destination = link.transform.position;

            // Run 애니메이션 실행
            anim.SetBool("Run", true);
            anim.SetBool("Move", false);

            // 상태를 chase 로 전이한다.
            state = State.Chase;
        }
    }

    private void UpdateChase()
    {
        // 링크를 네비게이션의 목적지로 설정한다.
        navi.destination = link.transform.position;

        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        Vector3 lookrotation = navi.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        if (distance <= attackDistance)
        {
            navi.isStopped = true;
            anim.SetTrigger("Attack");
            anim.SetBool("Run", false);
            state = State.Attack;
        }
    }

    private void UpdateAttack()
    {
        anim.SetBool("AttackWait", true);

        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        Vector3 lookrotation = navi.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        if (distance > attackDistance)
        {
            navi.isStopped = false;

            // 링크를 네비게이션의 목적지로 설정한다.
            navi.destination = link.transform.position;

            // 애니메이션을 실행한다.
            anim.SetBool("Move", true);

            // 상태를 Move 로 전이한다.
            state = State.Move;
        }
        else
        {
            currentTime += Time.deltaTime;
            if(currentTime >= 2)
            {
                anim.SetBool("AttackWait", false);
                currentTime = 0;
            }
        }
    }

    private void DamagedProcess()
    {
        throw new NotImplementedException();
    }

    private void UpdateDie()
    {
        throw new NotImplementedException();
    }
}
