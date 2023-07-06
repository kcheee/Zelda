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
    public float speed = 5f;
    private float NORMALspeed = 1f;
    private float RUNspeed = 4f;
    private float DASHspeed = 15f;
    private float rotationspeed = 10f;

    private float DASHstack = 0;
    private int ATTACKstack = 0;
    private int CHARGEDstack = 0;
    public int HEAITHstack = 12;

    private float IDLEstack = 0f;

    public bool Candash = false;
    public bool isdashing = false;

    public float gravity = -9.81f;

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



    int state;
    float yvelocity;

    CharacterController cc;
    public GameObject Enemy;
    public Animator Playeranimator;

    void Start()
    {
        state = MOVE;
        speed = NORMALspeed;
        IDLEstack = 0.3f;
        //Enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
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
        //}
        if (state == GUARD)
        {
            UpdateGuard();
        }

    }
    //private void UpdateIdle()
    //{

    //    //아니면 걍 여기서 키 입력 없을시 애니메이션 출력이 좀더 쉽고 코드 단축이 되려나..
    //    this.Playeranimator.SetTrigger("Idle");

    //    //if(Input.anykey)
    //    //공격키 -> 공격상태 전환
    //    //이동키 -> 이동상태 전환
    //    //피격시 -> 피격상태 전환
    //    //강공격시 -> 강공격상태 전환

    //    //대기 애니메이션
    //    //if(Input.)
    //}
    #region move
    private void UpdateMove()
    {
        //키 누르는게 없으면 idle 상태로 전환.
        if (!Input.anyKey) //누르는게 없을시.
        {
            IDLEstack += Time.deltaTime; //누르는 시간 계산
            if (IDLEstack >= 0.3f)
            {
                this.Playeranimator.SetBool("Idle",true);
            }
            else
            {
                IDLEstack = 0;
                this.Playeranimator.SetBool("Idle", false);
            }
            Debug.Log(IDLEstack);
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.y = 0;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        //transform.Rotate(transform.forward * h * rotationspeed * Time.deltaTime);
        this.Playeranimator.SetTrigger("move"); // 움직임

        //yvelocity += gravity * Time.deltaTime;
        //이동애니메이션
        if (Input.GetKey(KeyCode.LeftShift))
        {
            DASHstack += Time.deltaTime;
            if (DASHstack <= 0.3f)
            {
                this.Playeranimator.SetTrigger("dash"); // 대쉬 애니메이션
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
            this.Playeranimator.SetTrigger("move"); //이동 애니메이션
        }
        //공격
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ATTACKstack++;
            state = ATTACK;
        }
        //강공격
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            state = CHARGED;
        }
        //가드
        if (Input.GetKeyDown(KeyCode.Z))
        {
            state = GUARD;
        }

        ////회피는 (대쉬 혹은 점프 )
        ////러쉬 (공격 후 발동되는 거)


    }
    #endregion 
    //private void UpdateDash()
    //{
    //    if ( Candash == true)
    //    {
    //        // 흐르는 시간이 1초보다 켜지면
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
    //        //방향에 따른 회피모션 속도 평와 다르게.
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

    //    //달리기키 2번 눌렸을때 애니메이션으로 3미터? 이동
    //    //횟수제한(1, 2번)
    //    //공격 기능 있음( 몬스터 파괴)
    //    //키입력이 없을 경우 IDLE

    //    print("d");
    //    speed = DASHspeed;
    //    //대쉬애니메이션 
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

        //if (ATTACKstack >= 1)/*&& 회피스택 == 1)*/
        //{
        //    state = RUSH;
        //}
    }
    private void UpdateChargedattack()
    {
        //강공격 시간 지연 후 범위 강공격 데미지 제일 쎔. 
        //키입력이 없을 경우 IDLE
        //강공격 스택
        CHARGEDstack++;
        if (CHARGEDstack == 1)
        {
            //애니메이션 1 넓은 강공격
            state = MOVE;
        }
        if (ATTACKstack == 1 && CHARGEDstack >= 1)
        {
            //애니메이션 2 강공격 위로 올려치기
            if (ATTACKstack == 1 && CHARGEDstack == 2)
            {
                //애니메이션 3 허공에서 공격
                print("ATTACKstack");
                print("CHARGEDstack");
            }
            else if (ATTACKstack == 1 && CHARGEDstack == 3)
            {
                //애니메이션 4 내려찍기
            }
            state = MOVE;

        }
        if (ATTACKstack == 2 && CHARGEDstack == 1)
        {
            //애니메이션 5 돌진
            state = MOVE;
        }

        //일정 시간후
        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            state = MOVE;
        }
        Time.timeScale = 0.2f;
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

    private void UpdateGuard()
    {
        //가드
        //피격 상태 전환 불가, idle, run, attack 상태 전환가능
        //가드키 누를시 어떤 상태든 해당 상태로 변환.
        //if (Input.GetKeyDown(KeyCode.Z))
        //{

        //}

    }
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

