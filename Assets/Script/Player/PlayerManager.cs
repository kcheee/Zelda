using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;

    public int hp;
    int maxhp=6;
    public GameObject Healthbar;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }
    private void Awake()
    {
        HP = Healthbar.transform.childCount;
        instance = this;
    }
    public void potionHeal()
    {
        Healthbar.transform.GetChild(HP-1).gameObject.SetActive(true);
    }
    public void Update()
    {
        //transform.position= Vector3.zero; 
    }
}