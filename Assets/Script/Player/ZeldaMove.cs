using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본 - 이동 (wasd
//일반공격 (left control
// 강공격 (left ALT
//대쉬 달리기 (left shift
//활 상태 ( x
//가드 ( z
//러쉬 
//피격 - 사망

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
        this.Playeranimator.SetBool("dash", false); // 대쉬 애니메이션
        this.Playeranimator.SetBool("move", false); // 움직임 트리거 설정

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
        //강공격
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            CHARGEDstack++;
            state = CHARGED;
        }
        //활
        if (Input.GetKeyDown(KeyCode.X))
        {
            state = BOW;
        }
        //가드
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
        #region 이동
        if (Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack += Time.deltaTime;
            if (DASHstack <= 0.3f)
            {

                this.Playeranimator.SetBool("dash", true); // 대쉬 애니메이션
                speed = DASHspeed; //대쉬 스피드로 변환.

                return;
            }
            if (DASHstack >= 0.3f)
            {
                speed = RUNspeed; //달리기 스피드
                print("n");
            }

            Debug.Log(DASHstack);
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack = 0;
            speed = NORMALspeed; //정상 스피드
            this.Playeranimator.SetBool("dash", false); // 대쉬 애니메이션
            this.Playeranimator.SetBool("move", true); //이동 애니메이션
        }
        #endregion
        //공격

        ////회피는 (대쉬 혹은 점프 )
        ////러쉬 (공격 후 발동되는 거)


    }
    #endregion 
   
    private void UpdateAttack()
    {
        print("a");
        if (ATTACKstack == 1)
        {
            //액션 애니메이션 1
            print("a1");
            state = MOVE;
        }
        //시간 내에 입력시
        else if (ATTACKstack == 2)
        {
            //액션 애니메이션 2
            print("a2");
            state = MOVE;

        }
        //시간 내에 입력시 
        else if (ATTACKstack == 3)
        {
            //액션 애니메이션 3
            print("a3");
            ATTACKstack = 0;
            print("ATTACKstack");
            state = MOVE;
        }
        //공격1 - 시간 설정을 해서 해당 시간안에 공격키를 눌렀을경우 상태 변환 (공격2) 누르는 시간에 따라 move 상태 변환.
        //공격2 - 동일
        //공격3 - 동일, 스택 초기화로 다음 공격 애니메이션 시 공격 1

    }
    private void UpdateChargedattack()
    {
        //강공격 시간 지연 후 범위 강공격 데미지 제일 쎔. 
        //키입력이 없을 경우 IDLE
        //강공격 스택
        //Time.deltaTime
        ////if (CHARGEDstack == 1)
        //{
        //    //애니메이션 1 넓은 강공격
        //    print("1");
        //}
        if (ATTACKstack == 1 && CHARGEDstack >= 1)
        {
            //애니메이션 2 강공격 위로 올려치기
            print("2");
            if (ATTACKstack == 1 && CHARGEDstack == 2)
            {
                //애니메이션 3 허공에서 공격
                print("3");
            }
            else if (ATTACKstack == 1 && CHARGEDstack == 3)
            {
                //애니메이션 4 내려찍기
                print("4");
            }

        }
        if (ATTACKstack == 2 && CHARGEDstack == 1)
        {
            //애니메이션 5 돌진
            print("5");
        }

        //일정 시간후
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            state = MOVE;
        }
        Time.timeScale = 0.2f;
    }
    //void Update()
    //{
    //    timer += Time.deltaTime; // 경과 시간을 누적

    //    if (timer >= delayTime)
    //    {
    //        // 지연 시간이 경과하면 원하는 동작 수행
    //        Debug.Log("Delayed action performed!");

    //        // 원하는 동작을 수행한 후에는 타이머를 초기화
    //        timer = 0f;
    //    }
    //}
    private void UpdateBow()
    {
        //장전 -> 대기 -> 발사 애니메이션.
        print("bow");
        //일정 시간 후
        state = MOVE;
    }

    //private void UpdateHit()
    //{
    //    //피격
    //    //피격 애니메이션 끝내고 나서야 공격, 혹은 달리기 가능 (짧음)
    //    //스택 누적,
    //    //키입력이 없을 경우 IDLE
    //    HEAITHstack--;
    //    state = MOVE; 
    //    //피격 애니메이션
    //    if (HEAITHstack == 0)
    //    {
    //        //죽는 애니메이션, 효과
    //        Destroy(this.gameObject, 3f);
    //}



    //}

    //private void UpdateGuard()
    //{
    //    //가드
    //    //피격 상태 전환 불가, idle, run, attack 상태 전환가능
    //    //가드키 누를시 어떤 상태든 해당 상태로 변환.
    //    //if (Input.GetKeyDown(KeyCode.Z))
    //    //{

    //    //}

    //}
    //private void UpdateRush()
    //{
    //    //러쉬
    //    //피격x , 무차별 공격 애니메이션이 공격키를 통해 재생됨.
    //    //시간제한 있음 , 공격이 끊기면 자동으로  move 상태됨.
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
    //    //몸에 공격 맞을 시, hit 상태로 전환되며 피격 애니메이션과 함께 목숨 스택 차감.
    //}

}

