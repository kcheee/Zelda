using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.Timeline;

public class FinishAttack_ : MonoBehaviour
{
    public Animator animator;
    public Camera MainCam;
    public Camera SecondCam;
    public GameObject Player_R;    // ���� 
    static public bool Finishattack = false;
    public Volume volume;
    // Start is called before the first frame update

    private void OnEnable()
    {
        animator.applyRootMotion = true;
        MainCam.enabled = false;
        SecondCam.enabled = true;
        Finishattack = true;

        
    }

    private void Update()
    {
        animation_T.instance.state = animation_T.ani_state.FinishAttack;
        // ��ο����� ȿ��
        if (volume.weight < 1)
            volume.weight += Mathf.Lerp(0, 1, Time.deltaTime * 0.4f);
    }

    private void OnDisable()
    {
        // ���ں� ��� ���߰�

        animator.applyRootMotion = false;
        //MainCam.transform.position = SecondCam.transform.position;
        MainCam.enabled = true;
        SecondCam.enabled = false;
        Finishattack = false;
        volume.weight = 0;
        // ����
        // FinishAttack ������Ʈ false
        if (transform.parent.gameObject.activeSelf)
        {
            transform.parent.gameObject.SetActive(false);
            Debug.Log("dlrpdho?");
        }

    }
}
