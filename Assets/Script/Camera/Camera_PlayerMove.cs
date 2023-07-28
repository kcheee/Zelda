using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Camera_PlayerMove : MonoBehaviour
{
    public Transform characterBody;
    public Transform CameraArm;
    private float StartY = -3f;
    float flag = 0;

     public float speed = 10;
    float NORMALspeed = 10;
    private float DASHstack = 0;
    public float DASHspeed = 30;
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

    #region �뽬 ���� ����� ���� �ڷ�ƾ
    float dash_ti;
    public static bool dash_bool;

    Vector3 dash_dir;   // �뽬 ���� �ޱ�
    //IEnumerator DashAttack(Vector3 dir)
    //{

    //    if (dash_ti > 2)
    //    {
    //        Debug.Log("����");
    //        dash_ti = 0;
    //        yield return null;
    //    }
    //    else
    //    {
    //        Debug.Log("ylfgod");
    //        yield return new WaitForSeconds(0.02f);
    //        yield return StartCoroutine(DashAttack(dir));
    //    }
    //}
    #endregion


    private void Move()
    {
        // Input�� vector2�� ����.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();

        // �̵��� �ִ°��� üũ.
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            // animation_T ��Ȳ��ȯ
            animation_T.instance.animator.SetBool("move", true);

            // ī�޶��� x,z ���� �����ͼ� ����ȭ ���� y���� 0���� ������Ű�� ������ ĳ���Ͱ� ���Ʒ��� �����̴� �� ����
            Vector3 lookForward = new Vector3(CameraArm.forward.x, 0, CameraArm.forward.z).normalized;

            // ���� ���� ������
            Vector3 lookRight = new Vector3(CameraArm.right.x, 0, CameraArm.right.z).normalized;


            // dir = transform.forward+transform.right ���� ���� (������ ����)
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // �뽬 �� �޸���.
            if (Input.GetKey(KeyCode.Z))
            {

                DASHstack += Time.deltaTime;
                if (DASHstack <= 0.3f)
                {
                    animation_T.instance.state = animation_T.ani_state.dash;
                    animation_T.instance.animator.SetBool("dash", true);
                    //this.Playeranimator.SetBool("dash", true); // �뽬 �ִϸ��̼�
                    speed = DASHspeed; //�뽬 ���ǵ�� ��ȯ.

                    /*return*/
                    
                }
                if (DASHstack >= 0.3f)
                {
                    animation_T.instance.animator.SetBool("dash", false);
                    animation_T.instance.state = animation_T.ani_state.run;
                    animation_T.instance.animator.SetBool("run", true);
                    speed = RUNspeed; //�޸��� ���ǵ�

                }
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                animation_T.instance.animator.SetBool("run", false);
                DASHstack = 0;
                speed = NORMALspeed;
                //���� ���ǵ�

            } 

            // ĳ������ �չ����� ī�޶� �չ������� ����.
            characterBody.forward = moveDir;

            // move ���·� �ٲ�
            // attack ���°� �ƴҶ� ������.
            if (animation_T.instance.state != animation_T.ani_state.attack && !dash_bool)
            {
                transform.position += moveDir * Time.deltaTime * speed;
            }

            // �뽬 ���� �ޱ�
            dash_dir = moveDir;
        }
        else 
        {      
            // �÷��̾� �ִϸ����� ���°� move�� �ƴҶ� idle�� �ٲٰ� move = false;
            if (animation_T.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("move"))
            {               
                animation_T.instance.state = animation_T.ani_state.idle;
                animation_T.instance.animator.SetBool("move", false);
            }
        }
        // �뽬���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.V))
        {
            //StartCoroutine(DashAttack(dash_dir));
            animation_T.instance.animator.SetBool("DashAttack", true);          
        }

        if (dash_bool)
        {
            dash_ti += Time.deltaTime;
            if (dash_ti < 2f)
            {
                transform.position += dash_dir * Time.deltaTime * 15;
            }
            else
            {
                dash_bool = false;
                dash_ti = 0;
            }
        }
    }
    public bool DASHBOOL()
    {
       
        return dash_bool = true;

    }
    void LookAround()
    {
        // �ǴϽ������϶� �������� �ʰ� ����.
        if(!FinishAttack_.Finishattack) {
        // ���콺 x,y ��ǥ �� 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // ���� ����.
        if (x < 180)
            x = Mathf.Clamp(x, -10, 70);
        else
            x = Mathf.Clamp(x, 335, 361);

        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }
    }
}
