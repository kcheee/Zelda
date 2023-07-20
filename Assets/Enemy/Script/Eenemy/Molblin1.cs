using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        Damaged,
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

    Rigidbody rb;

    // bool
    bool isPohyo;
    bool isDodge;
    bool isAttack;
    bool isDisturb = true;
    bool isTwoHands;
    bool isComboAttack;
    bool isKick;
    bool isPohyo;
    public bool isDamaged;

    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Link");
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);
    }
    #endregion

    #region Update
    void Update()
    {
        // print("isAttack" + isAttack);
        // print("isDisturb" + isDisturb);

        #region �ٶ󺸱�
        // ��ũ�� �ִ� ������ ã�´�.
        linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();
        Quaternion linkRotate = Quaternion.LookRotation(linkDir);

        // �� ������ �ٶ󺻴�.
        transform.rotation = Quaternion.Lerp(transform.rotation, linkRotate, Time.deltaTime * 5);
        #endregion

        #region �Ÿ����
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);
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
        else if (state == MolblinState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MolblinState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        else if (state == MolblinState.Kick)
        {
            Kick();
        }
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
        else if (state == MolblinState.Damaged)
        {
            DamageProcess();
        }
        else if (state == MolblinState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region Update States

    private void UpdateIdle()
    {
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
            // 2�ʰ� ������
            if (currentTime > 2 && isPohyo)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                isDodge = false;
                state = MolblinState.Move;
                currentTime = 0;
            }
        }
    }

    private void UpdateMove()
    {
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� �־����� Idle ���·� ���ư���.
        if (distance > detectDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinState.Idle;
            anim.SetBool("Move", false);
        }

        // ���� ��ũ���� �Ÿ��� �����Ÿ����� ������ ���ݰ��ɰŸ����� �ָ� �̵��Ѵ�.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            anim.SetBool("Move", true);
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
        }

        // ��ũ�� ���� �Ÿ� ������ ������
        else if (distance < attackPossibleDistance)
        {
            // ���ݼ��û��·� ��ȯ�Ѵ�.
            state = MolblinState.AttackChoice;
        }
    }

    private void UpdateAttackChoice()
    {
        anim.SetBool("Move", true);
        // ���� �ð� �߿� ��ũ�� ���ݰ��� �Ÿ� ���� �־����ٸ� Idle        
        if (distance > attackPossibleDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinState.Idle;
            anim.SetBool("Wait", false);
        }

        // 50% Ȯ���� ��հ��� ����
        int attackRandom = Random.Range(0, 2);
        if (attackRandom == 0 && isComboAttack == false)
        {
            isTwoHands = true;

            // ��ũ �������� �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;

            // ��ũ���� �Ÿ��� ���� 2 �Ÿ� ���ϰ� �Ǹ�
            if (distance < pattern2Distance)
            {
                anim.SetBool("Wait", true);
                anim.SetBool("Move", false);
                state = MolblinState.TwoHandsAttack;
            }
        }

        // 50% Ȯ���� �޺�����  ����
        else if (attackRandom == 1 && isTwoHands == false)
        {
            isComboAttack = true;

            // ��ũ �������� �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;

            // ��ũ���� �Ÿ��� ���� 2 �Ÿ� ���ϰ� �Ǹ�
            if (distance < pattern3Distance)
            {
                anim.SetBool("Wait", true);
                anim.SetBool("Move", false);
                state = MolblinState.ComboAttack;
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

    private void UpdateDodge()
    {
        // �ִϸ��̼� ����
        anim.SetTrigger("Dodge");
        state = MolblinState.Idle;
        isDodge = false;
    }

    private void TwoHandsAttack()
    {

            // �ð��� �帣�� �Ѵ�.
            currentTime += Time.deltaTime;
            // 1�� �Ŀ�
            if (currentTime >= 1)
            {
                print("��� ����");
                anim.SetBool("TwoHands", true);

                //isDisturb = false;
                isAttack = true;

                // ��� ������ �Ѵ�.
                anim.SetBool("Wait", false);
            }
            // ������ ������ �ð��� �Ǹ�
            if (currentTime >= 1.5f)
            {
                //isDisturb = true;
                isTwoHands = false;
                isDodge = false;

                anim.SetTrigger("AttackDelay");
                state = MolblinState.AttackDelay;

                currentTime = 0;
            }
        
    }

    private void ComboAttack()
    {
  
            currentTime += Time.deltaTime;

            if (currentTime >= 1)
            {
                print("�޺� ����");

                //isDisturb = false;
                isAttack = true;

                // �޺� ������ �Ѵ�.
                anim.SetBool("Wait", false);
                anim.SetBool("ComboAttack", true);
            }
            if (currentTime >= 4)
            {

                isDisturb = true;
                isComboAttack = false;
                isDodge = false;

                anim.SetTrigger("AttackDelay");
                state = MolblinState.AttackDelay;

                currentTime = 0;
            }
        
    }

    private void UpdateAttackDelay()
    {
        anim.SetBool("TwoHands", false);
        anim.SetBool("ComboAttack", false);

        currentTime += Time.deltaTime;

        if (distance < 4)
        {
            anim.SetBool("Move", false);
            anim.SetBool("Wait", false);

            int randomValue = Random.Range(0, 10);
            if (randomValue < 5)
            {
                // ������
                state = MolblinState.Kick;
            }
            else if (randomValue >= 5 && isDodge == false)
            {
                // ȸ��
                state = MolblinState.Dodge;
                isDodge = true;
            }

            return;
        }

        if (currentTime >= delayTime)
        {
            // ���¸� ���ݼ������� �ٲ۴�.
            state = MolblinState.Idle;

            currentTime = 0;
    }

    public void DamageProcess()
    {
            currentHP--;
        if (currentHP > 0)
        {
            if (isDisturb == true)
            {
                anim.SetTrigger("Damage");
                state = MolblinnState.Idle;
            }
        }

        //if (currentHP > 0)
        //{
        //    if (isDisturb == true)
        //    {
        //        // ü���� ���ҽ�Ų��.
        //        currentHP--;

        //        anim.SetTrigger("Damage");
        //        state = MolblinState.Idle;
        //    }
        //}

        else if (currentHP <= 0)
        {
            state = MolblinnState.Die;
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

            // SoundManager.instance.OnMyDieSound();

            // GameManager.instance.KillcntUpdate();

            anim.enabled = false;

            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);

                rb.AddForce(Vector3.up * power, ForceMode.Impulse);
            }

            // �������϶� ���ں� ������ ���ɰ����� �پ��.
            if (GameManager.instance.state == GameManager.State.Boss)
            {
                // ���ɰ����� �پ��
                GameManager.instance.BossGage.GetComponent<Slider>().value -= 80;
            }

            // ������ �˰� �ٲٰ�
            Invoke("DieColor", 3);
            // �������Ʈ�� �Բ� ���ӿ�����Ʈ�� �ı��Ѵ�.
            Invoke("DieEffect", 4);
        }

    }

    public void DieColor()
    {
        // �𸮺��� ���� ��İ� �Ѵ�.
        SkinnedMeshRenderer[] mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i] == molClub)     // �𸮺� ����� ��İ� ���� �ʴ´�.
            {
                continue;
            }
            mesh[i].materials[0].color = Color.black;
        }
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

    #region Attack Event
    public BoxCollider footBoxCollider;
    public BoxCollider clubBoxCollider;
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
    #endregion

    #endregion
}