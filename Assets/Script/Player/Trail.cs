using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    //트레일 멤버 변수   
    private bool canAttack;
    public TrailRenderer trail;
    public Vector3[] trail_offsets; //이동할 정점 Set
    public float waitTime = 0.01f;  //다음 정점으로 이동하기 전 대기시간


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
        canAttack = false; //공격 중복 방지용 flag
        //playeranim.SetBool("isAttack", true);
        //트레일 구현
        
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
