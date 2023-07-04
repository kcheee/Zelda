using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    bool activeInvetory = false;

    public Slot[] slots;    // ����
    public Transform slotHolder;    // ���� ��ġ
    private void Start()
    {
        inven = Inventory.Instance;
        slots=slotHolder.GetComponentsInChildren<Slot>();
        inven.onslotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        // �����Ҷ� �κ��丮 ��
        inventoryPanel.SetActive(activeInvetory);
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;

            // ��ư Ȱ��ȭ ��Ȱ��ȭ
            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            // bool ������ �ݴ밪
            activeInvetory =!activeInvetory;
            inventoryPanel.SetActive(activeInvetory);
        }
    }
    public void AddSlot()
    {
        Debug.Log("����");
        inven.SlotCnt++;
    }

    void RedrawSlotUI()
    {
        for(int i = 0;i<slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i = 0;i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
