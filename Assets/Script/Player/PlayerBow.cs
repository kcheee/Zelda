using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ڰ� ���콺 ���� ��ư�� ������
// ȭ����忡�� ȭ���� �����
// �� ȭ���� ��ġ�� ��ġ�ϰ�ʹ�.

public class PlayerBow : MonoBehaviour
{
    List<GameObject> arrowObjectPool;
    int arrowObjectPoolCount = 5;
    public static List<GameObject> deActiveArrowObjectPool;

    public GameObject arrowFactory;
    public Transform[] firePositions;
    private int currentPositionIndex = 0;

    bool bAutoFire;
    float currentTime;
    public float fireTime = 0.2f;

    void Start()
    {
        arrowObjectPool = new List<GameObject>();
        deActiveArrowObjectPool = new List<GameObject>();

        for (int i = 0; i < arrowObjectPoolCount; i++)
        {
            GameObject arrow = Instantiate(arrowFactory);
            arrow.SetActive(false);
            arrowObjectPool.Add(arrow);
            deActiveArrowObjectPool.Add(arrow);
        }
    }

    void Update()
    {
        if (bAutoFire)
        {
            currentTime += Time.deltaTime;
            if (currentTime > fireTime)
            {
                MakeArrow();
                currentTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            bAutoFire = true;
            MakeArrow();
            currentTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            bAutoFire = false;
        }
    }

    void MakeArrow()
    {
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {
            Transform currentPosition = firePositions[currentPositionIndex];
            arrow.transform.position = currentPosition.position;
            arrow.transform.forward = currentPosition.forward;

            currentPositionIndex = (currentPositionIndex + 1) % firePositions.Length;
        }
    }

    GameObject GetArrowFromObjectPool()
    {
        if (deActiveArrowObjectPool.Count > 0)
        {
            GameObject arrow = deActiveArrowObjectPool[0];
            arrow.SetActive(true);
            deActiveArrowObjectPool.Remove(arrow);
            return arrow;
        }
        return null;
    }
}
