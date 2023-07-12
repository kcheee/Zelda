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
            Debug.Log("보스전 시작.");
            // 게임 매니저의 보스 패턴.
            GameManager.instance.state= GameManager.State.Boss;
            occupationGage.SetActive(true);
        }
    }
}
