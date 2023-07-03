using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraZoom : MonoBehaviour
{
    Vector3 fristPo = new Vector3(0, 1, -8); // �ʱ� ��ġ
    Vector3 ZoomPo = new Vector3(2, 1, -3);


    IEnumerator ZoomCamera(Vector3 pos1, Vector3 pos2)
    {
        // zoomPosition���� Update ��ü
        while (transform.localPosition != pos2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos2, 0.2f);
            //Debug.Log(transform.localPosition + " " + pos2);

            yield return new WaitForSeconds(0.02f);
        }
    }


    // flag
    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bomb && !flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, ZoomPo));
            Debug.Log("�ѹ�������");
            flag = true;
        }
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.None && flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, fristPo));
            Debug.Log("�ѹ�������1");
            flag = false;
        }

    }
}