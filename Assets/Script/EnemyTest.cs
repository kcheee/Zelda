using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 제일 위에있는 오브젝트 접근 후 첫번째 자식 접근 (Link)
        Debug.Log(other.transform.root.GetChild(0).forward);
        
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        if(!Trail.strongatt)
        { 
        rb.AddForce(other.transform.root.GetChild(0).up*rb.mass * 5, ForceMode.Impulse);
        rb.AddForce(other.transform.root.GetChild(0).forward * rb.mass * 1.5f, ForceMode.Impulse);
        }
        else {
            rb.AddForce(other.transform.root.GetChild(0).up * rb.mass * 10, ForceMode.Impulse);
            rb.AddForce(other.transform.root.GetChild(0).forward * rb.mass * 20, ForceMode.Impulse);
        }
    }
}
