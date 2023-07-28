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
    NavMeshAgent agent;
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
        agent = GetComponentInChildren<NavMeshAgent>();      // 자식 오브젝트의 컴포넌트 중 NavMeshAgent 를 가져온다.
    }

    public GameObject buffSound;
    public GameObject attackSound;
    public GameObject dieSound;


    bool isDance;


    // Update is called once per frame
    void Update()
    {
        print("dkfjsdkjfl" + distance);

        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            //case State.Patrol: UpdatePatrol(); break;
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

        // 만약 링크와의 거리가 감지거리 보다 크다면
        if (distance > DetectDistace)
        {

        }
    }

    private void Dance()
    {
        print("Dancing");
        #region 거리재기
        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        // 만약 링크와의 거리가 공격 거리 이하가 되면
        if (distance <= DetectDistace)
        {
            state = State.Move;
            anim.SetBool("Move", true);
            isDance = false;
        }
    }
    //int targetIndex;
    //private void UpdatePatrol()
    //{
    //    // 길 정보를 알고 싶다.
    //    // 내가 길의 어떤 위치로 갈 것인지 알고 싶다.
    //    Vector3 pos = PathManager.instance.points[targetIndex].position;

    //    // 그곳으로 이동하고 싶다.
    //    agent.SetDestination(pos);

    //    // 0.1M 까지 근접했다면 도착한 것으로 하고 싶다.
    //    pos.y = transform.position.y;
    //    float distance = Vector3.Distance(transform.position, pos);
        
    //    // 도착했다면 targetIndex 를 1 증가(순환)하고 싶다.
    //    if (distance < 0.4f)
    //    {
    //        targetIndex = (targetIndex + 1) % PathManager.instance.points.Length;
    //    }

    //    #region 거리재기
    //    // 링크의 Y 값을 나의 Y 값으로 한다.
    //    Vector3 linktransform = link.transform.position;
    //    linktransform.y = transform.position.y;

    //    // 링크와의 거리를 잰다.
    //    distance = Vector3.Distance(linktransform, transform.position);
    //    #endregion

    //    // 목적지로 이동했다면 다음 목적지를 설정하여 이동한다.

    //    // 만약 링크와의 거리가 공격 거리 이하가 되면
    //    if (distance < DetectDistace)
    //    {
    //        // 상태를 Move 로 전이한다.
    //        state = State.Move;           
    //    }
    //}

    private void UpdateMove()
    {
        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        Vector3 lookrotation = agent.steeringTarget - transform.position;
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
            agent.destination = link.transform.position;

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
        agent.destination = link.transform.position;

        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
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
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        if (distance > attackDistance)
        {
            agent.isStopped = false;

            // 링크를 네비게이션의 목적지로 설정한다.
            agent.destination = link.transform.position;

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
