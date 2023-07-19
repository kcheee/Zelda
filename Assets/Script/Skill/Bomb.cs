using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Bomb : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody[] rbs;
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
                Rigidbody[] rigid = cols[i].GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigid)
                {
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = new Vector3(0, 0, 0);
                    rb.AddExplosionForce(15 * rb.mass, collision.contacts[0].point, 20, 15 * rb.mass, ForceMode.Impulse);
                }

                // ��ź ������
              RagdollBokoblin.Damage = 4;
              cols[i].GetComponentInParent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;
             
            }
        }
       


    }
}
