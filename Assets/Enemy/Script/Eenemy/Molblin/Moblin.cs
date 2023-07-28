using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region ��ǥ
 // �¾ �� ����Ѵ�.
 // ��ũ�� �����Ÿ� ���� �ٰ����� �i�ư���.
 // ��ũ�� ���ݰŸ� ���� �ٰ����� ���ݽð��� �� ������ �����غ� �Ѵ�.
 // ���ݽð��� �Ǹ� ��ũ�� ���� 3�� ���� ������ �Ѵ�.
 // ������ ���� ��ũ�� ������ �����ϰ� ������ �Ѵ�.
 // ������ ���� ���� �ٽ� ���� ��⸦ �ϰų� �i�ư��ų� �Ѵ�.
 // ���� ���Ŀ� 5~10�� ���� ���� ����Ʈ�� �����ش�.
 // ���� ����Ʈ�� 0�� �Ǹ� �������°� �ȴ�.
 // �������°� ������ �ٽ� ��ũ�� ���� 3�� ���� ������ �Ѵ�.
 // �����ϴ� ���� �ƴ� �� ��ũ���� ������ �ǰݸ���� �ϰ� �ٽ� ���� ��⸦ �ϰų� �i�ư��ų� �Ѵ�.
 // ü���� 0�� �Ǹ� �״´�. ���� Ŭ���� UI ���´�.
#endregion

public class Moblin : MonoBehaviour
{
    static public Moblin instance = null;

    private void Awake()
    {
        instance = this;
    }

    // ����
    public MoblinState state;
    // ���� ����
    public enum MoblinState
    {
        Idle, Move, Dodge, Attack, Damaged, Die
    }

    #region ����
    // �̵��ӵ�
    public float speed = 5;

    // �ִϸ�����
    public Animator anim;
    // �Ÿ�
    float distance;
    public float detectDistance;
    public float attackDistance;
    public float dodgeDistance;

    // �ð�
    float currentTime;
    public float waitTime;

    // �÷��̾�(��ũ)
    GameObject link;

    // ü��
    public int currentHP;
    public int maxHP = 10;

