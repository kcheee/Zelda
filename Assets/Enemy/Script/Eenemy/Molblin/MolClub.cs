using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolClub : MonoBehaviour
{
    public static MolClub instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Link")
        {
            print("@@@@@@@@@@@@@@@@@@@");

            // ��ũ�� ������ �Լ��� ȣ���Ѵ�.
            PlayerManager.instance.HP--;
        }
    }
}
