using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Trail : MonoBehaviour
{
    #region �̱���
    static public Trail instance;
     private void Awake()
    {
        //�ʱ� ��ġ ����
        // traill renderer ������.
        instance = this;
        sword.enabled = false;
    }
    #endregion

    public GameObject[] swordeft1;
    int swordeftIndex;
    public BoxCollider sword;

    #region �ִϸ��̼ǿ� �پ�����
    // ������ bool ����
    static public bool strongatt = false;
    bool flag = false;
    Vector3 swordSize;
    void StartHit()
    { 
        // ���� �ִϸ��̼ǿ� �پ��ִ� traill_tracking �Լ�.
        //trail.emitting = true;
        swordSize  =sword.size;
        //  Į �ݶ��̴� 
        sword.enabled = true;
        
        swordeft1[animation_T.instance.comboCount].SetActive(false);
        if(strongatt)
        sword.size = new Vector3(sword.size.x+3,sword.size.y+3,sword.size.z+3);
    }
    void EndHit()
    {
        sword.size = swordSize;
        Debug.Log(animation_T.instance.comboCount);
        if (!flag)
        swordeft1[animation_T.instance.comboCount].SetActive(true);
        //trail.emitting = false;
        sword.enabled = false;
        strongatt=false;
        flag= false;
    } 
    // ������
    void StrongAttack(){ strongatt =true; }

    public GameObject ChargeAtkEft;
    public void ChargeAtkEFT()
    {
        flag = true;
        ChargeAtkEft.SetActive(true);   
    }
    #endregion


    



}
