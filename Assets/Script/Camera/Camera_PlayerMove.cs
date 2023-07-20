using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Camera_PlayerMove : MonoBehaviour
{
    public Transform characterBody;
    public Transform CameraArm;
    private float StartY = -3f;
    float flag = 0;

     public float speed = 10;
    float NORMALspeed = 10;
    private float DASHstack = 0;
    private float DASHspeed = 30;
    private float RUNspeed = 20;

    private int ATTACKstack = 0;
    private int CHARGEDstack = 0;

    public float time = 0f;
    public float delayTime = 3f;

    private void Start()
    {
        StartY = transform.position.y;
    }
    private void Update()
    {
        //transform.position= new Vector3(characterBody.position.x,0,characterBody.position.z);
        LookAround();
        Move();

    }
    //static public bool at = false;

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

    private void Move()
    {

        // Input를 vector2로 받음.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();

        // 이동이 있는것을 체크.
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            // animation_T 상황전환
            animation_T.instance.animator.SetBool("move", true);

            // 카메라의 x,z 값만 가져와서 정규화 해줌 y값은 0으로 고정시키는 이유는 캐릭터가 위아래로 움직이는 것 방지
            Vector3 lookForward = new Vector3(CameraArm.forward.x, 0, CameraArm.forward.z).normalized;

            // 위와 같은 느낌쓰
            Vector3 lookRight = new Vector3(CameraArm.right.x, 0, CameraArm.right.z).normalized;


            // dir = transform.forward+transform.right 같은 느낌 (방향을 정함)
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 대쉬 및 달리기.
            if (Input.GetKey(KeyCode.Z))
            {
               
                DASHstack += Time.deltaTime;
                if (DASHstack <= 0.3f)
                {
                    animation_T.instance.state = animation_T.ani_state.dash;
                    animation_T.instance.animator.SetBool("dash", true);
                    //this.Playeranimator.SetBool("dash", true); // 대쉬 애니메이션
                    speed = DASHspeed; //대쉬 스피드로 변환.

                    /*return*/
                    ;
                }
                if (DASHstack >= 0.3f)
                {
                    animation_T.instance.animator.SetBool("dash", false);
                    animation_T.instance.state = animation_T.ani_state.run;
                    animation_T.instance.animator.SetBool("run", true);
                    speed = RUNspeed; //달리기 스피드

                }
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                animation_T.instance.animator.SetBool("run", false);
                DASHstack = 0;
                speed = NORMALspeed; //정상 스피드
            }

            //// 캐릭터의 앞방향을 카메라 앞방향으로 설정.

            // 회전 값
            /*
            Quaternion look = Quaternion.LookRotation(lookdir);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * 5);
            */

            
            characterBody.forward = moveDir;
            // move 상태로 바꿈
            if (animation_T.instance.state != animation_T.ani_state.attack)
            {
                transform.position += moveDir * Time.deltaTime * speed;
            }
            
            //transform.position = new Vector3(transform.position.x, characterBody.transform.localPosition.y, transform.position.z);
        }
        else 
        {      
            // 플레이어 애니메이터 상태가 move가 아닐때 idle로 바꾸고 move = false;
            if (animation_T.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("move"))
            {
               
                animation_T.instance.state = animation_T.ani_state.idle;
                animation_T.instance.animator.SetBool("move", false);
            }
        }
        // 대쉬어택 테스트
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(DashAttack());
        }
    }

    void LookAround()
    {
        // 마우스 x,y 좌표 값 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // 각도 제한.
        if (x < 180)
            x = Mathf.Clamp(x, -10, 70);
        else
            x = Mathf.Clamp(x, 335, 361);


        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
