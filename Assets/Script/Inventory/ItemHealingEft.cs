using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "ItemEffect/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPotin = 5;
    public override bool ExecuteRole()
    {
        Debug.Log("�̰� ������ �Ǿ���");
        return true;
    }
}
