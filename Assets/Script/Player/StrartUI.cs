using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ��ư�� ���콺 Ŀ���� �ø��� ȭ��ǥ�� �����̰�ʹ�.
// ���콺 Ŀ���� ġ��� ȭ��ǥ�� ��Ȱ��ȭ �Ǹ鼭 ������ �ʰ� ���������ʴ´�.

//1. ��Ȱ��ȭ ����.
//2. ���콺�� �ø��� Ȱ��ȭ.

//4. ���콺�� ġ��� ��Ȱ��ȭ.

public class StrartUI : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject ImageUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ImageUI.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ImageUI.SetActive(false);
    }
}

