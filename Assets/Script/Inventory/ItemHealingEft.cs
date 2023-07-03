using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "ItemEffect/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPotin = 5;
    public override bool ExecuteRole()
    {
        Debug.Log("이게 실행이 되야함");
        return true;
    }
}
