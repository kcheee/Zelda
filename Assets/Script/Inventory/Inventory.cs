using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // region 보기 편하게 설정
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

    // delegate 대리자 정의
    public delegate void OnslotCountChange(int val);

    // delegate 대리자 인스턴스화
    public OnslotCountChange onslotCountChange;

    // delegate
    public delegate void OnCgangeItem();
    public OnCgangeItem onChangeItem;

    // 아이템 리스트화
    public List<Item> items = new List<Item>();

    private int slotCnt;

    // slotCnt 프로퍼티 인벤토리의 칸수
    public int SlotCnt
    {
        // get => slotCnt와 Get{ return slotCnt;}는 같은 의미
        get => slotCnt; set
        {
            slotCnt = value;
            onslotCountChange.Invoke(slotCnt);  // 메서드 실행
        }
    }
    private void Start()
    {
        slotCnt = 4;    //슬롯은 4칸
    }

    public bool Additem(Item _item)
    {
        if (items.Count < slotCnt)  // 인벤토리의 칸수가 아이템의 커야 실행
        {
            items.Add(_item);   // 아이템 추가
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }
        
    public void RemoveItem(int _index)
    {
        Debug.Log("실행");
        items.RemoveAt(_index);  // 인덱스에 있는 아이템 제거
        onChangeItem.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Potion"))
        {
            // 부딪힌 오브젝트의 정보를 가져옴
            FieldItems fieldItems = collision.collider.GetComponent<FieldItems>();

            // 인벤토리에 아이템 정보를 가져오고 부딪힌 오브젝트를 지움.
            if (Additem(fieldItems.GetItem()))
                fieldItems.DestoryItem();
        }
    }

}
