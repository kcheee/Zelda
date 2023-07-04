using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �¾ �� ����Ѵ�.
/// ��ũ�� �����Ÿ� ���� �ٰ����� �i�ư���.
/// ��ũ�� ���ݰŸ� ���� �ٰ����� ���ݽð��� �� ������ �����غ� �Ѵ�.
/// ���ݽð��� �Ǹ� ��ũ�� �����Ѵ�.
/// ������ �� �� ���� ������ �ð����� ��ٸ���.
/// �ݺ�
/// ���� �ð��� �Ǳ� ���� ��ũ�� ���� �Ÿ����� �־����� ����ϰų� �i�ư���.
/// ���� �ð��� �Ǳ� ���� ��ũ�� �����ؼ� �ǰ��ϸ� ��� �ϰų� �i�ư��ų� �ٽ� ���� �غ� �Ѵ�.
/// ��ũ�� ������ ������ �ڷ� ���ư���.
/// ü���� 0�� �ƴ϶�� �ٽ� �Ͼ��,
/// ���ں��� ü���� 0�� �Ǹ� �װ� �ʹ�.
/// </summary>


public class Bocoblin : MonoBehaviour
{
    static public Bocoblin instance = null;
    private void Awake()
    {
        instance = this;
    }

    // ����
    public BocoblinState state;
    // ���� ����
    public enum BocoblinState
    {
        Idle, Move, Attack, Damaged, Die
    }

    // �̵��ӵ�
    public float speed = 5;

    // �Ÿ�
    float distance;
    public float detectDistance;
    public float attackDistance;

    // �ð�
    float currentTime;
    public float waitTime;

    // ������̼�


    // �÷��̾�(��ũ)
    GameObject link;

    // �ִϸ��̼�
    Animator anim;

    // ü��
    public int currentHP;
    public int maxHP = 10;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        link = GameObject.Find("Link");
        currentHP = maxHP;
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(state);
        if(state == BocoblinState.Idle)
        {
            UpdateIdle();
        }
        else if (state == BocoblinState.Move)
        {
            UpdateMove();
        }
        else if (state == BocoblinState.Attack)
        {
            UpdateAttack();
        }
        else if (state == BocoblinState.Damaged)
        {
            UpdateDamaged();
        }
        else if (state == BocoblinState.Die)
        {
            UpdateDie();
        }
        Debug.Log(state);
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
            // �� ������� 
            if (currentTime > 1)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = BocoblinState.Move;
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
        if(distance > detectDistance)
        {
            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = BocoblinState.Idle;
            anim.SetBool("Move", false);
        }
        // ���� ��ũ���� �Ÿ��� ���ݰŸ����� �ָ� 
        else if (distance > attackDistance)
        {
            // transform.LookAt(new Vector3(rink.transform.position.x,0,rink.transform.position.z));
            // ��ũ�� ��ġ���͸� ���ؼ�
            // �߹��� �ٶ� �� ���� ���ư��� ������ �����ϱ� ���� X �����̼��� 0���� �����Ѵ�. 
            Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);
            // ��ũ�� �ִ� ���� �ٶ󺻴�.
            transform.LookAt(dir);
            // ��ũ���� ������ ���ؼ�
            Vector3 rinkDir = link.transform.position - transform.position;
            rinkDir.y = 0;
            rinkDir.Normalize();
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += rinkDir * speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, rink.transform.position, 0.1f);
        }
        // ��ũ�� ���� �Ÿ� ������ ������
        else
        {
            // ���ݻ��·� ��ȯ�Ѵ�.
            state = BocoblinState.Attack;
            anim.SetBool("Move", false);
        }
    }
    
    private void UpdateAttack()
    {
        anim.SetBool("Buff", true);
        distance = Vector3.Distance(link.transform.position, transform.position);
        // ����ð��� �帣�� �Ѵ�.
        currentTime += Time.deltaTime;
        // ���� ����ð��� ��� �ð����� �������
        if (currentTime > waitTime)
        {
            // ����!!!!!!!!!!!!!
            print("@@@@@@@@@@@@ ���� @@@@@@@@@@@@");
            anim.SetBool("Buff", false);
            // �ִϸ��̼� ( ��� -> ���� ) ����
            anim.SetTrigger("Attack");
            // ��ũ�� ������ �Լ��� ȣ���Ѵ�.
            // link.gameObject.GetComponent<HP>().Ondamaged();

            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;

            // ���¸� �ʱ�ȭ�Ѵ�.
            state = BocoblinState.Idle;
        }
        // ��� �ð� �߿� ��ũ�� ���ݰŸ� ���� �־����ٸ�
        else if (currentTime < waitTime && distance > attackDistance)
        {
            // ���� �ִϸ��̼��� �ߴ��ϰ�
            anim.SetBool("Buff", false);

            print("��������~~~~");

            // ���¸� Idle �� ��ȯ�Ѵ�.
            state = BocoblinState.Idle;
        }
    }

    public void UpdateDamaged()
    {
        // ü���� ���ҽ�Ų��.
        currentHP--;

        // �ٸ� �ִϸ��̼� ����, �ǰݾִϸ��̼�
        anim.SetBool("Damaged", true);
        anim.SetBool("Buff", false);
        anim.SetBool("Move", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (currentHP <= 0)
            {
                // ���¸� Die �� ��ȯ�Ѵ�.
                state = BocoblinState.Die;
            }
            return;
        }
    }

    private void UpdateDie()
    {
        // 1�� �Ŀ� �ı��Ѵ�.
        Destroy(gameObject, 2);
        // �ı��� �� ���� ���� ��ƼŬ�ý����� �����Ѵ�.
    }
}
