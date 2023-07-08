using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    #region �̱���
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
        // ��ų â �ݱ�
        Skillpanel.SetActive(false);
    }
    private void Update()
    {
        // ��Ÿ���� ���������� ��ųâ ����.
        if (CoolTimer.instance.cooltime == CoolTimer.CoolTime.None)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                // ��ųâ ���������� �ݰ� ���������� ����.
                if (!Skillpanel.activeSelf)
                    Skillpanel.SetActive(true);
            }
        }
        #region CrossHair Onoff
        // CrossHair Onoff
        // ��ź
        if (SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bomb)
        {
            if (!CrossHair.activeSelf) CrossHair.SetActive(true);
        }
        else
            CrossHair.SetActive(false);

        // Ȱ 
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