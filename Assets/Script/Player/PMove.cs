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
    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
    }
    private void Move()
    {

        // Input�� vector2�� ����.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();

        // �̵��� �ִ°��� üũ.
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
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
        // �뽬���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(DashAttack());
        }
        // ����
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
        //������
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
    //        //�׼� �ִϸ��̼� 1
    //        print("a1");
    //    }
    //    //�ð� ���� �Է½�
    //    else if (ATTACKstack == 2)
    //    {
    //        //�׼� �ִϸ��̼� 2
    //        print("a2");

    //    }
    //    //�ð� ���� �Է½� 
    //    else if (ATTACKstack == 3)
    //    {
    //        //�׼� �ִϸ��̼� 3
    //        print("a3");
    //        ATTACKstack = 0;
    //        print("ATTACKstack");
    //    }

    //    //����1 - �ð� ������ �ؼ� �ش� �ð��ȿ� ����Ű�� ��������� ���� ��ȯ (����2) ������ �ð��� ���� move ���� ��ȯ.
    //    //����2 - ����
    //    //����3 - ����, ���� �ʱ�ȭ�� ���� ���� �ִϸ��̼� �� ���� 1

    //}
    //private void UpdateChargedattack()
    //{
    //    //������ �ð� ���� �� ���� ������ ������ ���� ��. 
    //    //Ű�Է��� ���� ��� IDLE
    //    //������ ����
    //    if (CHARGEDstack == 1)
    //    {
    //        //�ִϸ��̼� 1 ���� ������
    //        print("1");
    //    }
    //    if (ATTACKstack == 1 && CHARGEDstack >= 1)
    //    {
    //        //�ִϸ��̼� 2 ������ ���� �÷�ġ��
    //        print("2");
    //        if (ATTACKstack == 1 && CHARGEDstack == 2)
    //        {
    //            //�ִϸ��̼� 3 ������� ����
    //            print("3");
    //        }
    //        else if (ATTACKstack == 1 && CHARGEDstack == 3)
    //        {
    //            //�ִϸ��̼� 4 �������
    //            print("4");
    //        }

    //    }
    //    if (ATTACKstack == 2 && CHARGEDstack == 1)
    //    {
    //        //�ִϸ��̼� 5 ����
    //        print("5");
    //    }


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

