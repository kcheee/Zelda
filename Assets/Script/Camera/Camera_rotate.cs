using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_rotate : MonoBehaviour
{
    public Transform characterBody;
    public Transform CameraArm;
    private float StartY = -3f;
    float flag = 0;

    private void Start()
    {
        StartY = transform.position.y;
    }
    private void Update()
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

            // ĳ������ �չ����� ī�޶� �չ������� ����.
            characterBody.forward = moveDir;

            // �� ������Ʈ�� ������   

            transform.position += moveDir * Time.deltaTime * 5;
            //transform.position = new Vector3(transform.position.x, characterBody.transform.localPosition.y, transform.position.z);
        }
    }

    void LookAround()
    {
        // ���콺 x,y ��ǥ �� 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180)
            x = Mathf.Clamp(x, -1, 70);
        else
            x = Mathf.Clamp(x, 335, 361);


        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
