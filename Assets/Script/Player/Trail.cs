using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    //Ʈ���� ��� ����   
    private bool canAttack;
    public TrailRenderer trail;
    public Vector3[] trail_offsets; //�̵��� ���� Set
    public float waitTime = 0.01f;  //���� �������� �̵��ϱ� �� ���ð�


    private void Awake()
    {
        trail.emitting = false;
        canAttack = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && canAttack)
        {    
            StartCoroutine(Attack());  
        }
    }
    IEnumerator Attack()
    {
        canAttack = false; //���� �ߺ� ������ flag
        //playeranim.SetBool("isAttack", true);
        //Ʈ���� ����
        
        yield return new WaitForSeconds(waitTime);
        trail.transform.localPosition = trail_offsets[0];
        yield return new WaitForSeconds(waitTime);
        trail.emitting = true;
        Debug.Log("tkfgod");
        for (int i = 1; i < trail_offsets.Length; i++)
        {
            yield return new WaitForSeconds(waitTime);
            trail.transform.localPosition = trail_offsets[i];
        }

        yield return new WaitForSeconds(waitTime);

        trail.emitting = false;
        //playeranim.SetBool("isAttack", false);
        canAttack = true;
        yield return null;
    }
}
