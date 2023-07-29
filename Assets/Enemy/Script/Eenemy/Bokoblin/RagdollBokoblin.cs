using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

#region ��ǥ
// �¾ �� ����Ѵ�.
// ��ũ�� �����Ÿ� ���� �ٰ����� �i�ư���.
// ��ũ�� ���ݰŸ� ���� �ٰ����� ���ݽð��� �� ������ �����غ� �Ѵ�.
// ���ݽð��� �Ǹ� ��ũ�� �����Ѵ�.
// ������ �� �� ���� ������ �ð����� ��ٸ���.
// �ݺ�
// ���� �ð��� �Ǳ� ���� ��ũ�� ���� �Ÿ����� �־����� ����ϰų� �i�ư���.
// ���� �ð��� �Ǳ� ���� ��ũ�� �����ؼ� �ǰ��ϸ� ��� �ϰų� �i�ư��ų� �ٽ� ���� �غ� �Ѵ�.
// ��ũ�� ������ ������ �ڷ� ���ư���.
// ü���� 0�� �ƴ϶�� �ٽ� �Ͼ��,
// ���ں��� ü���� 0�� �Ǹ� �װ� �ʹ�.
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

    #region ����
    // ����
    public BocoblinState state;

    // ���� ����
    public enum BocoblinState
    {
        Idle, Move, Air, Dodge, Wait, Attack, AttackWait, Damaged, Die
    }

    // ���ں���
    Animator anim;                          // �ִϸ�����
    NavMeshAgent agent;                     // �׺�޽ÿ�����Ʈ(�̵�)
    public SkinnedMeshRenderer bococlub;    // ����Ŭ�� �޽÷�����(���)
    public BoxCollider club;                // ����Ŭ�� �ڽ��ݶ��̴� (����)
    public TrailRenderer trail;             // ����Ŭ�� Ʈ���� ������ (����)
    public Transform bokoRoot;

    // �÷��̾�(��ũ)
    GameObject link;

    // �̵��ӵ�
    public float speed = 5;                 // �ȱ� �ӵ�
    public float runSpeed = 8;              // �޸��� �ӵ�

    // �Ÿ�
    float distance;                         // ��ũ - ���ں� �Ÿ�
    public float detectDistance;            // ���� �Ÿ�
    public float attackPossibleDistance;    // ���� ���� �Ÿ�
    public float attackDistance;            // ���� �Ÿ�

    // �ð�
    float currentTime;                      // ����ð�
    public float waitTime;                  // ���� ��� �ð�
    public float standupTime = 6;           // �����ٰ� �Ͼ�� �ð�

    // ü��
    public int currentHP;
    public int maxHP = 10;

    // ������ٵ�
    Rigidbody[] rbs;                        // ������ ����ִ� ������ٵ��
    Transform hipBone;                      // ������ �ִ� ����

    // �������Ʈ���丮
    public GameObject dieEffectFactory;     // ��� ����Ʈ ����

    // bool
    bool isDie;
    bool isWait;
    bool isEffect;

    // �ٸ� ��ũ��Ʈ���� ������ ��������
    static public int Damage = 1;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;                                      // ����ü���� �ִ�ü������ �Ѵ�.
        link = GameObject.Find("Link");                         // ��ũ�� ã�´�.
        anim = gameObject.GetComponent<Animator>();             // �ִϸ����͸� �����´�.
        rbs = GetComponentsInChildren<Rigidbody>();             // ���� ������Ʈ�� ������ٵ� �����´�.
        agent = GetComponentInChildren<NavMeshAgent>();         // �ڽ� ������Ʈ�� ������Ʈ �� NavMeshAgent �� �����´�.
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);   // hipBone ��ġ�� �����´�.
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

        // ��ũ���� �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        currentTime += Time.deltaTime;

        if (currentTime >= 5 && distance > detectDistance)
        {
            anim.SetBool("Dance", true);
            currentTime = 0;
        }

        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        else if (distance <= detectDistance)
        {
            anim.SetBool("Dance", false);


            //#region �ٶ󺸱�
            //// ��ũ�� �ִ� ������ ã�´�.
            //Vector3 linkDir = link.transform.position - transform.position;
            //linkDir.y = 0;
            //linkDir.Normalize();

            //// �� ������ �ٶ󺻴�.
            //transform.forward = linkDir;
            //#endregion

            bococlub.enabled = true;

            currentTime += Time.deltaTime;

            if (currentTime >= 1)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = BocoblinState.Move;

                currentTime = 0;
            }
        }
    }

    private void UpdateMove()
    {
        #region �ٶ󺸱� �� �Ÿ����
        // ��ũ�� �ٶ󺻴�.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);

        // ��ũ�� Y ���� ���� Y ������ �Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;

        // ��ũ���� �Ÿ��� ���.
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� �־����� Idle ���·� ���ư���.
        if (detectDistance < distance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = BocoblinState.Idle;
            anim.SetBool("Move", false);
        }

        // ���� ��ũ���� �Ÿ��� �����Ÿ����� ������ ���ݰ��ɰŸ����� �ָ� �̵��Ѵ�.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            agent.isStopped = false;

            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            agent.destination = link.transform.position;

            // �ִϸ��̼� ����
            anim.SetBool("Move", true);
        }

        // ��ũ�� ���� �Ÿ� ������ ������ ��ٸ���.
        else if (distance <= attackPossibleDistance)
        {
            agent.isStopped = true;

            // ���ݴ����·� ��ȯ�Ѵ�.
            state = BocoblinState.Wait;

            // �ִϸ��̼�
            anim.SetBool("Wait", true);
            anim.SetBool("Move", false);
        }
    }

    private void UpdateAir()
    {
        transform.position = hipBone.position;

        // 5�� �Ŀ� �Ͼ��.
        currentTime += Time.deltaTime;

        if (currentTime > standupTime)
        {
            // �ִϸ����͸� Ȱ��ȭ �Ѵ�.
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
        // �ִϸ��̼� ����
        anim.SetBool("Dodge", false);
        anim.SetBool("Move", true);
    }

    private void UpdateWait()
    {
        // �ð��� �帣�� �Ѵ�.
        currentTime += Time.deltaTime;

        #region �Ÿ���� �� �ٶ󺸱�
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        // ��ũ�� �ٶ󺻴�.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
        #endregion

        // ��� �ð� �߿� ��ũ�� ���ݰŸ� ���� �־����ٸ� Idle
        if (distance > attackPossibleDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = BocoblinState.Idle;
            // �ִϸ��̼� ����
            anim.SetBool("Wait", false);
            anim.SetBool("Run", false);
        }

        // ��� �ð��� ������ Dodge or Attack
        else if (currentTime >= waitTime)
        {
            int rValue = Random.Range(0, 10);
            // 30% Ȯ���� ȸ��
            if (rValue < 3 && isWait == false)
            {
                currentTime = 0;
                state = BocoblinState.Dodge;
                // �ִϸ��̼� ����
                anim.SetBool("Dodge", true);
            }

            // 70% Ȯ���� �����Ϸ� ��
            else
            {
                agent.isStopped = false;
                isWait = true;

                // �ִϸ��̼�
                anim.SetBool("Wait", false);
                anim.SetBool("Run", true);

                // AttackDistance ���� �޷���
                agent.destination = link.transform.position;
                agent.speed = 7;
                // ���� ���ݰŸ����� ���������
                if (distance <= attackDistance)
                {
                    agent.isStopped = true;
                    state = BocoblinState.Attack;

                    // �ִϸ��̼� ����
                    anim.SetBool("Attack", true);

                    isWait = false;
                }
            }

            // �ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;
        }
    }

    private void UpdateAttack()
    {
        // �ִϸ��̼� ����
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);

        // ���ں��� ���¸� AttackWait ���� �ٲ۴�.
        state = BocoblinState.AttackWait;
    }

    #region �ݶ��̴� & Ʈ���� ����
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

        #region �Ÿ���� �� �ٶ󺸱�
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        //// ��ũ�� �ִ� ������ ã�´�.
        //Vector3 linkDir = link.transform.position - transform.position;
        //linkDir.y = 0;
        //linkDir.Normalize();

        //// ��ũ�� �ٶ󺻴�.
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
        #endregion

        // ���� ���ݽð����� ����ϴ� ���߿� ��ũ�� ���ݰŸ����� �־����� Idle
        if (currentTime < 2 && distance > attackDistance)
        {
            // ���¸� Idle �� �ٲ۴�.
            state = BocoblinState.Idle;
            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;
            anim.SetBool("AttackWait", false);
            agent.isStopped = true;
        }

        // 4�ʰ� ������ �ٽ� ����
        else if (currentTime >= 3f)
        {
            //  �ٽ� ������.
            // �ִϸ��̼�
            anim.SetBool("AttackWait", true);
            anim.SetBool("Attack", true);
            // ���¸� Attack ���� �ٲ۴�.
            state = BocoblinState.Attack;
            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;
        }
    }

    public void DamagedProcess()
    {
        // �ִϸ����͸� ��Ȱ��ȭ �Ѵ�.
        anim.enabled = false;

        // ü�� ����.
        currentHP -= Damage;

        anim.SetBool("Move", false);
        anim.SetBool("Wait", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("AttackWait", false);

        // ���� ü���� 0���� ũ�ٸ�
        if (currentHP > 0)
        {
            // ������ 1�� �ʱ�ȭ  
            Damage = 1;

            // ���߻��·� �ٲ۴�.
            UpdateAir();
        }
        // ���� ü���� 0�� �Ǹ�
        else if (currentHP <= 0)
        {
            agent.enabled = false;
            // ������·� �ٲ۴�.
            //UpdateDie();
            state = BocoblinState.Die;
        }
    }

    #region ��� ���μ���
    private void UpdateDie()
    {
        if (isDie == false)
        {
            isDie = true;

            SoundManager.instance.OnMyDieSound();

            //GameManager.instance.KillcntUpdate();

            // �������϶� ���ں� ������ ���ɰ����� �پ��.
            //if (GameManager.instance.state == GameManager.State.Boss)
            //{
            //    // ���ɰ����� �پ��
            //    GameManager.instance.BossGage.GetComponent<Slider>().value -= 1;
            //}

            // ������ �˰� �ٲٰ�
            Invoke("DieColor", 3.5f);
            // �������Ʈ�� �Բ� ���ӿ�����Ʈ�� �ı��Ѵ�.
            Invoke("DieEffect", 4);
        }
    }

    public void DieColor()
    {
        // ���ں��� ���� ��İ� �Ѵ�.
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
            isEffect = true;    // �ѹ��� ������ �� �ְ�

            // �ı��� �� ���� ���� ��ƼŬ�ý����� �����Ѵ�.
            GameObject dieEffect = Instantiate(dieEffectFactory);
            dieEffect.transform.position = hipBone.position;

            // ���ӿ�����Ʈ�� �ı��Ѵ�.
            Destroy(gameObject);
        }
    }
    #endregion
    #endregion
}