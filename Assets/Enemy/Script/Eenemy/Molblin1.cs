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
        Wait, 
        Attack, 
        AttackWait, 
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
    bool isWait;
    bool isAttack;
    bool isDisturb = true;
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

    #region Upadate
    // Update is called once per frame
    void Update()
    {
        #region �ٶ󺸱�
        // ��ũ�� �ִ� ������ ã�´�.
        linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();

        // �� ������ �ٶ󺻴�.
        transform.forward = linkDir;
        #endregion

        #region �Ÿ����
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = 0;
        distance = Vector3.Distance(y, transform.position);
        #endregion


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
        else if (state == MolblinnState.Wait)
        {
            UpdateWait();
        }
        else if (state == MolblinnState.Attack)
        {
            UpdateAttack();
        }
        else if (state == MolblinnState.AttackWait)
        {
            UpdateAttackWait();
        }
        else if (state == MolblinnState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MolblinnState.Die)
        {
            UpdateDie();
        }
    }
    #endregion

    #region Updates
    private void UpdateDodge()
    {
        // rb.AddForce(transform.forward * -3, ForceMode.Impulse);
        transform.position = Vector3.Lerp(transform.position, dodgePos.position, 0.8f);
        state = MolblinnState.Idle;

        // �ִϸ��̼� ����
        //anim.SetBool("Dodge", false);
    }

    private void UpdateIdle()
    {
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        if (distance <= detectDistance)
        {
            currentTime += Time.deltaTime;

            // 2�ʰ� ������
            if (currentTime > 2)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = MolblinnState.Move;
                // anim.SetBool("Move", true);
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
            //anim.SetBool("Move", false);
        }

        // ���� ��ũ���� �Ÿ��� �����Ÿ����� ������ ���ݰ��ɰŸ����� �ָ� �̵��Ѵ�.
        else if (detectDistance > distance && distance > attackPossibleDistance)
        {
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
        }

        // ��ũ�� ���� �Ÿ� ������ ������ ��ٸ���.
        else if (distance <= attackPossibleDistance)
        {
            // ���ݴ����·� ��ȯ�Ѵ�.
            state = MolblinnState.Wait;

            // �ִϸ��̼�
            //anim.SetBool("Wait", true);
            //anim.SetBool("Move", false);
        }
    }

    private void UpdateWait()
    {
        // �ð��� �帣�� �Ѵ�.
        currentTime += Time.deltaTime;

        // ��� �ð� �߿� ��ũ�� ���ݰŸ� ���� �־����ٸ� Idle
        if (distance > attackPossibleDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MolblinnState.Idle;

            // �ִϸ��̼� ����
            //anim.SetBool("Wait", false);
            
            isWait = false;
        }

        // 30% Ȯ���� ȸ���Ѵ�.
        int dodgeValue = Random.Range(0, 10);
        if (isWait)
        {
            // �ƹ��ϵ� �Ͼ�� �ʴ´�.
            return;
        }
        
        if (dodgeValue < 0 && isWait == false)
        {
            isWait = true;
            state = MolblinnState.Dodge;
            currentTime = 0;
            // �ִϸ��̼� ����
            //anim.SetBool("Dodge", true);
        }
        else
        {
            // ��� �ð��� ������ Dodge or Attack
            if (currentTime >= waitTime)
            {
                // ��ũ�� �ִ� ������ �̵��Ѵ�.
                transform.position += linkDir * speed * Time.deltaTime;

                if (distance <= attackDistance)
                {
                    // ���� ���� �� �ϳ��� ��� �����Ѵ�.
                    state = MolblinnState.Attack;

                    // �ð��� �ʱ�ȭ�Ѵ�.
                    currentTime = 0;
                }
            }
        }        
    }

    private void UpdateAttack()
    {
        int attackValue = Random.Range(0, 5);
        // �������� 1 ����
        if (attackValue == 0 || attackValue == 1 || attackValue == 2)
        {
            print("1");

        }
        // �������� 2 ����
        else if(attackValue == 3)
        {
            print("2");
            isDisturb = false;
        }
        // �������� 3 ����
        else if(attackValue == 4)
        {
            print("3");
            isDisturb = false;
        }

        state = MolblinnState.AttackWait;
    }

    private void UpdateAttackWait()
    {
        isDisturb = true;

        currentTime += Time.deltaTime;

        // ���� ���ݽð����� ����ϴ� ���߿� ��ũ�� ���ݰŸ����� �־����� Idle
        if (currentTime < 3 && distance > attackDistance)
        {
            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;
            
            // ���¸� Idle �� �ٲ۴�.
            state = MolblinnState.Idle;

            //anim.SetBool("AttackWait", false);
        }

        // 2�ʰ� ������ �ٽ� ����
        else if (currentTime >= 2)
        {
            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;

            // �ִϸ��̼�
            anim.SetBool("AttackWait", true);
            anim.SetBool("Attack", true);

            // ���¸� Attack ���� �ٲ۴�.
            state = MolblinnState.Attack;
        }
    }

    public void UpdateDamaged()
    {
        if (isDisturb)
        {
            // ü���� ���ҽ�Ų��.
            currentHP--;

            anim.SetTrigger("Damaged");
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