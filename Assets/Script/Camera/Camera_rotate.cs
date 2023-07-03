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

            // 캐릭터의 앞방향을 카메라 앞방향으로 설정.
            characterBody.forward = moveDir;

            // 이 오브젝트를 움직임   

            transform.position += moveDir * Time.deltaTime * 5;
            //transform.position = new Vector3(transform.position.x, characterBody.transform.localPosition.y, transform.position.z);
        }
    }

    void LookAround()
    {
        // 마우스 x,y 좌표 값 
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
