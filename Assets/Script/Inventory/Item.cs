using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ���Ƿ� ��Ÿ
public enum ItemType
{
    Equipment,
    Consumables,
    Etc
}

// inspector���� Ŭ���� �Ǵ� ����ü�� ������ ������� ����.
// Serializable�� ����� inspector�� �����ų �� ����. 
[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;

    // ������ ��� �޼���
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
