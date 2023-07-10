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

        this.gameObject.GetComponent<Bocoblin1>().state = Bocoblin1.BocoblinState.Damaged;

        rb.AddForce(other.transform.root.GetChild(0).up * 10, ForceMode.Impulse);
        rb.AddForce(other.transform.root.GetChild(0).forward * 5, ForceMode.Impulse);

    }
}
