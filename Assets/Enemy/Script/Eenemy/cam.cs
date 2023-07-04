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
        // 1. ���콺�� �Է°��� �޾Ƽ�
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 2. ȸ������ ������ ������
        rx += my * rotSpeed * Time.deltaTime;   // y ���� �߽����� ��������
        ry += mx * rotSpeed * Time.deltaTime;   // x ���� �߽����� ��������

        // 3. �� �Ʒ� 75��ŭ�� ȸ���ϰ� �ʹ�
        rx = Mathf.Clamp(rx, -70, 70);  // Mathf.Clamp(����, �ּҰ�, �ִ�) : �ּҰ����� �۾����� �ּҰ���, �ִ밪���� ũ�� �ִ밪�� ��ȯ

        // 4. ȸ���Ѵ�
        transform.eulerAngles = new Vector3(-rx, ry, 0);    // -rx �� ���� : 
                                                            // transform.rotation
    }
}
