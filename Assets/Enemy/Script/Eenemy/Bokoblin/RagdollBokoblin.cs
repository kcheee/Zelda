using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
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
public class RagdollBokoblin : MonoBehaviour
{
    #region instance
    static public RagdollBokoblin instance = null;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region 변수
    // 상태
    public BocoblinState state;

    // 상태 열거
    public enum BocoblinState
    {
        Idle, Move, Air, Dodge, Wait, Attack, AttackWait, Damaged, Die
    }

    // 보코볼린
    Animator anim;                          // 애니메이터
    NavMeshAgent agent;                     // 네비메시에이전트(이동)
    public SkinnedMeshRenderer bococlub;    // 보코클럽 메시렌더러(사망)
    public BoxCollider club;                // 보코클럽 박스콜라이더 (공격)
    public TrailRenderer trail;             // 보코클럽 트레일 렌더러 (공격)
    public Transform bokoRoot;

    // 플레이어(링크)
    GameObject link;

    // 이동속도
    public float speed = 5;                 // 걷기 속도
    public float runSpeed = 8;              // 달리기 속도

    // 거리
    float distance;                         // 링크 - 보코블린 거리
    public float detectDistance;            // 감지 거리
    public float attackPossibleDistance;    // 공격 가능 거리
    public float attackDistance;            // 공격 거리

    // 시간
    float currentTime;                      // 현재시간
    public float waitTime;                  // 공격 대기 시간
    public float standupTime = 6;           // 누웠다가 일어나는 시간

    // 체력
    public int currentHP;
    public int maxHP = 10;

    // 리지드바디
    Rigidbody[] rbs;                        // 랙돌에 들어있는 리지드바디들
    Transform hipBone;                      // 랙돌에 있는 힙본

    // 사망이펙트팩토리
    public GameObject dieEffectFactory;     // 사망 이펙트 공장

    // bool
    bool isDie;
    bool isWait;
    bool isEffect;

