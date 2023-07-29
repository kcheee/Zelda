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

            // 링크의 데미지 함수를 호출한다.
            PlayerManager.instance.HP--;
        }
    }
}
