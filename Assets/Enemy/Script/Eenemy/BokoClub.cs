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
        if(other.name == "Link")
        {
            print("@@@@@@@@@@@@@@@@@@@");

            // 링크의 데미지 함수를 호출한다.
            PlayerManager.instance.HP--;

            RagdollBokoblin.instance.club.enabled = false;
        }
    }
}
