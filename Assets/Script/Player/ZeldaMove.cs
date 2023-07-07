using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�⺻ - �̵� (wasd
//�Ϲݰ��� (left control
// ������ (left ALT
//�뽬 �޸��� (left shift
//Ȱ ���� ( x
//���� ( z
//���� 
//�ǰ� - ���

public class ZeldaMove : MonoBehaviour
{
    public float speed;
    private float NORMALspeed = 5f;
    private float RUNspeed = 10f;
    private float DASHspeed = 20f;
    private float rotationspeed = 10f;

    private float DASHstack = 0;
    private int ATTACKstack = 0;
    private int CHARGEDstack = 0;
    public int HEAITHstack = 12;

    private float IDLEstack = 0f;

    public bool Candash = false;
    public bool isdashing = false;

    public float gravity = -9.81f;

    public enum ZeldaState
    {
        IDLE,
        MOVE,
        ATTACK,
        CHARGED,
    }
    public ZeldaState zeldastate;

    int IDLE = 0;
    int MOVE = 1;
    int ATTACK = 2;
    int CHARGED = 5;
    int HIT = 6;
    int GUARD = 7;
    int RUSH = 8;
    int BOW = 9;

    private float currentTime;
    private float idleTime;

    public float time = 0f;
    public float delayTime = 3f;


    int state;
    float yvelocity;

    CharacterController cc;
    public GameObject Enemy;
    public Animator Playeranimator;
    private Rigidbody rigidbody;
    //public GameObject ss;
    //Camera cam;

    private Vector3 dir = Vector3.zero;

    void Start()
    {
        zeldastate = ZeldaState.IDLE;
        this.Playeranimator.SetBool("dash", false); // �뽬 �ִϸ��̼�
        this.Playeranimator.SetBool("move", false); // ������ Ʈ���� ����

        rigidbody = this.GetComponent<Rigidbody>();
        state = MOVE;
        speed = NORMALspeed;
        IDLEstack = 0.3f;
        //Enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        dir.Normalize();
        if (state == IDLE)
        {
            UpdateIdle();
        }
        if (state == MOVE)
        {
            UpdateMove();
        }
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
        ////}
        //if (state == GUARD)
        //{
        //    UpdateGuard();
        //}
        if (state == BOW)
        {
            UpdateBow();
        }

    }

    void UpdateIdle()
    {

        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
        {
            zeldastate = ZeldaState.MOVE;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ATTACKstack++;
            state = ATTACK;
        }
        //������
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            CHARGEDstack++;
            state = CHARGED;
        }
        //Ȱ
        if (Input.GetKeyDown(KeyCode.X))
        {
            state = BOW;
        }
        //����
        if (Input.GetKey(KeyCode.Z))
        {
            state = GUARD;
        }

    }
    #region move
    void UpdateMove()
    {
        if (dir != Vector3.zero)
        {
            if(Mathf.Sign(transform.forward.x)!= Mathf.Sign(dir.x)|| MathF.Sign(transform.forward.z) != MathF.Sign(dir.z))
            {
                transform.Rotate(0,1,0);
            }
            transform.forward = Vector3.Lerp(transform.forward, dir, rotationspeed * Time.deltaTime);
        }

        rigidbody.MovePosition(this.transform.position + dir * speed * Time.deltaTime);
        #region �̵�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack += Time.deltaTime;
            if (DASHstack <= 0.3f)
            {

                this.Playeranimator.SetBool("dash", true); // �뽬 �ִϸ��̼�
                speed = DASHspeed; //�뽬 ���ǵ�� ��ȯ.

                return;
            }
            if (DASHstack >= 0.3f)
            {
                speed = RUNspeed; //�޸��� ���ǵ�
                print("n");
            }

            Debug.Log(DASHstack);
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack = 0;
            speed = NORMALspeed; //���� ���ǵ�
            this.Playeranimator.SetBool("dash", false); // �뽬 �ִϸ��̼�
            this.Playeranimator.SetBool("move", true); //�̵� �ִϸ��̼�
        }
        #endregion
        //����

        ////ȸ�Ǵ� (�뽬 Ȥ�� ���� )
        ////���� (���� �� �ߵ��Ǵ� ��)


    }
    #endregion 
   
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

    }
    private void UpdateChargedattack()
    {
        //������ �ð� ���� �� ���� ������ ������ ���� ��. 
        //Ű�Է��� ���� ��� IDLE
        //������ ����
        //Time.deltaTime
        ////if (CHARGEDstack == 1)
        //{
        //    //�ִϸ��̼� 1 ���� ������
        //    print("1");
        //}
        if (ATTACKstack == 1 && CHARGEDstack >= 1)
        {
            //�ִϸ��̼� 2 ������ ���� �÷�ġ��
            print("2");
            if (ATTACKstack == 1 && CHARGEDstack == 2)
            {
                //�ִϸ��̼� 3 ������� ����
                print("3");
            }
            else if (ATTACKstack == 1 && CHARGEDstack == 3)
            {
                //�ִϸ��̼� 4 �������
                print("4");
            }

        }
        if (ATTACKstack == 2 && CHARGEDstack == 1)
        {
            //�ִϸ��̼� 5 ����
            print("5");
        }

        //���� �ð���
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            state = MOVE;
        }
        Time.timeScale = 0.2f;
    }
    //void Update()
    //{
    //    timer += Time.deltaTime; // ��� �ð��� ����

    //    if (timer >= delayTime)
    //    {
    //        // ���� �ð��� ����ϸ� ���ϴ� ���� ����
    //        Debug.Log("Delayed action performed!");

    //        // ���ϴ� ������ ������ �Ŀ��� Ÿ�̸Ӹ� �ʱ�ȭ
    //        timer = 0f;
    //    }
    //}
    private void UpdateBow()
    {
        //���� -> ��� -> �߻� �ִϸ��̼�.
        print("bow");
        //���� �ð� ��
        state = MOVE;
    }

    //private void UpdateHit()
    //{
    //    //�ǰ�
    //    //�ǰ� �ִϸ��̼� ������ ������ ����, Ȥ�� �޸��� ���� (ª��)
    //    //���� ����,
    //    //Ű�Է��� ���� ��� IDLE
    //    HEAITHstack--;
    //    state = MOVE; 
    //    //�ǰ� �ִϸ��̼�
    //    if (HEAITHstack == 0)
    //    {
    //        //�״� �ִϸ��̼�, ȿ��
    //        Destroy(this.gameObject, 3f);
    //}



    //}

    //private void UpdateGuard()
    //{
    //    //����
    //    //�ǰ� ���� ��ȯ �Ұ�, idle, run, attack ���� ��ȯ����
    //    //����Ű ������ � ���µ� �ش� ���·� ��ȯ.
    //    //if (Input.GetKeyDown(KeyCode.Z))
    //    //{

    //    //}

    //}
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

}

