using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�⺻ - �̵�
//�Ϲݰ���
// ������
//�뽬 // �뽬+ ����
//����

//����
//�ǰ� - ���
//�⺻
//Ȱ ����

public class ZeldaMove : MonoBehaviour
{
    public float speed = 5f;
    private float NORMALspeed = 1f;
    private float RUNspeed = 4f;
    private float DASHspeed = 30f;

    private float DASHstack = 0;
    private int ATTACKstack = 0;
    private int CHARGEDstack = 0;
    public int HEAITHstack = 12;
    
    public bool Candash = false;
    public bool isdashing = false;

    public float gravity = -9.81f;

    int MOVE = 1;
    int ATTACK = 2;
    int DASH = 4;
    int CHARGED = 5;
    int HIT = 6;
    int GUARD = 7;
    int RUSH = 8;
    int BOW = 9;

    private float currentTime;
    private float idleTime;



    int state;
    float yvelocity;

    CharacterController cc;
    public GameObject Enemy;
    public Animator Playeranimator;

    void Start()
    {
        state = MOVE;
        speed = NORMALspeed;
        //Enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {

        if (state == MOVE)
        {
            UpdateMove();
        }
        //if (state == DASH)
        //{
        //    UpdateDash();
        //}
        if (state == ATTACK)
        {
            UpdateAttack();
        }
        if (state == CHARGED)
        {
            UpdateChargedattack();
        }
        //if(state == HIT)
        //{
        //    UpdateHit();
        //}
        if (state == GUARD)
        {
            UpdateGuard();
        }
        if (state == BOW)
        {
            UpdateBow();
        }


    }
    //private void UpdateIdle()
    //{

    //    //����Ű -> ���ݻ��� ��ȯ
    //    //�̵�Ű -> �̵����� ��ȯ
    //    //�ǰݽ� -> �ǰݻ��� ��ȯ
    //    //�����ݽ� -> �����ݻ��� ��ȯ

    //    //��� �ִϸ��̼�
    //    //if(Input.)
    //}
    private void UpdateMove()
    {
        //�̵�
        //if(!Input.anyKey)
        //{
        //    //��� �ִϸ��̼�;
        //�ƴϸ� �� ���⼭ Ű �Է� ������ �ִϸ��̼� ����� ���� ���� �ڵ� ������ �Ƿ���..
        //}
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.y = 0;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;

        //yvelocity += gravity * Time.deltaTime;
        //�̵��ִϸ��̼�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack += Time.deltaTime;
            if (DASHstack <= 0.3f)
            {
                speed = DASHspeed;
                print("speed");
                return;
            }
            if (DASHstack >= 0.3f)
            {
                speed = RUNspeed;
                print("n");
            }
            
            Debug.Log(DASHstack);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            DASHstack = 0;
            speed = NORMALspeed;
        }

            //if (DASHstack >= 0.3f)
            //{
            //    speed = RUNspeed;
            //    print("l");
            //}


