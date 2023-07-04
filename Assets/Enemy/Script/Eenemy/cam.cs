using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    float rx;
    float ry;
    public float rotSpeed = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 마우스의 입력값을 받아서
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 2. 회전값을 축적한 다음에
        rx += my * rotSpeed * Time.deltaTime;   // y 축을 중심으로 도리도리
        ry += mx * rotSpeed * Time.deltaTime;   // x 축을 중심으로 끄덕끄덕

        // 3. 위 아래 75만큼만 회전하고 싶다
        rx = Mathf.Clamp(rx, -70, 70);  // Mathf.Clamp(변수, 최소값, 최댓값) : 최소값보다 작아지면 최소값을, 최대값보다 크면 최대값을 반환

        // 4. 회전한다
        transform.eulerAngles = new Vector3(-rx, ry, 0);    // -rx 인 이유 : 
                                                            // transform.rotation
    }
}
