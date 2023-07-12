using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    public GameObject occupationGage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Link"))
        {
            Debug.Log("������ ����.");
            // ���� �Ŵ����� ���� ����.
            GameManager.instance.state= GameManager.State.Boss;
            occupationGage.SetActive(true);
        }
    }
}