        //else if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //}
        //����
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ATTACKstack++;
            state = ATTACK;
        }
        //������
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            state = CHARGED;
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            state = BOW;
        }

        ////����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            state = GUARD;
        }

        ////ȸ�Ǵ� (�뽬 Ȥ�� ���� )
        ////���� (���� �� �ߵ��Ǵ� ��)


    }
    //private void UpdateDash()
    //{
    //    if ( Candash == true)
    //    {
    //        // �帣�� �ð��� 1�ʺ��� ������
    //        if (currentTime > 0.5f)
    //        {
    //            isdashing = false;
    //            currentTime = 0;
    //        }
    //        else if (currentTime <= 0.5f)
    //        {
    //            isdashing = true;
    //            currentTime += Time.deltaTime;
                
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        //Playeranimator.SetTrigger("Adash");
    //        //���⿡ ���� ȸ�Ǹ�� �ӵ� ��� �ٸ���.
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        //Playeranimator.SetTrigger("Wdash");
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        //Playeranimator.SetTrigger("Sdash");
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        //Plateranimator.SetTrigger("Ddash");
    //    }

    //    //�޸���Ű 2�� �������� �ִϸ��̼����� 3����? �̵�
    //    //Ƚ������(1, 2��)
    //    //���� ��� ����( ���� �ı�)
    //    //Ű�Է��� ���� ��� IDLE

    //    print("d");
    //    speed = DASHspeed;
    //    //�뽬�ִϸ��̼� 
    //    DASHstack = 0;
    //    speed = RUNspeed;
    //    print("k");
    //    state = MOVE;

    //}
    private void UpdateAttack()
    {
        print("a");
        if (ATTACKstack == 1)
        {
            //�׼� �ִϸ��̼� 1
            print("a1");
            state = MOVE;
        }
        //�ð� ���� �Է½�
        else if (ATTACKstack == 2)
        {
            //�׼� �ִϸ��̼� 2
            print("a2");
            state = MOVE;

        }
        //�ð� ���� �Է½� 
        else if (ATTACKstack == 3)
        {
            //�׼� �ִϸ��̼� 3
            print("a3");
            ATTACKstack = 0;
            print("ATTACKstack");
            state = MOVE;
        }
        //����1 - �ð� ������ �ؼ� �ش� �ð��ȿ� ����Ű�� ��������� ���� ��ȯ (����2) ������ �ð��� ���� move ���� ��ȯ.
        //����2 - ����
        //����3 - ����, ���� �ʱ�ȭ�� ���� ���� �ִϸ��̼� �� ���� 1

        //if (ATTACKstack >= 1)/*&& ȸ�ǽ��� == 1)*/
        //{
        //    state = RUSH;
        //}
    }
    private void UpdateChargedattack()
    {
        //������ �ð� ���� �� ���� ������ ������ ���� ��. 
        //Ű�Է��� ���� ��� IDLE
        //������ ����
        CHARGEDstack++;
        if (CHARGEDstack == 1)
        {
            //�ִϸ��̼� 1 ���� ������
            state = MOVE;
        }
        if (ATTACKstack == 1 && CHARGEDstack >= 1)
        {
            //�ִϸ��̼� 2 ������ ���� �÷�ġ��
            if (ATTACKstack == 1 && CHARGEDstack == 2)
            {
                //�ִϸ��̼� 3 ������� ����
                print("ATTACKstack");
                print("CHARGEDstack");
            }
            else if (ATTACKstack == 1 && CHARGEDstack == 3)
            {
                //�ִϸ��̼� 4 �������
            }
            state = MOVE;

        }
        if (ATTACKstack == 2 && CHARGEDstack == 1)
        {
            //�ִϸ��̼� 5 ����
            state = MOVE;
        }

        //���� �ð���
        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            state = MOVE;
        }
        Time.timeScale = 0.2f;
    }
    //private void UpdateHit()
    //{
    //    //�ǰ�
    //    //�ǰ� �ִϸ��̼� ������ ������ ����, Ȥ�� �޸��� ���� (ª��)
    //    //���� ����,
    //    //Ű�Է��� ���� ��� IDLE

    //    HEAITHstack--;
    //    //�ǰ� �ִϸ��̼�
    //    if (HEAITHstack == 0)
    //    {
    //        //�״� �ִϸ��̼�, ȿ��
    //        Destroy(this.gameObject, 3f);
    //    }



    //}

    private void UpdateGuard()
    {
        //����
        //�ǰ� ���� ��ȯ �Ұ�, idle, run, attack ���� ��ȯ����
        //����Ű ������ � ���µ� �ش� ���·� ��ȯ.

    }
    //private void UpdateRush()
    //{
    //    //����
    //    //�ǰ�x , ������ ���� �ִϸ��̼��� ����Ű�� ���� �����.
    //    //�ð����� ���� , ������ ����� �ڵ�����  move ���µ�.
    //    if (!Input.GetKeyDown(KeyCode.LeftShift))
    //    {
    //        state = MOVE;
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    //state = HIT;
    //    if(other.gameObject.name.Contains("Enemey"))
    //    {
    //        state = HIT;
    //    }
    //    state = MOVE;
    //    //���� ���� ���� ��, hit ���·� ��ȯ�Ǹ� �ǰ� �ִϸ��̼ǰ� �Բ� ��� ���� ����.
    //}

    //Ȱ
    private void UpdateBow()
    {
        //Ȱ ����, ����,
    }
}

        ////����
        ////if(cc.isGrounded && Input.GetButtonDown("Jump"))
        ////{
        ////    //yvelocity = jumpPower;
        ////}