    // 다른 스크립트에서 데미지 관리변수
    static public int Damage = 1;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;                                      // 현재체력을 최대체력으로 한다.
        link = GameObject.Find("Link");                         // 링크를 찾는다.
        anim = gameObject.GetComponent<Animator>();             // 애니메이터를 가져온다.
        rbs = GetComponentsInChildren<Rigidbody>();             // 하위 오브젝트의 리지드바디를 가져온다.
        agent = GetComponentInChildren<NavMeshAgent>();         // 자식 오브젝트의 컴포넌트 중 NavMeshAgent 를 가져온다.
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);   // hipBone 위치를 가져온다.
    }
    #endregion

    #region Upadate
    // Update is called once per frame
    void Update()
    {
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
        else if (state == BocoblinState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == BocoblinState.Wait)
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
            DamagedProcess();
        }
        else if (state == BocoblinState.Die)
        {
            UpdateDie();
        }
    }
    #endregion

    #region States
    private void UpdateIdle()
    {
        agent.isStopped = false;

        // 링크와의 거리를 구한다.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        currentTime += Time.deltaTime;

        if (currentTime >= 5 && distance > detectDistance)
        {
            anim.SetBool("Dance", true);
            currentTime = 0;
        }

        // 만약 링크와의 거리가 감지 거리보다 가까우면
        else if (distance <= detectDistance)
        {
            anim.SetBool("Dance", false);


            //#region 바라보기
            //// 링크가 있는 방향을 찾는다.
            //Vector3 linkDir = link.transform.position - transform.position;
            //linkDir.y = 0;
            //linkDir.Normalize();

            //// 그 방향을 바라본다.
            //transform.forward = linkDir;
            //#endregion

            bococlub.enabled = true;

            currentTime += Time.deltaTime;

            if (currentTime >= 1)
            {
                // 상태를 Move 로 변환한다.
                state = BocoblinState.Move;

                currentTime = 0;
            }
        }
    }

    private void UpdateMove()
    {
        #region 바라보기 및 거리재기
        // 링크를 바라본다.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // 링크의 Y 값을 나의 Y 값으로 한다.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // 링크와의 거리를 잰다.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // 만약 링크와의 거리가 감지 거리보다 멀어지면 Idle 상태로 돌아간다.
        if (detectDistance < distance)
        {
            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
            anim.SetBool("Move", false);
        }

        // 만약 링크와의 거리가 감지거리보다 가깝고 공격가능거리보다 멀면 이동한다.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            agent.isStopped = false;

            // 링크가 있는 곳으로 이동한다.
            agent.destination = link.transform.position;

            // 애니메이션 실행
            anim.SetBool("Move", true);
        }

        // 링크가 공격 거리 안으로 들어오면 기다린다.
        else if (distance <= attackPossibleDistance)
        {
            agent.isStopped = true;

            // 공격대기상태로 전환한다.
            state = BocoblinState.Wait;

            // 애니메이션
            anim.SetBool("Wait", true);
            anim.SetBool("Move", false);
        }
    }

    private void UpdateAir()
    {
        transform.position = hipBone.position;

        // 5초 후에 일어난다.
        currentTime += Time.deltaTime;

        if (currentTime > standupTime)
        {
            // 애니메이터를 활성화 한다.
            anim.enabled = true;
            agent.enabled = true;

            anim.SetTrigger("StandUp");

            currentTime = 0;

            state = BocoblinState.Idle;
        }
    }

    private void UpdateDodge()
    {
        isWait = false;
        state = BocoblinState.Move;
        // 애니메이션 실행
        anim.SetBool("Dodge", false);
        anim.SetBool("Move", true);
    }

    private void UpdateWait()
    {
        // 시간을 흐르게 한다.
        currentTime += Time.deltaTime;

        #region 거리재기 및 바라보기
        // 거리를 구한다.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        // 링크를 바라본다.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
        #endregion

        // 대기 시간 중에 링크가 공격거리 보다 멀어진다면 Idle
        if (distance > attackPossibleDistance)
        {
            // 상태를 Idle 로 전환한다.
            state = BocoblinState.Idle;
            // 애니메이션 실행
            anim.SetBool("Wait", false);
            anim.SetBool("Run", false);
        }

        // 대기 시간이 지나면 Dodge or Attack
        else if (currentTime >= waitTime)
        {
            int rValue = Random.Range(0, 10);
            // 30% 확률로 회피
            if (rValue < 3 && isWait == false)
            {
                currentTime = 0;
                state = BocoblinState.Dodge;
                // 애니메이션 실행
                anim.SetBool("Dodge", true);
            }

            // 70% 확률로 공격하러 감
            else
            {
                agent.isStopped = false;
                isWait = true;

                // 애니메이션
                anim.SetBool("Wait", false);
                anim.SetBool("Run", true);

                // AttackDistance 까지 달려감
                agent.destination = link.transform.position;
                agent.speed = 7;
                // 만약 공격거리보다 가까워지면
                if (distance <= attackDistance)
                {
                    agent.isStopped = true;
                    state = BocoblinState.Attack;

                    // 애니메이션 실행
                    anim.SetBool("Attack", true);

                    isWait = false;
                }
            }

            // 시간을 초기화한다.
            currentTime = 0;
        }
    }

    private void UpdateAttack()
    {
        // 애니메이션 실행
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);

        // 보코블린의 상태를 AttackWait 으로 바꾼다.
        state = BocoblinState.AttackWait;
    }

    #region 콜라이더 & 트레일 관리
    public void StartAttack()
    {
        club.enabled = true;
    }

    public void StopAttack()
    {
        club.enabled = false;
    }

    public void StartTrail()
    {
        trail.enabled = true;
    }

    public void StopTrail()
    {
        trail.enabled = false;
    }
    #endregion

    private void UpdateAttackWait()
    {
        currentTime += Time.deltaTime;

        agent.isStopped = false;

        #region 거리재기 및 바라보기
        // 거리를 구한다.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        //// 링크가 있는 방향을 찾는다.
        //Vector3 linkDir = link.transform.position - transform.position;
        //linkDir.y = 0;
        //linkDir.Normalize();

        //// 링크를 바라본다.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
        #endregion

        // 다음 공격시간까지 대기하는 도중에 링크가 공격거리에서 멀어지면 Idle
        if (currentTime < 2 && distance > attackDistance)
        {
            // 상태를 Idle 로 바꾼다.
            state = BocoblinState.Idle;
            // 현재시간을 초기화한다.
            currentTime = 0;
            anim.SetBool("AttackWait", false);
            agent.isStopped = true;
        }

        // 4초가 지나면 다시 공격
        else if (currentTime >= 3f)
        {
            //  다시 때린다.
            // 애니메이션
            anim.SetBool("AttackWait", true);
            anim.SetBool("Attack", true);
            // 상태를 Attack 으로 바꾼다.
            state = BocoblinState.Attack;
            // 현재시간을 초기화한다.
            currentTime = 0;
        }
    }

    public void DamagedProcess()
    {
        // 애니메이터를 비활성화 한다.
        anim.enabled = false;

        // 체력 감소.
        currentHP -= Damage;

        anim.SetBool("Move", false);
        anim.SetBool("Wait", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("AttackWait", false);

        // 만약 체력이 0보다 크다면
        if (currentHP > 0)
        {
            // 데미지 1로 초기화  
            Damage = 1;

            // 공중상태로 바꾼다.
            UpdateAir();
        }
        // 만약 체력이 0이 되면
        else if (currentHP <= 0)
        {
            agent.enabled = false;
            // 사망상태로 바꾼다.
            //UpdateDie();
            state = BocoblinState.Die;
        }
    }

    #region 사망 프로세스
    private void UpdateDie()
    {
        if (isDie == false)
        {
            isDie = true;

            SoundManager.instance.OnMyDieSound();

            //GameManager.instance.KillcntUpdate();

            // 보스전일때 보코블린 죽으면 점령게이지 줄어듦.
            //if (GameManager.instance.state == GameManager.State.Boss)
            //{
            //    // 점령게이지 줄어듦
            //    GameManager.instance.BossGage.GetComponent<Slider>().value -= 1;
            //}

            // 색깔을 검게 바꾸고
            Invoke("DieColor", 3.5f);
            // 사망이펙트와 함께 게임오브젝트를 파괴한다.
            Invoke("DieEffect", 4);
        }
    }

    public void DieColor()
    {
        // 보코블린의 몸을 까맣게 한다.
        SkinnedMeshRenderer[] mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i] == bococlub)
            {
                continue;
            }
            mesh[i].materials[0].color = Color.black;
        }

        SoundManager.instance.OnMyBoomSound();
    }

    public void DieEffect()
    {
        if (isEffect == false)
        {
            isEffect = true;    // 한번만 실행할 수 있게

            // 파괴할 때 검은 먼지 파티클시스템을 실행한다.
            GameObject dieEffect = Instantiate(dieEffectFactory);
            dieEffect.transform.position = hipBone.position;

            // 게임오브젝트를 파괴한다.
            Destroy(gameObject);
        }
    }
    #endregion
    #endregion
}