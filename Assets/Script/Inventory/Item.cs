using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장비 포션류 기타
public enum ItemType
{
    Equipment,
    Consumables,
    Etc
}

// inspector에는 클래스 또는 구조체의 정보가 노출되지 않음.
// Serializable를 사용해 inspector에 노출시킬 수 있음. 
[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;

    // 아이템 사용 메서드
    public bool Use()
    {
        bool isUsed = false;
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole();
        }
        return isUsed;
    }
}
