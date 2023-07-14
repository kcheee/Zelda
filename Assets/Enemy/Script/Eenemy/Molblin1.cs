using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public MolblinnState state;

    // ���� ����
    public enum MolblinnState
    {
        Idle, 
        Move, 
        Dodge, 
        AttackChoice, 
        Kick,
        TwoHandsAttack,
        ComboAttack,
        Damaged,
        Die
    }

    // �̵��ӵ�
    public float speed = 3;

    // �Ÿ�
    float distance;
    public float detectDistance;
    public float attackPossibleDistance;
    public float attackDistance;
    public Transform dodgePos;

    // �ð�
    float currentTime;
    public float waitTime;

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
    bool isDodge;
    bool isAttack;
    bool isDisturb = true;
    bool isTwoHands;
    bool isComboAttack;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }
    #endregion

    #region Update
    void Update()
    {
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

        // ������
        if(distance <= 3)
        {
            isDodge = true;
            state = MolblinnState.Dodge;
            state = MolblinnState.Idle;


            anim.SetBool("Move", false);
            anim.SetBool("Kick", true);
            state = MolblinnState.Kick;
        }

        #region �����Լ�
        if (state == MolblinnState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MolblinnState.Move)
        {
            UpdateMove();
        }
        else if (state == MolblinnState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MolblinnState.AttackChoice)
        {
            UpdateAttackChoice();
        }
        else if (state == MolblinnState.Kick)
        {
            Kick();
        }
        else if(state == MolblinnState.TwoHandsAttack)
        {
            TwoHandsAttack();
        }
        else if(state == MolblinnState.ComboAttack)
        {
            ComboAttack();
        }
        else if (state == MolblinnState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MolblinnState.Die)
        {
            UpdateDie();
        }
        #endregion
    }
    #endregion

    #region Update States
    bool isPohyo = false;
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
                state = MolblinnState.Move;
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
            state = MolblinnState.Idle;
            anim.SetBool("Move", false);
        }

        // ���� ��ũ���� �Ÿ��� �����Ÿ����� ������ ���ݰ��ɰŸ����� �ָ� �̵��Ѵ�.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
            anim.SetBool("Move", true);
        }

        // ��ũ�� ���� �Ÿ� ������ ������
        else if (distance < attackPossibleDistance)                                 
        {
            // ���ݼ��û��·� ��ȯ�Ѵ�.
            state = MolblinnState.AttackChoice;
        }
    }

    private void UpdateAttackChoice()
    {
        // ���� �ð� �߿� ��ũ�� ���ݰ��� �Ÿ� ���� �־����ٸ� Idle        
        if (distance > attackPossibleDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinnState.Idle;
            anim.SetBool("Move", false);
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

                state = MolblinnState.TwoHandsAttack;
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

                state = MolblinnState.ComboAttack;
            }
        }
    }

    public float pattern2Distance = 7;
    public float pattern3Distance = 5;

    private void Kick()
    {
        print("������");
        anim.SetBool("Kick", false);
        state = MolblinnState.Idle;
    }

    private void UpdateDodge()
    {
        Debug.Log(distance);
        // �ִϸ��̼� ����
        anim.SetTrigger("Dodge");
    }

    private void TwoHandsAttack()
    {
        if (distance < 6)
        {
            state = MolblinnState.Dodge;
        }
        else
        {
            currentTime += Time.deltaTime;

            if (currentTime > 1)
            {
                print("��� ����");

                isDisturb = false;
                isAttack = true;

                // ��� ������ �Ѵ�.
                anim.SetBool("TwoHands", true);

                if (currentTime > 4)
                {
                    Debug.Log(currentTime);
                    anim.SetBool("TwoHands", false);

                    isDisturb = true;
                    isTwoHands = false;

                    // ���¸� ���ݼ������� �ٲ۴�.
                    state = MolblinnState.AttackChoice;

                    currentTime = 0;
                }
            } 
        }
    }

    private void ComboAttack()
    {
        if (distance < 3)
        {
            state = MolblinnState.Dodge;
        }
        else
        {
            currentTime += Time.deltaTime;

            if (currentTime > 1)
            {
                print("�޺� ����");

                isDisturb = false;
                isAttack = true;

                // ��� ������ �Ѵ�.
                anim.SetBool("ComboAttack", true);

                if (currentTime > 6)
                {
                    anim.SetBool("ComboAttack", false);

                    isDisturb = true;
                    isComboAttack = false;

                    // ���¸� �ʱ�ȭ �Ѵ�.
                    state = MolblinnState.AttackChoice;

                    currentTime = 0;
                }
            }            
        }
    }

    public void UpdateDamaged()
    {
        // ü���� ���ҽ�Ų��.
        currentHP--;

        if (isDisturb == true)
        {
            anim.SetTrigger("Damage");
        }
    }

    private void UpdateDie()
    {
        GetComponent<Rigidbody>().mass = 500;
        // 1�� �Ŀ� �ı��Ѵ�.
        Destroy(gameObject, 2);
        // �ı��� �� ���� ���� ��ƼŬ�ý����� �����Ѵ�.

    }
    #endregion
}