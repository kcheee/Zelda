using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;

    public int HP;
    int maxhp=6;
    public GameObject Healthbar;

    private void Awake()
    {
        HP = Healthbar.transform.childCount;
        instance = this;
    }
    public void potionHeal()
    {
        Healthbar.transform.GetChild(HP-1).gameObject.SetActive(true);
    }
}
