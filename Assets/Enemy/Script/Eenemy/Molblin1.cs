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

        #region ȸ���ϱ�
        // ���� ��ũ�� 3M ���� �� ������ �ٰ��´ٸ�
        if(distance < 3)
        {
            if (isWait || isAttack)
            {
                // �ƹ��ϵ� �Ͼ�� �ʴ´�.
                return;
            }

            // 30% Ȯ���� ȸ���Ѵ�.
            int dodgeValue = Random.Range(0, 10);
            if (dodgeValue < 3 && isWait == false)
            {
                isWait = true;
                state = MolblinnState.Dodge;
                currentTime = 0;
                // �ִϸ��̼� ����
                anim.SetTrigger("Dodge");
            }
            // ������ Ȯ����
            else
            {
                state = MolblinnState.Attack;
                isWait = false;
            }
        }
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
        state = MolblinnState.Idle;
        isWait = false;
    }

    bool isPohyo = false;
    private void UpdateIdle()
    {
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        if (distance <= detectDistance)
        {
            currentTime += Time.deltaTime;
            if (isPohyo == false)
            {
                // ��ȿ�� �ϰ� 
                isPohyo = true;
                anim.SetTrigger("Buff");
            }

            // 3�ʰ� ������
            if (currentTime > 3 && isPohyo)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = MolblinnState.Move;
                anim.SetBool("Move", true);
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
        }

        // ��ũ�� ���� �Ÿ� ������ ������ ��ٸ���.
        else if (distance <= attackPossibleDistance)
        {
            // ���ݴ����·� ��ȯ�Ѵ�.
            state = MolblinnState.Wait;

            // �ִϸ��̼�
            anim.SetBool("Move", false);
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
            
            isWait = false;
        }
        // ��� �ð��� ������ Dodge or Attack
        if (currentTime >= waitTime)
        {
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
            anim.SetBool("Move", true);

            if (distance <= attackDistance)
            {
                anim.SetBool("Move", false);

                // ���� ���� �� �ϳ��� ��� �����Ѵ�.
                state = MolblinnState.Attack;

                // �ð��� �ʱ�ȭ�Ѵ�.
                currentTime = 0;
            }
        }   
    }

    private void UpdateAttack()
    {
        isAttack = true;
        // ��ũ�� �𸮺��� ������ ���Ϸ��� ��û ������ �ٰ�����
        if (distance <= 3)  
        {
            // �������� 1 ����
            anim.SetTrigger("Kick");
            Kick();
        }
        // �װ� �ƴ϶��
        else
        {
            int attackValue = Random.Range(0, 5);
            // 60% Ȯ���� �������� 2 ����
            if (attackValue < 3)
            {
                anim.SetTrigger("TwoHands");
                TwoHandsAttack();
            }

            // 40% Ȯ���� �������� 3 ����
            else
            {
                anim.SetTrigger("ComboAttack");
                ComboAttack();
            }
        }
    }

    private void Kick()
    {
        print("1");

        currentTime += Time.deltaTime;
        if(currentTime >= 2)
        {
            isAttack = false;
            state = MolblinnState.Idle;
            currentTime = 0;
        }        
    }

    private void TwoHandsAttack()
    {
        print("2");
        
        currentTime += Time.deltaTime;
        if (currentTime >= 3)
        {
            isAttack = false;
            isDisturb = false;
            state = MolblinnState.Idle;
            currentTime = 0;
        }
    }

    private void ComboAttack()
    {        
        print("3");
        
        currentTime += Time.deltaTime;
        if (currentTime >= 5)
        {
            isAttack = false;
            isDisturb = false;
            state = MolblinnState.Idle;
            currentTime = 0;
        }
    }

    private void UpdateAttackWait()
    {
        isAttack = false;
        isDisturb = true;
        state = MolblinnState.Idle;

        //// ���� ���ݽð����� ����ϴ� ���߿� ��ũ�� ���ݰŸ����� �־����� Idle
        //if (currentTime < 3 && distance > attackDistance)
        //{
        //    // ����ð��� �ʱ�ȭ�Ѵ�.
        //    currentTime = 0;
            
        //    // ���¸� Idle �� �ٲ۴�.
        //    state = MolblinnState.Idle;

        //    //anim.SetBool("AttackWait", false);
        //}

        //// 2�ʰ� ������ �ٽ� ����
        //else if (currentTime >= 2)
        //{
        //    // ����ð��� �ʱ�ȭ�Ѵ�.
        //    currentTime = 0;

        //    // �ִϸ��̼�
        //    anim.SetBool("AttackWait", true);
        //    anim.SetBool("Attack", true);

        //    // ���¸� Attack ���� �ٲ۴�.
        //    state = MolblinnState.Attack;
        //}
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