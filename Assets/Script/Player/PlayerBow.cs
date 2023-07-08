using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

// ����ڰ� ���콺 ���� ��ư�� ������
// ȭ����忡�� ȭ���� �����
// �� ȭ���� �ѱ���ġ�� ��ġ�ϰ�ʹ�.

public class PlayerBow : MonoBehaviour
{
    #region ������Ʈ Ǯ
    // ������Ʈ Ǯ
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

        for (int i = 0; i < arrowObjectPoolCount; i++)
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
            // �����ð��� �Ǹ�
            if (currentTime > fireTime)
            {
                // ȭ���� ����ڴ�.
                MakeArrow();
                currentTime = 0;
            }
        }
        // ���� ����ڰ� x �� ������ 
        if (Input.GetKeyDown(KeyCode.X))
        {
            bAutoFire = true;
            
            currentTime = 0;
        }
        if (Input.GetKey(KeyCode.X))
        {
            bAutoFire = true;
            transform.forward = Camera.main.transform.forward;
            SkillManager.instance.skill_state = SkillManager.Skill_state.skill_bowzoom;
            currentTime = 0;
        }

        else if (Input.GetKeyUp(KeyCode.X))
        {
            bAutoFire = false;
            StartCoroutine(shotBow());
        }
    }

    //Ȱ ���.
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
        // ��ų ��Ÿ��
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

    }

    void MakeArrow()
    {
        // ȭ���� ������� �� ȭ���Ͽ��� ��Ȱ��ȭ�� ȭ���� �ϳ� �����ͼ� Ȱ��ȭ �ϰ�ʹ�.
        GameObject arrow = GetArrowFromObjectPool();
        if (arrow != null)
        {           

            float rx = UnityEngine.Random.Range(-2.0f, 2.0f); float ry = UnityEngine.Random.Range(-2.0f, 2.0f); float rz = UnityEngine.Random.Range(-2.0f, 2.0f);
            arrow.transform.position = firePosition.position;
            // ī�޶� ���� ��
            arrow.transform.forward = Camera.main.transform.forward;
            // �������� ȭ�� ���
            Vector3 dir = arrow.transform.eulerAngles;
            dir = new Vector3(dir.x+rx, dir.y+ry, dir.z+rz);
            arrow.transform.eulerAngles = dir;

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

        // Ȱ��ȭ�� ȭ���� ���� ��쿡�� ���ο� ȭ���� �����Ѵ�.
        GameObject newArrow = Instantiate(arrowFactory);
        arrowObjectPool.Add(newArrow);
        return newArrow;
    }
}
