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

    #region
    void StartHit()
    {
        // ���� �ִϸ��̼ǿ� �پ��ִ� traill_tracking �Լ�.
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
        //�ʱ� ��ġ ����
        initialPo = trail.transform.localPosition;
        // traill renderer ������.
        trail.emitting = false;
        canAttack = true;
        instance = this;
    }


    #region �ڷ�ƾ �Ἥ traill
    // �����Լ�.. ���� �Ⱦ�.
    double InterPolate(double input_y)
    {
        double output = 0;

        //������
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

    //traill renderer ��ġ�� �� vector3����
    Vector3[] T_offset = new Vector3[100];
    public IEnumerator Attack()
    {
        canAttack = false; //���� �ߺ� ������ flag
                           //playeranim.SetBool("isAttack", true);

        // ��ġ �ʱ�ȭ.
        int flag = 0;
        while (true)
        {
            if (T_offset[flag].magnitude == 0)
                break;
            T_offset[flag] = Vector3.zero;
            flag++;
        }

        //Ʈ���� ����, ��ġ ����.
        for (int i = 0; i < trail_offsets.Length; i++)
        {
            if (trail_offsets[i].magnitude == 0) break;

            T_offset[i] = trail_offsets[i];
            Debug.Log(T_offset[i]);
        }
        yield return new WaitForSeconds(waitTime);
        trail.transform.position = T_offset[0]; // �ʱ� ��ġ�� �̵�
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
