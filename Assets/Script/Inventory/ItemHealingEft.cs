using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "ItemEffect/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPotin = 5;
    public GameObject healthbar;

    void Start()
    {
        healthbar = GameObject.Find("HealthBar");
    }
    public override bool ExecuteRole()
    {
        Debug.Log(healthbar);
        return true;
    }
}
