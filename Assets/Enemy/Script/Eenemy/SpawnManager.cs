using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // 보코블린 공장
    public GameObject bokoFactory;
    // 모리블린 공장
    public GameObject molFactory;
    // 스폰 위치
    Transform[] spawnSpot;

    // Start is called before the first frame update
    void Start()
    {
        spawnSpot = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.BossGage.GetComponent<Slider>().value == 100)
        {
            for (int i = 0; i < spawnSpot.Length; i++)
            {
                if (spawnSpot[0])
                {
                    continue;
                }
                else
                {
                    GameObject bokoblin = Instantiate(bokoFactory);
                    bokoblin.transform.position = spawnSpot[i].position;
                }
            }
        }

            
    }
}
