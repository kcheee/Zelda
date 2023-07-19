using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BokoDamageManager : MonoBehaviour
{
    Rigidbody[] rbs;

    private void Start()
    {
        // 부모에 가서 자식에 있는 rigidbody 가져옴.
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
                // 강공격이 아니면
                if (!Trail.strongatt)
                {
                    Debug.Log("실행");
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