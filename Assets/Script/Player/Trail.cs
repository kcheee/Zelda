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

    #region
    void StartHit()
    {
        // 공격 애니메이션에 붙어있는 traill_tracking 함수.
        trail.emitting = true;
        traill_track = true;   
    }
    void EndHit()
    {
        trail.emitting = false;
        traill_track = false;
        trail_index = 0;
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
    }


    #region 코루틴 써서 traill
    // 보간함수.. 지금 안씀.
    double InterPolate(double input_y)
    {
        double output = 0;

        //보간식
        foreach (Vector3 i in trail_offsets)
        {
            double frontValue = 1;
            double std_y = i.y;
            double std_x = i.x;
            double std_z = i.z;
            List<double> y_list = new List<double>();

            foreach (Vector3 j in trail_offsets)
            {
                if (j != i)
                {
                    y_list.Add(j.y);
                }
            }

            foreach (double j in y_list)
            {
                frontValue = frontValue * (input_y - j);
                frontValue = frontValue / (std_y - j);
            }
            frontValue = frontValue * std_x;
            output += frontValue;
        }
        return output;
    }

    //traill renderer 위치에 쓸 vector3변수
    Vector3[] T_offset = new Vector3[100];
    public IEnumerator Attack()
    {
        canAttack = false; //공격 중복 방지용 flag
                           //playeranim.SetBool("isAttack", true);

        // 위치 초기화.
        int flag = 0;
        while (true)
        {
            if (T_offset[flag].magnitude == 0)
                break;
            T_offset[flag] = Vector3.zero;
            flag++;
        }

        //트레일 구현, 위치 대입.
        for (int i = 0; i < trail_offsets.Length; i++)
        {
            if (trail_offsets[i].magnitude == 0) break;

            T_offset[i] = trail_offsets[i];
            Debug.Log(T_offset[i]);
        }
        yield return new WaitForSeconds(waitTime);
        trail.transform.position = T_offset[0]; // 초기 위치로 이동
        yield return new WaitForSeconds(waitTime);
        trail.emitting = true;
        for (int i = 1; i < T_offset.Length-5; i++)
        {     
            yield return new WaitForSeconds(waitTime);
            trail.transform.position = T_offset[i];
        }
            trail.emitting = false;
        yield return new WaitForSeconds(waitTime);
     
        //playeranim.SetBool("isAttack", false);
        canAttack = true;

        yield return null;
    }
    #endregion

}
