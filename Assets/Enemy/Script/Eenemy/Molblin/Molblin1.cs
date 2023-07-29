using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Molblin1 : MonoBehaviour
{
    #region ����
    static public Molblin1 instance = null;
    private void Awake()
    {
        instance = this;
    }

    // ����
    public MolblinState state;

    // ���� ����
    public enum MolblinState
    {
        Idle,
        Move,
        Dodge,
        AttackChoice,
        Kick,
        TwoHandsAttack,
        ComboAttack,
        AttackDelay,
        Die
    }

    // �̵��ӵ�
    public float speed = 3;

    // �Ÿ�
    float distance;
    public float detectDistance;
    public float attackPossibleDistance;
    public float pattern2Distance = 7;
    public float pattern3Distance = 5;

    // �ð�
    float currentTime;
    public float waitTime;
    public float delayTime = 2;

    // �÷��̾�(��ũ)
    GameObject link;
    Vector3 linkDir;

    // �ִϸ��̼�
    Animator anim;

    // ü��
    public int currentHP;
    public int maxHP = 30;
    public Slider sliderHP;

    public int HP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            sliderHP.value = currentHP;
        }
    }

    // �𸮺�
    SkinnedMeshRenderer[] mesh;
    NavMeshAgent agent;

    // bool
    bool isPohyo;
    bool isDodge;
    bool isAttack;
    bool isTwoHands;
    bool isComboAttack;
    bool isKick;
    public bool isDamaged;
    static public bool anim_rotation = false;

    #endregion

    #region Start
    void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
        link = GameObject.Find("Link");
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);
        mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateDamaged();
        }

        #region �Ÿ����
        // �Ÿ��� ���Ѵ�.
        Vector3 linktransform = link.transform.position;
        linktransform.y = transform.position.y;
        distance = Vector3.Distance(linktransform, transform.position);
        #endregion

        #region �����Լ�
        if (state == MolblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MolblinState.Move)
        {
            UpdateMove();
        }
        //else if (state == MolblinState.Dodge)
        //{
        //    Dodge();
        //}
        else if (state == MolblinState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        //else if (state == MolblinState.Kick)
        //{
        //    Kick();
        //}
        else if (state == MolblinState.TwoHandsAttack)
        {
            TwoHandsAttack();
        }
        else if (state == MolblinState.ComboAttack)
        {
            ComboAttack();
        }
        else if (state == MolblinState.AttackDelay)
        {
            UpdateAttackDelay();
        }
        else if (state == MolblinState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region States
    private void UpdateIdle()
    {
        isChosen = false;
        agent.isStopped = true;

        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        if (distance < detectDistance)
        {
            if (isPohyo == false)
            {
                // ��ȿ�� �ϰ� 
                isPohyo = true;
                anim.SetTrigger("Buff");
            }
            currentTime += Time.deltaTime;
            // 1�ʰ� ������
            if (currentTime > 1 && isPohyo)
            {
                agent.isStopped = false;
                // ���¸� Move �� ��ȯ�Ѵ�.
                isDodge = false;
                state = MolblinState.Move;
                currentTime = 0;
            }
        }
    }

    private void Kick()
    {
        print("������");
        anim.SetTrigger("Kick");
        state = MolblinState.Idle;
        isKick = false;
    }

    private void Dodge()
    {
        // �ִϸ��̼� ����
        anim.SetTrigger("Dodge");
        state = MolblinState.Idle;
        isDodge = false;
    }

    private void UpdateMove()
    {
        agent.isStopped = false;
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� �־����� Idle ���·� ���ư���.
        if (distance > detectDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinState.Idle;
            anim.SetBool("Move", false);
            agent.isStopped = true;
        }

        // ���� ��ũ���� �Ÿ��� �����Ÿ����� ������ ���ݰ��ɰŸ����� �ָ� �̵��Ѵ�.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            agent.isStopped = false;
            anim.SetBool("Move", true);
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            agent.destination = link.transform.position;
        }

        // ��ũ�� ���� �Ÿ� ������ ������
        else if (distance < attackPossibleDistance)
        {
            // ���ݼ��û��·� ��ȯ�Ѵ�.
            state = MolblinState.AttackChoice;
        }
        return;
    }

    bool isChosen;
    private void UpdateAttackChoice()
    {
        // ���� �ð� �߿� ��ũ�� ���ݰ��� �Ÿ� ���� �־����ٸ� Idle        
        if (distance > attackPossibleDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinState.Idle;
            anim.SetBool("Wait", false);
            anim.SetBool("Move", false);
            agent.isStopped = true;
        }

        if (isChosen == false)
        {
            isChosen = true;

            // 50% Ȯ���� ��հ��� ����
            int attackRandom = Random.Range(0, 2);
            if (attackRandom == 0)
            {
                print("TwoHandsAttack");
                state = MolblinState.TwoHandsAttack;
            }
            // 50% Ȯ���� �޺�����  ����
            else if (attackRandom == 1)
            {
                print("ComboAttack");
                state = MolblinState.ComboAttack;
            }
        }
    }

    // ��հ���
    public void TwoHandsAttack()
    {
        anim.SetBool("Move", true);
        agent.destination = link.transform.position;

        // ��ũ���� �Ÿ��� ���� 2 �Ÿ� ���ϰ� �Ǹ�
        if (distance < pattern2Distance)
        {
            isTwoHands = true;
            isDodge = true;

            // agent ���߱�
            agent.isStopped = true;

            // �ִϸ��̼� ����
            anim.SetBool("Move", false);
            anim.SetBool("TwoHands", true); 
        }
    }

    private void ComboAttack()
    {
        anim.SetBool("Move", true);
        agent.destination = link.transform.position;

        // ��ũ���� �Ÿ��� ���� 2 �Ÿ� ���ϰ� �Ǹ�
        if (distance < pattern3Distance)
        {
            isComboAttack = true;
            isDodge = true;

            // �ִϸ��̼�
            anim.SetBool("Move", false);
            anim.SetBool("ComboAttack", true);

            // agent ���߱�
            agent.isStopped = true;
        }
    }

    public void OnmyAttackEnd()
    {
        anim.SetBool("TwoHands", false);
        anim.SetBool("ComboAttack", false);

        state = MolblinState.AttackDelay;
    }

    private void UpdateAttackDelay()
    {
        anim.SetBool("Move", false);

        isComboAttack = false;
        isTwoHands = false;

        agent.isStopped = true;

        currentTime += Time.deltaTime;

        if (distance < 4)
        {
            anim.SetBool("Move", false);

            int randomValue = Random.Range(0, 10);
            if (randomValue < 5 && isKick == false)
            {
                // ������
                Kick();
                isKick = true;
            }

            else if (randomValue >= 5 && isDodge == false)
            {
                // ȸ��
                Dodge();
                isDodge = true;
            }
        }

        if (currentTime >= delayTime)
        {
            // ���¸� ���ݼ������� �ٲ۴�.
            state = MolblinState.Idle;

            currentTime = 0;
        }
    }

    public void UpdateDamaged()
    {
        // ü�� ����
        HP--;

        // ���� ü���� 0 ���� ũ�ٸ�
        if (currentHP > 0)
        {
            if (isTwoHands || isComboAttack)
            {
                print("332423432423423432");
            }
            else
            {
                // �𸮺� �� ��ȭ
                MaterialChange.instance.DoDamage();

                // �ִϸ��̼�
                anim.SetTrigger("Damage");

                // ���� �ʱ�ȭ
                state = MolblinState.Idle;
            }
        }

        // �װ� �ƴ϶� ü���� 0 ���ϰ� �Ǹ�
        else if (currentHP <= 0)
        {
            // ���¸� ������� �ٲ۴�.
            state = MolblinState.Die;
        }
    }

    public float power = 5;
    Rigidbody[] rbs;
    bool isDie;
    bool isEffect;
    public GameObject dieEffectFactory;
    public SkinnedMeshRenderer molClub;
    Transform hipBone;

    #region Die Porcess
    private void UpdateDie()
    {
        if (isDie == false)
        {
            isDie = true;

            SoundManagerMolblin.instance.OnMyDieSound();

            // GameManager.instance.KillcntUpdate();

            anim.enabled = false;

            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);

                rb.AddForce(Vector3.up * power, ForceMode.Impulse);
            }

            //// �������϶� ���ں� ������ ���ɰ����� �پ��.
            //if (GameManager.instance.state == GameManager.State.Boss)
            //{
            //    // ���ɰ����� �پ��
            //    GameManager.instance.BossGage.GetComponent<Slider>().value -= 80;
            //}

            // ������ �˰� �ٲٰ�
            Invoke("DieColor", 3f);
            Invoke("BoomSound", 3.7f);
            // �������Ʈ�� �Բ� ���ӿ�����Ʈ�� �ı��Ѵ�.
            Invoke("DieEffect", 4);
        }
    }

    public void DieColor()
    {
        // �𸮺��� ���� ��İ� �Ѵ�.
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i] == molClub)     // �𸮺� ����� ��İ� ���� �ʴ´�.
            {
                continue;
            }
            mesh[i].materials[0].color = Color.black;
        }
    }

    public void BoomSound()
    {
        SoundManagerMolblin.instance.OnMyBoomSound();
    }

    public void DieEffect()
    {
        if (isEffect == false)
        {
            // �ı��� �� ���� ���� ��ƼŬ�ý����� �����Ѵ�.
            GameObject dieEffect = Instantiate(dieEffectFactory);
            dieEffect.transform.position = hipBone.position;
            isEffect = true;    // �ѹ��� ������ �� �ְ�

            // ���ӿ�����Ʈ�� �ı��Ѵ�.
            Destroy(gameObject);
        }
    }
    #endregion

    #region Event
    public BoxCollider footBoxCollider;
    public BoxCollider clubBoxCollider;
    public TrailRenderer trailRenderer;

    public void StartKick()
    {
        // �� �ݶ��̴� Ȱ��ȭ
        footBoxCollider.enabled = true;
    }
    public void EndKick()
    {
        // �� �ݶ��̴� ��Ȱ��ȭ
        footBoxCollider.enabled = false;
    }

    public void StartTwoHandAttack()
    {
        // ���� �ݶ��̴� Ȱ��ȭ
        clubBoxCollider.enabled = true;
    }
    public void EndTwoHandAttack()
    {
        // ���� �ݶ��̴� ��Ȱ��ȭ
        clubBoxCollider.enabled = false;
    }

    public void StartComboAttack()
    {
        // ���� �ݶ��̴� Ȱ��ȭ
        clubBoxCollider.enabled = true;
    }
    public void EndComboAttack()
    {
        // ���� �ݶ��̴� ��Ȱ��ȭ
        clubBoxCollider.enabled = false;
    }

    public void StartTrail()
    {
        trailRenderer.enabled = true;
    }

    public void EndTrail()
    {
        trailRenderer.enabled = false;
    }

    public GameObject hitBoomFactory;

    public void PlayHitEffect()
    {
        GameObject hitBoom = Instantiate(hitBoomFactory);
        hitBoom.transform.position = trailRenderer.transform.position;
    }
    #endregion

    #endregion
}