using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraZoom : MonoBehaviour
{
    Vector3 fristPo = new Vector3(0, 1, -10); // �ʱ� ��ġ
    Vector3 ZoomPo = new Vector3(1.5f, 0f, -4); // �� ��ġ

    // Ȱ, ��ź ī�޶�.
    IEnumerator ZoomCamera(Vector3 pos1, Vector3 pos2)
    {
        // zoomPosition���� Update ��ü
        while (Vector3.Distance(transform.localPosition, pos2)>0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos2, 0.2f);
            //Debug.Log(transform.localPosition + " " + pos2);

            yield return new WaitForSecondsRealtime(0.02f);      
        }
    }

    // flag
    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        #region ��ź
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bomb && !flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, ZoomPo));

            flag = true;
        }
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.None && flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, fristPo));

            flag = false;
        }
        #endregion

        #region Ȱ
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bowzoom && !flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, ZoomPo));
            flag = true;
        }
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.None && flag)
        {
            StartCoroutine(ZoomCamera(transform.localPosition, fristPo));
            flag = false;   
        }
        #endregion

    }
}