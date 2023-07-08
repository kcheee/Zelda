using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    #region 싱글톤
    static public SkillUI instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject Skillpanel;
    public GameObject CrossHair;

    private void Start()
    {
        // 스킬 창 닫기
        Skillpanel.SetActive(false);
    }
    private void Update()
    {
        // 쿨타임이 지나있으면 스킬창 열림.
        if (CoolTimer.instance.cooltime == CoolTimer.CoolTime.None)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                // 스킬창 열려있으면 닫고 닫혀있으면 열림.
                if (!Skillpanel.activeSelf)
                    Skillpanel.SetActive(true);
            }
        }
        #region CrossHair Onoff
        // CrossHair Onoff
        // 폭탄
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bomb)
        {
            if (!CrossHair.activeSelf) CrossHair.SetActive(true);
        }
        else
            CrossHair.SetActive(false);

        // 활 
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bowzoom
            || SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bow)
        {
            if (!CrossHair.activeSelf) CrossHair.SetActive(true);
        }
        else
            CrossHair.SetActive(false);
        #endregion
    }
}