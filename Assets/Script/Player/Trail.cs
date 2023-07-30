using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Trail : MonoBehaviour
{
    // �̱���
    static public Trail instance;
    
    //Ʈ���� ��� ����   
    private bool canAttack;
    public TrailRenderer trail;
    public Vector3[] trail_offsets;//�̵��� ���� Set
    public float waitTime = 0.01f;  //���� �������� �̵��ϱ� �� ���ð�
    public int trail_index = 0;
    static public bool traill_track = false;
    public int interpolateSize=2; //���� �����̿� �������� ���� ��
    Vector3 initialPo;

    public GameObject swordeft1;
    public BoxCollider sword;

    #region �ִϸ��̼ǿ� �پ�����
    // ������ bool ����
    static public bool strongatt = false;
    void StartHit()
    { 
        // ���� �ִϸ��̼ǿ� �پ��ִ� traill_tracking �Լ�.
        //trail.emitting = true;
        traill_track = true;

        //  Į �ݶ��̴� 
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
        //�ʱ� ��ġ ����
        initialPo = trail.transform.localPosition;
        // traill renderer ������.
        trail.emitting = false;
        canAttack = true;
        instance = this;
        sword.enabled = false;
    }



}
