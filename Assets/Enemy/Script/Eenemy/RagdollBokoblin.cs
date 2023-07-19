using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    #region ����
    static public RagdollBokoblin instance = null;
    private void Awake()
    {
        instance = this;
    }

    // ����
    public BocoblinState state;

    // ���� ����
    public enum BocoblinState
    {
        Idle, Move, Air, Dodge, Wait, Attack, AttackWait, Damaged, Die
    }

    // �̵��ӵ�
    public float speed = 5;
    public float runSpeed = 8;

    // �Ÿ�
    float distance;                         // ��ũ - ���ں� �Ÿ�
    public float detectDistance;            // ���� �Ÿ�
    public float attackPossibleDistance;    // ���� ���� �Ÿ�
    public float attackDistance;            // ���� �Ÿ�

    // �ð�
    float currentTime;
    public float waitTime;

    // �÷��̾�(��ũ)
    GameObject link;

    // �ִϸ��̼�
    Animator anim;

    // ���ں��� Ŭ��
    public SkinnedMeshRenderer bococlub;
    public BoxCollider club;

    // ü��
    public int currentHP;
    public int maxHP = 10;

    // �������Ʈ���丮
    public GameObject dieEffectFactory;
    
    // ������ٵ�
    Rigidbody[] rbs;
    Transform hipBone;

    // bool
    bool isWait;
    bool isAir;
    bool isAttack;
    bool isEffect;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;                                      // ����ü���� �ִ�ü������ �Ѵ�.
        link = GameObject.Find("Link");                         // ��ũ�� ã�´�.
        anim = gameObject.GetComponent<Animator>();             // �ִϸ����͸� �����´�.
        rbs = GetComponentsInChildren<Rigidbody>();             // ���� ������Ʈ�� ������ٵ� �����´�.
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

    private void UpdateIdle()
    {
        SoundManager.instance.isBuffSoundPlaying = false;
        SoundManager.instance.isAttackSoundPlaying = false;
        SoundManager.instance.isDieSoundPlaying = false;

        // ��ũ���� �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        // ���� ��ũ���� �Ÿ��� ���� �Ÿ����� ������
        if (distance <= detectDistance)
        {
            #region �ٶ󺸱�
            // ��ũ�� �ִ� ������ ã�´�.
            Vector3 linkDir = link.transform.position - transform.position;
            linkDir.y = 0;
            linkDir.Normalize();

            // �� ������ �ٶ󺻴�.
            transform.forward = linkDir;
            #endregion

            currentTime += Time.deltaTime;

            // 2�ʰ� ������
            if (currentTime > 2)
            {
                // ���¸� Move �� ��ȯ�Ѵ�.
                state = BocoblinState.Move;
                currentTime = 0;
            }
        }
    }

    private void UpdateMove()
    {
        #region �Ÿ����
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);
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
            #region �ٶ󺸱� �� �̵�
            // ��ũ�� �ִ� ������ ã�´�.
            Vector3 linkDir = link.transform.position - transform.position;
            linkDir.y = 0;
            linkDir.Normalize();

            // �� ������ �ٶ󺻴�.
            transform.forward = linkDir;
            anim.SetBool("Move", true);
            // ��ũ�� �ִ� ������ �̵��Ѵ�.
            transform.position += linkDir * speed * Time.deltaTime;
            #endregion
        }

        // ��ũ�� ���� �Ÿ� ������ ������ ��ٸ���.
        else if (distance <= attackPossibleDistance)
        {
            if (SoundManager.instance.isBuffSoundPlaying == false)
            {
                SoundManager.instance.OnMyBuffSound();
            }

            // ���ݴ����·� ��ȯ�Ѵ�.
            state = BocoblinState.Wait;

            // �ִϸ��̼�
            anim.SetBool("Wait", true);
            anim.SetBool("Move", false);

        }
    }

    private void UpdateAir()
    {
        // 5�� �Ŀ� �Ͼ��.
        currentTime += Time.deltaTime;
        if (currentTime > 6)
        {           
            // �ִϸ����͸� Ȱ��ȭ �Ѵ�.
            anim.enabled = true;

            transform.position = new Vector3(hipBone.position.x, transform.position.y, hipBone.position.z);

            anim.SetTrigger("StandUp");
            anim.SetBool("Wait", false);
            anim.SetBool("Move", false);
            anim.SetBool("Run", false);
            anim.SetBool("AttackWait", false);
            anim.SetBool("Dodge", false);

            state = BocoblinState.Idle;
            
            currentTime = 0;
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

        // ��ũ�� �ִ� ������ ã�´�.
        Vector3 linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();

        // �� ������ �ٶ󺻴�.
        transform.forward = linkDir;
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
                isWait = true;

                // �ִϸ��̼�
                anim.SetBool("Wait", false);
                anim.SetBool("Run", true);

                // AttackDistance ���� �޷���
                transform.position += linkDir * runSpeed * Time.deltaTime;

                // ���� ���ݰŸ����� ���������
                if (distance <= attackDistance)
                {
                    state = BocoblinState.Attack;

                    // �ִϸ��̼� ����
                    anim.SetBool("Attack", true);

                    // �ð��� �ʱ�ȭ�Ѵ�.
                    currentTime = 0;

                    isWait = false;
                }
            }
        }
    }

    private void UpdateAttack()
    {
        if (SoundManager.instance.isAttackSoundPlaying == false)
        {
            SoundManager.instance.OnMyAttackSound();
        }

        // �ִϸ��̼� ����
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);

        // ���ں��� ���¸� AttackWait ���� �ٲ۴�.
        state = BocoblinState.AttackWait;
    }


    public void Attack()
    {
        club.enabled = true;
    }

    private void UpdateAttackWait()
    {
        SoundManager.instance.isAttackSoundPlaying = false;

        currentTime += Time.deltaTime;

        #region �Ÿ���� �� �ٶ󺸱�
        // �Ÿ��� ���Ѵ�.
        Vector3 y = link.transform.position;
        y.y = transform.position.y;
        distance = Vector3.Distance(y, transform.position);

        // ��ũ�� �ִ� ������ ã�´�.
        Vector3 linkDir = link.transform.position - transform.position;
        linkDir.y = 0;
        linkDir.Normalize();

        // �� ������ �ٶ󺻴�.
        transform.forward = linkDir;
        #endregion

        // ���� ���ݽð����� ����ϴ� ���߿� ��ũ�� ���ݰŸ����� �־����� Idle
        if (currentTime < 2 && distance > attackDistance)
        {
            // ���¸� Idle �� �ٲ۴�.
            state = BocoblinState.Idle;
            // ����ð��� �ʱ�ȭ�Ѵ�.
            currentTime = 0;
            anim.SetBool("AttackWait", false);
        }

        // 4�ʰ� ������ �ٽ� ����
        else if (currentTime >= 4)
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
        
        // ü���� 1 �����Ѵ�.
        currentHP--;



        foreach (Rigidbody rb in rbs)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.AddForce(transform.up * 5, ForceMode.Impulse);
            Debug.Log("����");
        }

        // ���� ü���� 0���� ũ�ٸ�
        if (currentHP > 0)
        {
            // ���߻��·� �ٲ۴�.
            state = BocoblinState.Air;
        }
        // ���� ü���� 0�� �Ǹ�
        else if (currentHP <= 0)
        {
            // ������·� �ٲ۴�.
            state = BocoblinState.Die;
        }
    }

    bool isDie;

    private void UpdateDie()
    {
        if(isDie == false)
        {
            isDie = true;

            if (SoundManager.instance.isDieSoundPlaying == false)
            {
                SoundManager.instance.OnMyDieSound();
            }

            GameManager.instance.KillcntUpdate();

            // �������϶� ���ں� ������ ���ɰ����� �پ��.
            if(GameManager.instance.state == GameManager.State.Boss)
            {
                // ���ɰ����� �پ��
                GameManager.instance.BossGage.GetComponent<Slider>().value -= 4;
            }

            // ������ �˰� �ٲٰ�
            Invoke("DieColor", 3);
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
}
