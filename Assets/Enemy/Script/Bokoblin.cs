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

    // �ð�
    float currentTime;
    float waitTime;
    float attackTime;

    // Distance
    float distance;
    public float DetectDistace = 80;     // ���� �Ÿ�
    public float chaceDistance = 60;     // ���� �Ÿ�
    public float attackDistance = 1;      // ���� �Ÿ�

    void Start()
    {
        link = GameObject.Find("Link");                     // "Link" ��� �̸��� ���ӿ�����Ʈ�� ã�ƿ´�.
        anim = GetComponent<Animator>();                    // Animator ������Ʈ�� �����´�.
        agent = GetComponentInChildren<NavMeshAgent>();      // �ڽ� ������Ʈ�� ������Ʈ �� NavMeshAgent �� �����´�.
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
        #region �Ÿ����
        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // ���� ��ũ���� �Ÿ��� �����Ÿ� ���� ũ�ٸ�
        if (distance > DetectDistace)
        {

        }
    }

    private void Dance()
    {
        print("Dancing");
        #region �Ÿ����
        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ� ���ϰ� �Ǹ�
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
    //    // �� ������ �˰� �ʹ�.
    //    // ���� ���� � ��ġ�� �� ������ �˰� �ʹ�.
    //    Vector3 pos = PathManager.instance.points[targetIndex].position;

    //    // �װ����� �̵��ϰ� �ʹ�.
    //    agent.SetDestination(pos);

    //    // 0.1M ���� �����ߴٸ� ������ ������ �ϰ� �ʹ�.
    //    pos.y = transform.position.y;
    //    float distance = Vector3.Distance(transform.position, pos);
        
    //    // �����ߴٸ� targetIndex �� 1 ����(��ȯ)�ϰ� �ʹ�.
    //    if (distance < 0.4f)
    //    {
    //        targetIndex = (targetIndex + 1) % PathManager.instance.points.Length;
    //    }

    //    #region �Ÿ����
    //    // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
    //    Vector3 linktransform = link.transform.position;
    //    linktransform.y = transform.position.y;

    //    // ��ũ���� �Ÿ��� ���.
    //    distance = Vector3.Distance(linktransform, transform.position);
    //    #endregion

    //    // �������� �̵��ߴٸ� ���� �������� �����Ͽ� �̵��Ѵ�.

    //    // ���� ��ũ���� �Ÿ��� ���� �Ÿ� ���ϰ� �Ǹ�
    //    if (distance < DetectDistace)
    //    {
    //        // ���¸� Move �� �����Ѵ�.
    //        state = State.Move;           
    //    }
    //}

    private void UpdateMove()
    {
        #region �ٶ󺸱� �� �Ÿ����
        // ��ũ�� �ٶ󺻴�.
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // ��ũ���� �Ÿ��� ���ݰŸ� ���� �ִٸ�
        if (distance > chaceDistance)
        {
            // ��Ʈ�� ���·� �����Ѵ�.
            state = State.Patrol;
        }
        else if(distance <= chaceDistance && distance > attackDistance)
        {
            // ��ũ�� �׺���̼��� �������� �����Ѵ�.
            agent.destination = link.transform.position;

            // Run �ִϸ��̼� ����
            anim.SetBool("Run", true);
            anim.SetBool("Move", false);

            // ���¸� chase �� �����Ѵ�.
            state = State.Chase;
        }
    }

    private void UpdateChase()
    {
        // ��ũ�� �׺���̼��� �������� �����Ѵ�.
        agent.destination = link.transform.position;

        #region �ٶ󺸱� �� �Ÿ����
        // ��ũ�� �ٶ󺻴�.
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
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

        #region �ٶ󺸱� �� �Ÿ����
        // ��ũ�� �ٶ󺻴�.
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        if (distance > attackDistance)
        {
            agent.isStopped = false;

            // ��ũ�� �׺���̼��� �������� �����Ѵ�.
            agent.destination = link.transform.position;

            // �ִϸ��̼��� �����Ѵ�.
            anim.SetBool("Move", true);

            // ���¸� Move �� �����Ѵ�.
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