    // ������ٵ�
    Rigidbody rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        link = GameObject.Find("Link");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        if (state == MoblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == MoblinState.Move)
        {
            UpdateMove();
        }
        else if(state == MoblinState.Dodge)
        {
            UpdateDodge();
        }
        else if (state == MoblinState.Attack)
        {
            UpdateAttack();
        }
        else if (state == MoblinState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == MoblinState.Die)
        {
            UpdateDie();
        }
    }

    private void UpdateIdle()
    {
        // ��ũ���� �Ÿ��� ���Ѵ�.
        distance = Vector3.Distance(link.transform.position, transform.position);
        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        if (distance < detectDistance)
        {
            // ��ũ�� ���� �����(1��)
            print("����... ��ũ!!");
            // ����ð��� �帣�� �Ѵ�.
            currentTime += Time.deltaTime;
            // ��ũ�� ��ġ���͸� ���ؼ�
            // �߹��� �ٶ� �� ���� ���ư��� ������ �����ϱ� ���� X �����̼��� 0���� �����Ѵ�. 
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
            // ��ũ�� �ִ� ���� �ٶ󺻴�.
            transform.LookAt(dir);
            // �� ������� 
            if (currentTime > 1)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = MoblinState.Move;
                anim.SetBool("Move", true);
                currentTime = 0;
            }
        }
        else
        {
            // Idle
        }
    }

    private void UpdateMove()
    {
        // �Ÿ��� ���Ѵ�.
        distance = Vector3.Distance(link.transform.position, transform.position);

        // ���� �Ÿ��� ���� �Ÿ����� ũ�� 
        if (distance > detectDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MoblinState.Idle;
            anim.SetBool("Move", false);
        }
        // ���� ��ũ���� �Ÿ��� ���ݰŸ����� �ָ� 
        else if (distance > attackDistance)
        {
            // ��ũ���� ������ ���ؼ�
            Vector3 linkDir = link.transform.position - transform.position;
            linkDir.y = 0;
            linkDir.Normalize();
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, rink.transform.position, 0.1f);
        }
        // ��ũ���� �Ÿ��� ���ݰŸ����� ������
        else
        {
            // ���ݻ��·� ��ȯ�Ѵ�.
            state = MoblinState.Attack;
            anim.SetBool("Move", false);
        }
    }

    public Transform dodgePos;
    void UpdateDodge()
    {
        // Ȯ���� �ڷ� ȸ���ϱ�
        int rValue = Random.Range(0, 10);
        // 30% Ȯ����
        if (rValue < 5)
        {
            anim.SetBool("Move", false);
            anim.SetTrigger("Dodge");
            // print("�� �� ��������");
            currentTime += Time.deltaTime;
            rb.AddForce(transform.forward * -2, ForceMode.Impulse);
            //transform.position = Vector3.Lerp(transform.position, dodgePos.position, 0.8f);
            state = MoblinState.Idle;
        }
        // 70% Ȯ����
        else
        {
            // �����Ѵ�.
            print("dkfajl;kdf;adfdsfsdfdsfds");
            anim.SetBool("Move", false);
            state = MoblinState.Attack;
        }
    }

    private void UpdateAttack()
    {
        // ��ũ�� ȸ�ǰŸ����� ���������
        if (distance < dodgeDistance)
        {
            state = MoblinState.Dodge;
        }

        // ���� �߿��� ü���� ��Ƶ� �ǰݾִϸ��̼��� X
        isAttack = true;

        // 1. ��ũ���� �Ÿ� ����
        distance = Vector3.Distance(link.transform.position, transform.position);

        // 2. ����ð��� �帣�� �Ѵ�.
        currentTime += Time.deltaTime;

        // �ִϸ��̼�
        anim.SetBool("Wait", true);

        // 3. ��ٸ��� �ð� ���� ��ũ���� �Ÿ��� �־�����
        if (currentTime < waitTime && distance > attackDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = MoblinState.Idle;
            print("��������~~~~");
            currentTime = 0;
            anim.SetBool("Wait", false);
        }

        // 3-1. ���ݽð��� �ƴ� �׸��� ���� ü���� �ִ�ü���̶��
        if (currentTime > waitTime && currentHP == maxHP)   
        {
            // �������� 1 : ������
            anim.SetBool("Wait", false);
            anim.SetTrigger("Attack1");
            print("111111111111111111111111111111");
            currentTime = 0;
        }

        // 3-2. ���ݽð��� �ƴ� �׸��� ���� ü���� �ִ�ü���� �� �̻��̶��
        else if(currentTime > waitTime && currentHP >= 5 )
        {
            // �������� 2 : �������
            anim.SetBool("Wait", false);
            // �������� ������ 1�ʵ��� �������ʾƾ��Ѵ�.
            print("2222222222222222222222222222222");
            currentTime = 0;
        }

        // 3-3. ���ݽð��� �ƴ� �׸��� ���� ü���� �ִ�ü���� �� �̸��̶��
        else if (currentTime > waitTime && currentHP < 5)
        {
            // �������� 3 : 3�� ���� ����
            anim.SetBool("Wait", false);
            // �̶����� ��ũ�� ������ �����Ѵ�
            print("333333333333333333333333333333333333333");
            currentTime = 0;
        }
    }

    bool isAttack;
    public void UpdateDamaged()
    {
        // �����ϰ� ���� ���� �� : Idle, Move
        if(isAttack == false)
        {
            currentHP--;
            // �ǰ� �ִϸ��̼�
            // anim.SetTrigger("Damaged");
        }
        // �����ϰ� ���� ��
        else if(isAttack == true)
        {
            currentHP--;
        }
    }

    private void UpdateDie()
    {
        // 1�� �Ŀ� �ı��Ѵ�.
        Destroy(gameObject);
        // �ı��� �� ���� ���� ��ƼŬ�ý����� �����Ѵ�.

    }
}
