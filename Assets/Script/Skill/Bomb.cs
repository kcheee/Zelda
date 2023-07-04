using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        // �� �ݰ����� ��ġ�� ������.
        Collider[] cols = Physics.OverlapSphere(collision.transform.position, 30);
        
        for (int i = 0; i < cols.Length; i++)
        {       
            // ��ź �ݰ濡 �ִ� ������Ʈ rigidbody ������
            if (cols[i].name.Contains("Capsule"))
            {
                Debug.Log(i + " : " + cols[i]);
                Rigidbody rigid = cols[i].GetComponent<Rigidbody>();
                // (������ ��, ������ ��ġ�� ���� �߽�, ������ ��ġ�� ���� �ݰ�, ���� �ڱ�ġ�� ��)
                rigid.AddExplosionForce(300, this.transform.position, 10, 20);
            }
        }
       


    }
}
