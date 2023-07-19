using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolFoot : MonoBehaviour
{
    public static MolFoot instance;

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
