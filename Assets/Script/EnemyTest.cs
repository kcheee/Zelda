using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ���� �����ִ� ������Ʈ ���� �� ù��° �ڽ� ���� (Link)
        //Debug.Log(other.transform.root.GetChild(0).forward);
        print(other.name);

        Rigidbody[] rb = transform.GetComponentsInChildren<Rigidbody>();
        if (!Trail.strongatt)
        {
            for (int i = 1; i < rb.Length; i++)
            {
                rb[i].AddForce(other.transform.up *  5, ForceMode.Impulse);
                //rb[i].AddForce(other.transform.forward  * 1.5f, ForceMode.Impulse);
            }
        }
        else
        {
            for (int i = 1; i < rb.Length; i++)
            {
                rb[i].AddForce(other.transform.up * 10, ForceMode.Impulse);
                //rb[i].AddForce(other.transform.forward * 20, ForceMode.Impulse);
            }
        }
        this.gameObject.GetComponent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;
    }
}