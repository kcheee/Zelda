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
     
        //rb.AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);


        // �� �ݰ����� ��ġ�� ������.
        Collider[] cols = Physics.OverlapSphere(collision.contacts[0].point, 20);
        for (int i = 0; i < cols.Length; i++)
        {
            // ��ź �ݰ濡 �ִ� ������Ʈ rigidbody ������
            if (cols[i].CompareTag("Bokoblin"))
            {
                Debug.Log(i + " : " + cols[i]);
                Rigidbody rigid = cols[i].GetComponent<Rigidbody>();
                // (������ ��, ������ ��ġ�� ���� �߽�, ������ ��ġ�� ���� �ݰ�, ���� �ڱ�ġ�� ��)

                rigid.AddExplosionForce(10 * rigid.mass, collision.contacts[0].point, 20, 30 * rigid.mass, ForceMode.Impulse);
                
                cols[i].GetComponentInParent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;
            }
        }
       


    }
}
