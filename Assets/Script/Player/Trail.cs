using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Trail : MonoBehaviour
{
    #region 싱글톤
    static public Trail instance;
     private void Awake()
    {
        //초기 위치 저장
        // traill renderer 꺼지게.
        instance = this;
        sword.enabled = false;
    }
    #endregion

    public GameObject[] swordeft1;
    int swordeftIndex;
    public BoxCollider sword;

    #region 애니메이션에 붙어있음
    // 강공격 bool 변수
    static public bool strongatt = false;
    bool flag = false;
    Vector3 swordSize;
    void StartHit()
    { 
        // 공격 애니메이션에 붙어있는 traill_tracking 함수.
        //trail.emitting = true;
        swordSize  =sword.size;
        //  칼 콜라이더 
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
    // 강공격
    void StrongAttack(){ strongatt =true; }

    public GameObject ChargeAtkEft;
    public void ChargeAtkEFT()
    {
        flag = true;
        ChargeAtkEft.SetActive(true);   
    }
    #endregion


    



}
