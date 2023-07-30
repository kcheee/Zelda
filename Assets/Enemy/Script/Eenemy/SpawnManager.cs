using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // ���ں� ����
    public GameObject bokoFactory;
    // �𸮺� ����
    public GameObject molFactory;
    // ���� ��ġ
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
