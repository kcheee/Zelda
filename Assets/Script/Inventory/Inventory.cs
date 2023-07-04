using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // region ���� ���ϰ� ����
    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    // delegate �븮�� ����
    public delegate void OnslotCountChange(int val);

    // delegate �븮�� �ν��Ͻ�ȭ
    public OnslotCountChange onslotCountChange;

    // delegate
    public delegate void OnCgangeItem();
    public OnCgangeItem onChangeItem;

    // ������ ����Ʈȭ
    public List<Item> items = new List<Item>();

    private int slotCnt;

    // slotCnt ������Ƽ �κ��丮�� ĭ��
    public int SlotCnt
    {
        // get => slotCnt�� Get{ return slotCnt;}�� ���� �ǹ�
        get => slotCnt; set
        {
            slotCnt = value;
            onslotCountChange.Invoke(slotCnt);  // �޼��� ����
        }
    }
    private void Start()
    {
        slotCnt = 4;    //������ 4ĭ
    }

    public bool Additem(Item _item)
    {
        if (items.Count < slotCnt)  // �κ��丮�� ĭ���� �������� Ŀ�� ����
        {
            items.Add(_item);   // ������ �߰�
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }
        
    public void RemoveItem(int _index)
    {
        Debug.Log("����");
        items.RemoveAt(_index);  // �ε����� �ִ� ������ ����
        onChangeItem.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Potion"))
        {
            // �ε��� ������Ʈ�� ������ ������
            FieldItems fieldItems = collision.collider.GetComponent<FieldItems>();

            // �κ��丮�� ������ ������ �������� �ε��� ������Ʈ�� ����.
            if (Additem(fieldItems.GetItem()))
                fieldItems.DestoryItem();
        }
    }

}
