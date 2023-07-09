using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

// 사용자가 마우스 왼쪽 버튼을 누르면
// 화살공장에서 화살을 만들고
// 그 화살을 총구위치에 배치하고싶다.
public class PlayerBow : MonoBehaviour
{

    List<GameObject> arrowObjectPool;
    int arrowObjectPoolCount = 5;
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

        for (int i = 0; i < arrowObjectPoolCount; i++)
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
            StartCoroutine(shotBow());

            currentTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            bAutoFire = false;
        }
    }
    IEnumerator shotBow()
    {
        SkillManager.instance.skill_state = SkillManager.Skill_state.skill_bomb;
        yield return new WaitForSeconds(0.5f);
        MakeArrow();
        yield return new WaitForSeconds(0.5f);
        MakeArrow();
        yield return new WaitForSeconds(0.5f);
        MakeArrow();
        yield return new WaitForSeconds(0.8f);
        MakeArrow();
        SkillManager.instance.skill_state = SkillManager.Skill_state.None;
        // 스킬 쿨타임
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

    }
    void MakeArrow()
    {
        // 화살이 만들어질 때 화살목록에서 비활성화된 화살을 하나 가져와서 활성화 하고싶다.
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {           
            arrow.transform.position = firePosition.position;
            arrow.transform.forward = Camera.main.transform.forward;
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
    }
}
