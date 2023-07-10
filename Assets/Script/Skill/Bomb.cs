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

        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        // �� �ݰ����� ��ġ�� ������.
        Collider[] cols = Physics.OverlapSphere(collision.transform.position, 30);
        
        for (int i = 0; i < cols.Length; i++)
        {       
            // ��ź �ݰ濡 �ִ� ������Ʈ rigidbody ������
            if (cols[i].name.Contains("Boco"))
            {
                Debug.Log(i + " : " + cols[i]);
                Rigidbody rigid = cols[i].GetComponent<Rigidbody>();
                // (������ ��, ������ ��ġ�� ���� �߽�, ������ ��ġ�� ���� �ݰ�, ���� �ڱ�ġ�� ��)
                rigid.AddExplosionForce(500 * rb.mass, this.transform.position, 20, 30);
                cols[i].GetComponent<Bocoblin1>().state = Bocoblin1.BocoblinState.Damaged;
            }
        }
       


    }
}
