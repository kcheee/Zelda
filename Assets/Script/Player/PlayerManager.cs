using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;

    public int hp;
    int maxhp = 6;
    public GameObject Healthbar;
    GameObject[] health = new GameObject[6];
    public BoxCollider playercoll;

    // HP Property
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }

    // 체력 UI 표시
    int healthUI = 0;
    
    // 체력 데미지.
    public void PlayerDamaged()
    {
        HP--;
        health[healthUI].SetActive(false);
        healthUI++;

        // 피가 0이되는 상황
        if (HP == 0)
        {
            //playercoll.enabled = false;
            StartCoroutine(GameManager.instance.Playerdie());
            animation_T.instance.animator.SetTrigger("Die");
            animation_T.instance.animator.SetBool("die_C",true);
        }
    }

    // 플레이어 체력 채움
    public void PlayerRetry()
    {
        //playercoll.enabled = true;
        HP = maxhp;
        healthUI=0;
        for (int i = 0; i < maxhp; i++) health[i].SetActive(true);
        animation_T.instance.animator.SetTrigger("retry");
        animation_T.instance.animator.SetBool("die_C", false);

    }

    private void Awake()
    {
        HP = Healthbar.transform.childCount;

        instance = this;

        // healthbar UI 오브젝트 가져옴
        for (int i = 0; i < maxhp; i++)
        {
            health[i] = Healthbar.transform.GetChild(i).gameObject;
        }
    }
    // 포션 힐
    public void potionHeal()
    {
        Debug.Log("tdsfg");
        Healthbar.transform.GetChild(HP - 1).gameObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            potionHeal();
        }

        // 데미지 테스트
        //if(Input.GetKeyDown(KeyCode.N))
        //{
        //    PlayerDamaged();
        //}
        //transform.position= Vector3.zero; 
    }
}