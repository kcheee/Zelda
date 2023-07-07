using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_PlayerMove : MonoBehaviour
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
    float ti;
    static public bool dashattack = false;
    IEnumerator DashAttack()
    {
        while (ti < 2)  // 2�ʵ��� ����
        {
            speed = 12;
            dashattack = true;
            //Debug.Log("����");
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

        // Input�� vector2�� ����.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();

        // �̵��� �ִ°��� üũ.
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            animation_T.instance.state = animation_T.ani_state.move;
            // ī�޶��� x,z ���� �����ͼ� ����ȭ ���� y���� 0���� ������Ű�� ������ ĳ���Ͱ� ���Ʒ��� �����̴� �� ����
            Vector3 lookForward = new Vector3(CameraArm.forward.x, 0, CameraArm.forward.z).normalized;

            // ���� ���� ������
            Vector3 lookRight = new Vector3(CameraArm.right.x, 0, CameraArm.right.z).normalized;

            // dir = transform.forward+transform.right ���� ���� (������ ����)
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log(speed);
                DASHstack += Time.deltaTime;
                if (DASHstack <= 0.3f)
                {

                    //this.Playeranimator.SetBool("dash", true); // �뽬 �ִϸ��̼�
                    speed = DASHspeed; //�뽬 ���ǵ�� ��ȯ.

                    /*return*/
                    ;
                }
                if (DASHstack >= 0.3f)
                {
                    speed = RUNspeed; //�޸��� ���ǵ�
                    print("n");
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                DASHstack = 0;
                speed = NORMALspeed; //���� ���ǵ�
            }
            // ĳ������ �չ����� ī�޶� �չ������� ����.
            characterBody.forward = moveDir;

            // �� ������Ʈ�� ������   

            transform.position += moveDir * Time.deltaTime * speed;
            //transform.position = new Vector3(transform.position.x, characterBody.transform.localPosition.y, transform.position.z);
        }
        else
            animation_T.instance.state = animation_T.ani_state.idle;

        // �뽬���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(DashAttack());
        }


    }
    
    void LookAround()
    {
        // ���콺 x,y ��ǥ �� 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // ���� ����.
        if (x < 180)
            x = Mathf.Clamp(x, -1, 70);
        else
            x = Mathf.Clamp(x, 335, 361);


        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
