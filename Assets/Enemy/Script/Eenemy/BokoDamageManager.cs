using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BokoDamageManager : MonoBehaviour
{
    Rigidbody[] rbs;

    private void Start()
    {
        // �θ� ���� �ڽĿ� �ִ� rigidbody ������.
        rbs = transform.parent.GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.name);       
        if (other.gameObject.layer == 17)
        {
            // rigidbody
            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);
                // �������� �ƴϸ�
                if (!Trail.strongatt)
                {
                    Debug.Log("����");
                    rb.AddForce(transform.up * 5 + (-transform.forward * 2.5f), ForceMode.Impulse);
                }
                else rb.AddForce(-transform.forward * 10 + transform.up * 10, ForceMode.Impulse);
            }

            GetComponentInParent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;
        }
    }

    private void Update()
    {
        //transform.position = Follow.transform.position;
    }
}