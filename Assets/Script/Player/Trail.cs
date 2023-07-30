using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Trail : MonoBehaviour
{
    // 싱글톤
    static public Trail instance;
    
    //트레일 멤버 변수   
    private bool canAttack;
    public TrailRenderer trail;
    public Vector3[] trail_offsets;//이동할 정점 Set
    public float waitTime = 0.01f;  //다음 정점으로 이동하기 전 대기시간
    public int trail_index = 0;
    static public bool traill_track = false;
    public int interpolateSize=2; //점과 점사이에 나누어질 구간 수
    Vector3 initialPo;

    public GameObject swordeft1;
    public BoxCollider sword;

    #region 애니메이션에 붙어있음
    // 강공격 bool 변수
    static public bool strongatt = false;
    void StartHit()
    { 
        // 공격 애니메이션에 붙어있는 traill_tracking 함수.
        //trail.emitting = true;
        traill_track = true;

        //  칼 콜라이더 
        sword.enabled = true;
        swordeft1.SetActive(false);
    }
    void EndHit()
    {
        swordeft1.SetActive(true);
        //trail.emitting = false;
        traill_track = false;
        trail_index = 0;
        sword.enabled = false;
        strongatt=false;
    } 
    void StrongAttack(){ strongatt =true; }

    public GameObject ChargeAtkEft;
    public void ChargeAtkEFT()
    {
        ChargeAtkEft.SetActive(true);
    }
    #endregion


    private void Awake()
    {
        //초기 위치 저장
        initialPo = trail.transform.localPosition;
        // traill renderer 꺼지게.
        trail.emitting = false;
        canAttack = true;
        instance = this;
        sword.enabled = false;
    }



}
