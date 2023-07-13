using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "ItemEffect/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPotin = 5;

    void Start()
    {

    }
    public override bool ExecuteRole()
    {
        PlayerManager.instance.potionHeal();
        return true;
    }
}
