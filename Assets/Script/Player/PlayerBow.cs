using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자가 마우스 왼쪽 버튼을 누르면
// 총알공장에서 총알을 만들고
// 그 총알을 총구위치에 배치하고싶다.
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
        // 태어날 때 화살을 미리 만들어서 화살목록에 등록하고 비활성화 해놓고
        arrowObjectPool = new List<GameObject>();
        deActiveArrowObjectPool = new List<GameObject>();

        for (int i = 0; i < bulletObjectPoolCount; i++)
        {
            GameObject arrow = Instantiate(arrowFactory);
            // bullet의 부모 = bulletParent
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
            // 누르는 중
            // 시간이 흐르다가 
            currentTime += Time.deltaTime;
            // 총알생성시간이 되면
            if (currentTime > fireTime)
            {
                // 화살을 만들겠다.
                MakeArrow();
                currentTime = 0;
            }
        }
        // 만약 사용자가 마우스 왼쪽 버튼을 누르면 
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
        // 화살이 만들어질 때 화살목록에서 비활성화된 화살을 하나 가져와서 활성화 하고싶다.
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {
            // 총구위치에 배치하고싶다.
            arrow.transform.position = firePosition.position;
            arrow.transform.up = firePosition.up;
        }
    }
    GameObject GetArrowFromObjectPool()
    {
        // 만약 비활성목록에 크기가 0보다 크다면
        if (DeActiveArrowObjectPool.Count > 0)
        {
            // 비활성목록의 0번째 항목을 반환하고싶다.
            GameObject arrow = DeActiveArrowObjectPool[0];
            arrow.SetActive(true);
            // 목록에서 bullet를 지우고싶다.
            DeActiveArrowObjectPool.Remove(arrow);
            return arrow;
        }
        // 그렇지 않다면 null을 반환하고싶다.
        return null;


        //enum BImpactName
        //{
        //    Floor,
        //    Enemy
        //}

        //// 화살공장
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
        ////        // 1. 카메라에서 카메라의 앞방향으로 시선을 만들고
        ////        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        ////        int layer = (1 << LayerMask.NameToLayer("Default"));
        ////        RaycastHit hitInfo;
        ////        // 2. 바라보고싶다.
        ////        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        ////        {
        ////            // 3. 시선이 닿은 곳에 화살공장에서 화살을 만들고싶다. 
        ////            GameObject Arrow = Instantiate(arrowFactory);
        ////            Arrow.transform.position = hitInfo.point;
        ////            // 방향을 회전하고싶다. 튀는방향(forward을 부딪힌 면의 Normal방향으로
        ////            Arrow.transform.forward = hitInfo.normal;
        ////        }
        ////        else
        ////        {
        ////            // 허공
        ////        }
        ////    }
        ////}



    }
}
