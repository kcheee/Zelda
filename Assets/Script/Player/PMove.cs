using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour
{
    public Transform characterBody;
    public Transform CameraArm;
    private float StartY = -3f;
    float flag = 0;

    float speed = 5;
    float NORMALspeed = 5;
    private float DASHstack = 0;
    private float DASHspeed = 20f;
    private float RUNspeed = 10f;

    private int ATTACKstack = 0;
    private int CHARGEDstack = 0;

    public float time = 0f;
    public float delayTime = 3f;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        StartY = transform.position.y;
    }

    IEnumerator Attack()
    {
        if (ATTACKstack == 1)
        {
            print("@");
            yield return new WaitForSeconds(10f);

        }
        else if (ATTACKstack == 2)
        {
            print("@2");
            yield return new WaitForSeconds(10f);
        }
        else if (ATTACKstack == 3)
        {
            print("@3");
            yield return new WaitForSeconds(10f);
        }

    }
    IEnumerator Charged()
    {
        if (CHARGEDstack == 1)
        {
            print("#");
            yield return new WaitForSeconds(0.2f);
        }
        if (CHARGEDstack == 1 && ATTACKstack == 1)
        {
            print("#@");
            yield return new WaitForSeconds(0.2f);
        }

        else if (CHARGEDstack == 2)
        {
            print("#2");
            yield return new WaitForSeconds(0.2f);
        }
        else if (CHARGEDstack == 3)
        {
            print("#3");
            yield return new WaitForSeconds(0.2f);
            CHARGEDstack = 0;
        }
    }

    #region 대쉬 어택 기술을 위한 코루틴
    float ti;
    static public bool dashattack = false;
    IEnumerator DashAttack()
    {
        while (ti < 2)  // 2초동안 실행
        {
            speed = 12;
            dashattack = true;
            //Debug.Log("실행");
            ti += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        ti = 0;
        speed = 5;
        dashattack = false;
        yield return null;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
    }
    private void Move()
    {

        // Input를 vector2로 받음.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();

        // 이동이 있는것을 체크.
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            // 카메라의 x,z 값만 가져와서 정규화 해줌 y값은 0으로 고정시키는 이유는 캐릭터가 위아래로 움직이는 것 방지
            Vector3 lookForward = new Vector3(CameraArm.forward.x, 0, CameraArm.forward.z).normalized;

            // 위와 같은 느낌쓰
            Vector3 lookRight = new Vector3(CameraArm.right.x, 0, CameraArm.right.z).normalized;

            // dir = transform.forward+transform.right 같은 느낌 (방향을 정함)
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log(speed);
                DASHstack += Time.deltaTime;
                if (DASHstack <= 0.3f)
                {

                    //this.Playeranimator.SetBool("dash", true); // 대쉬 애니메이션
                    speed = DASHspeed; //대쉬 스피드로 변환.

                    /*return*/
                    ;
                }
                if (DASHstack >= 0.3f)
                {
                    speed = RUNspeed; //달리기 스피드
                    print("n");
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                DASHstack = 0;
                speed = NORMALspeed; //정상 스피드
            }
            // 캐릭터의 앞방향을 카메라 앞방향으로 설정.
            characterBody.forward = moveDir;

            // 이 오브젝트를 움직임   

            transform.position += moveDir * Time.deltaTime * speed;
            //transform.position = new Vector3(transform.position.x, characterBody.transform.localPosition.y, transform.position.z);
        }
        // 대쉬어택 테스트
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(DashAttack());
        }
        // 공격
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            print("2");
            if (ATTACKstack == 3)
            {
                ATTACKstack = 0;
            }
            ATTACKstack++;
            StartCoroutine(Attack());
        }
        //강공격
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            print("3");
            if (CHARGEDstack == 3)
            {
                CHARGEDstack = 0;
            }
            CHARGEDstack++;
            StartCoroutine(Charged());

        }

    }
    //void UpdateAttack()
    //{
    //    print(ATTACKstack);
    //    time += Time.deltaTime;
    //    if (ATTACKstack == 1)
    //    {
    //        //액션 애니메이션 1
    //        print("a1");
    //    }
    //    //시간 내에 입력시
    //    else if (ATTACKstack == 2)
    //    {
    //        //액션 애니메이션 2
    //        print("a2");

    //    }
    //    //시간 내에 입력시 
    //    else if (ATTACKstack == 3)
    //    {
    //        //액션 애니메이션 3
    //        print("a3");
    //        ATTACKstack = 0;
    //        print("ATTACKstack");
    //    }

    //    //공격1 - 시간 설정을 해서 해당 시간안에 공격키를 눌렀을경우 상태 변환 (공격2) 누르는 시간에 따라 move 상태 변환.
    //    //공격2 - 동일
    //    //공격3 - 동일, 스택 초기화로 다음 공격 애니메이션 시 공격 1

    //}
    //private void UpdateChargedattack()
    //{
    //    //강공격 시간 지연 후 범위 강공격 데미지 제일 쎔. 
    //    //키입력이 없을 경우 IDLE
    //    //강공격 스택
    //    if (CHARGEDstack == 1)
    //    {
    //        //애니메이션 1 넓은 강공격
    //        print("1");
    //    }
    //    if (ATTACKstack == 1 && CHARGEDstack >= 1)
    //    {
    //        //애니메이션 2 강공격 위로 올려치기
    //        print("2");
    //        if (ATTACKstack == 1 && CHARGEDstack == 2)
    //        {
    //            //애니메이션 3 허공에서 공격
    //            print("3");
    //        }
    //        else if (ATTACKstack == 1 && CHARGEDstack == 3)
    //        {
    //            //애니메이션 4 내려찍기
    //            print("4");
    //        }

    //    }
    //    if (ATTACKstack == 2 && CHARGEDstack == 1)
    //    {
    //        //애니메이션 5 돌진
    //        print("5");
    //    }


    void LookAround()
    {
        // 마우스 x,y 좌표 값 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // 각도 제한.
        if (x < 180)
            x = Mathf.Clamp(x, -1, 70);
        else
            x = Mathf.Clamp(x, 335, 361);


        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //Vector3 dir = transform.position * h + transform.forward * v;
        //dir.Normalize();
        //if (dir != Vector3.zero)
        //{
        //    if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
        //    {
        //        transform.Rotate(0, 1, 0);
        //    }
        //    transform.forward = Vector3.Lerp(transform.forward, dir, rotationspeed * Time.deltaTime);
        //}
        //rigidbody.MovePosition(this.transform.position + dir * speed * Time.deltaTime);

//}

