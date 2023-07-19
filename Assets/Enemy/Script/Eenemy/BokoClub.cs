using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BokoClub : MonoBehaviour
{
    public static BokoClub instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Link")
        {
            print("@@@@@@@@@@@@@@@@@@@");

            // ��ũ�� ������ �Լ��� ȣ���Ѵ�.
            PlayerManager.instance.HP--;

            RagdollBokoblin.instance.club.enabled = false;  
        }
    }
}
