using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ڰ� ���콺 ���� ��ư�� ������
// �Ѿ˰��忡�� �Ѿ��� �����
// �� �Ѿ��� �ѱ���ġ�� ��ġ�ϰ�ʹ�.
public class PlayerBow : MonoBehaviour
{
    List<GameObject> arrowObjectPool;
    int bulletObjectPoolCount = 5;
    public static List<GameObject> deActiveArrowObjectPool;
    //public Transform bulletParent;

    public List<GameObject> DeActiveArrowObjectPool
    {
        get { return deActiveArrowObjectPool; }
    }

    public GameObject arrowFactory;
    public Transform firePosition;

    bool bAutoFire;
    float currentTime;
    public float fireTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // �¾ �� ȭ���� �̸� ���� ȭ���Ͽ� ����ϰ� ��Ȱ��ȭ �س���
        arrowObjectPool = new List<GameObject>();
        deActiveArrowObjectPool = new List<GameObject>();

        for (int i = 0; i < bulletObjectPoolCount; i++)
        {
            GameObject arrow = Instantiate(arrowFactory);
            // bullet�� �θ� = bulletParent
            //arrow.transform.parent = bulletParent;
            arrow.SetActive(false);
            arrowObjectPool.Add(arrow);
            deActiveArrowObjectPool.Add(arrow);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bAutoFire)
        {
            // ������ ��
            // �ð��� �帣�ٰ� 
            currentTime += Time.deltaTime;
            // �Ѿ˻����ð��� �Ǹ�
            if (currentTime > fireTime)
            {
                // ȭ���� ����ڴ�.
                MakeArrow();
                currentTime = 0;
            }
        }
        // ���� ����ڰ� ���콺 ���� ��ư�� ������ 
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
        // ȭ���� ������� �� ȭ���Ͽ��� ��Ȱ��ȭ�� ȭ���� �ϳ� �����ͼ� Ȱ��ȭ �ϰ�ʹ�.
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {
            // �ѱ���ġ�� ��ġ�ϰ�ʹ�.
            arrow.transform.position = firePosition.position;
            arrow.transform.up = firePosition.up;
        }
    }
    GameObject GetArrowFromObjectPool()
    {
        // ���� ��Ȱ����Ͽ� ũ�Ⱑ 0���� ũ�ٸ�
        if (DeActiveArrowObjectPool.Count > 0)
        {
            // ��Ȱ������� 0��° �׸��� ��ȯ�ϰ�ʹ�.
            GameObject arrow = DeActiveArrowObjectPool[0];
            arrow.SetActive(true);
            // ��Ͽ��� bullet�� �����ʹ�.
            DeActiveArrowObjectPool.Remove(arrow);
            return arrow;
        }
        // �׷��� �ʴٸ� null�� ��ȯ�ϰ�ʹ�.
        return null;


        //enum BImpactName
        //{
        //    Floor,
        //    Enemy
        //}

        //// ȭ�����
        //public GameObject arrowFactory;

        //public Transform arroweffect;
        //public GameObject[] ImpactFactorys;


        //void Start()
        //{
        //}

        //void Update()
        //{
        //    if (Input.GetKey(KeyCode.X))
        //    {
        //        UpdateArrow();
        //    }

        //}
        //private void UpdateArrow()
        //{
        //    //
        //    GameObject Arrow = Instantiate(arrowFactory);

        //}

        ////private void UpdateArrow()
        ////{

        ////    if (Input.GetKeyDown(KeyCode.X))
        ////    {
        ////        // 1. ī�޶󿡼� ī�޶��� �չ������� �ü��� �����
        ////        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        ////        int layer = (1 << LayerMask.NameToLayer("Default"));
        ////        RaycastHit hitInfo;
        ////        // 2. �ٶ󺸰�ʹ�.
        ////        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        ////        {
        ////            // 3. �ü��� ���� ���� ȭ����忡�� ȭ���� �����ʹ�. 
        ////            GameObject Arrow = Instantiate(arrowFactory);
        ////            Arrow.transform.position = hitInfo.point;
        ////            // ������ ȸ���ϰ�ʹ�. Ƣ�¹���(forward�� �ε��� ���� Normal��������
        ////            Arrow.transform.forward = hitInfo.normal;
        ////        }
        ////        else
        ////        {
        ////            // ���
        ////        }
        ////    }
        ////}



    }
}
