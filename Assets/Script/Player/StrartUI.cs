using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 버튼에 마우스 커서를 올리면 화살표가 움직이고싶다.
// 마우스 커서를 치우면 화살표가 비활성화 되면서 보이지 않고 움직이지않는다.

//1. 비활성화 상태.
//2. 마우스를 올리면 활성화.

//4. 마우스를 치우면 비활성화.

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

