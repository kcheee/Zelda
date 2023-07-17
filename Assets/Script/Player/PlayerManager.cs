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
    // HP Property
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }
    int healthUI = 0;
    
    // 체력 데미지.
    public void PlayerDamaged()
    {
        HP--;
        health[healthUI].SetActive(false);
        healthUI++;
    }
    private void Awake()
    {
        HP = Healthbar.transform.childCount;
        instance = this;

        for (int i = 0; i < maxhp; i++)
        {
            health[i] = Healthbar.transform.GetChild(i).gameObject;
        }
    }
    public void potionHeal()
    {
        Healthbar.transform.GetChild(HP - 1).gameObject.SetActive(true);
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            PlayerDamaged();
        }
        //transform.position= Vector3.zero; 
    }
}