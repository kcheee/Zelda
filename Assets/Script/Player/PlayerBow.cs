using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static SkillManager;

// 사용자가 마우스 왼쪽 버튼을 누르면
// 화살공장에서 화살을 만들고
// 그 화살을 총구위치에 배치하고싶다.

public class PlayerBow : MonoBehaviour
{
    #region 오브젝트 풀
    // 오브젝트 풀
    List<GameObject> arrowObjectPool;
    int arrowObjectPoolCount = 5;
    public static List<GameObject> deActiveArrowObjectPool;
    //public Transform bulletParent;

    public List<GameObject> DeActiveArrowObjectPool
    {
        get { return deActiveArrowObjectPool; }
    }
    #endregion

    public GameObject arrowFactory;
    public Transform firePosition;  // 초기 포지션
    public CameraShake camerashake; // 카메라 쉐이크

    public GameObject BowReady;

    bool bAutoFire;
    float currentTime;
    public float fireTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // 태어날 때 화살을 미리 만들어서 화살목록에 등록하고 비활성화 해놓고
        arrowObjectPool = new List<GameObject>();
        deActiveArrowObjectPool = new List<GameObject>();

        for (int i = 0; i < arrowObjectPoolCount; i++)
        {
            GameObject arrow = Instantiate(arrowFactory);
            // bullet의 부모 = bulletParent
            //arrow.transform.parent = bulletParent;
            arrow.SetActive(false);
            arrowObjectPool.Add(arrow);
            deActiveArrowObjectPool.Add(arrow);
        }

        // 방패 칼 들고 있기
        BowReady.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bAutoFire)
        {
            // 누르는 중
            // 시간이 흐르다가 
            currentTime += Time.deltaTime;
            // 생성시간이 되면
            if (currentTime > fireTime)
            {
                // 화살을 만들겠다.
                MakeArrow();
                currentTime = 0;
            }
        }
        // 만약 사용자가 x 을 누르면 
        if (Input.GetKeyDown(KeyCode.X))
        {
            bAutoFire = true;        
            currentTime = 0;

            // 칼 방패 뒤로
            // 방패 칼 들고 있기
            BowReady.SetActive(true);
        }
        if (Input.GetKey(KeyCode.X))
        {
            bAutoFire = true;
            // 카메라 앞방향
            transform.forward = Camera.main.transform.forward;
            // 플레이어 위아래 회전 제한.
            Vector3 dir = transform.eulerAngles;
            dir = new Vector3(0, dir.y, 0);
            transform.eulerAngles = dir;
            // 스킬 상태 전환.
            SkillManager.instance.skill_state = SkillManager.Skill_state.skill_bowzoom;
            currentTime = 0;
        }

        else if (Input.GetKeyUp(KeyCode.X))
        {
            bAutoFire = false;
            StartCoroutine(shotBow());            
        }

    }

    //활 쏘기.
    IEnumerator shotBow()
    {
        SkillManager.instance.skill_state = SkillManager.Skill_state.skill_bow;
        yield return new WaitForSeconds(0.2f);
        MakeArrow();
        yield return new WaitForSeconds(0.2f);
        MakeArrow();
        yield return new WaitForSeconds(0.2f);
        MakeArrow();
        yield return new WaitForSeconds(0.4f);
        MakeArrow();
        yield return new WaitForSeconds(0.5f);
        SkillManager.instance.skill_state = SkillManager.Skill_state.None;
        // 스킬 쿨타임
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

        // 칼 방패 없애기
        BowReady.SetActive(false);
    }

    void MakeArrow()
    {
        // 화살이 만들어질 때 화살목록에서 비활성화된 화살을 하나 가져와서 활성화 하고싶다.
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {           
            float rx = UnityEngine.Random.Range(-4.0f, 4.0f); float ry = UnityEngine.Random.Range(-4.0f, 4.0f); float rz = UnityEngine.Random.Range(-4.0f, 4.0f);
            // 카메라 쉐이크.
            camerashake.ShakeCamera();
            arrow.transform.position = firePosition.position;
            // 카메라 방향 앞
            arrow.transform.forward = Camera.main.transform.forward;
            // 랜덤으로 화살 쏘기
            Vector3 dir = arrow.transform.eulerAngles;
            dir = new Vector3(dir.x+rx, dir.y+ry, dir.z+rz);
            arrow.transform.eulerAngles = dir;

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

        // 활성화된 화살이 없는 경우에는 새로운 화살을 생성한다.
        GameObject newArrow = Instantiate(arrowFactory);
        arrowObjectPool.Add(newArrow);
        return newArrow;
    }

}
