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

    public Slot[] slots;    // 슬롯
    public Transform slotHolder;    // 슬롯 위치
    private void Start()
    {
        inven = Inventory.Instance;
        slots=slotHolder.GetComponentsInChildren<Slot>();
        inven.onslotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        // 시작할때 인벤토리 끔
        inventoryPanel.SetActive(activeInvetory);
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;

            // 버튼 활성화 비활성화
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
            // bool 형태의 반대값
            activeInvetory =!activeInvetory;
            inventoryPanel.SetActive(activeInvetory);
        }
    }
    public void AddSlot()
    {
        Debug.Log("실행");
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
