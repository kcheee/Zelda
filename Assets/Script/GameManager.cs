using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region 싱글톤 및 Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        StartCoroutine(Start_EndUIdelay());
    }
    #endregion

    // 패턴
    public enum State
    {
        None,
        Start,
        Die,
        Boss,
        Victory
    }
    public State state;

    #region Start UI 및 End UI
    // 초기 시작 UI
    public GameObject Start_EndUI;
    public GameObject VictoryText;
    IEnumerator Start_EndUIdelay()
    {
        yield return new WaitForSeconds(1.5f);
        Start_EndUI.SetActive(true);
        if (state == State.Victory)
        {
            yield return new WaitForSeconds(1f);
            VictoryText.SetActive(true);
        }
        yield return new WaitForSeconds(4f);
        Start_EndUI.SetActive(false);
    }
    #endregion

    #region 종료 UI

    IEnumerator EndUI()
    {
        Start_EndUI.SetActive(true);
        Vector3 UIPos = new Vector3(Start_EndUI.transform.position.x, Start_EndUI.transform.position.y + 85, Start_EndUI.transform.position.z);
        // while 문 돌려야 함.

        float dis = 999; Vector3.Distance(Start_EndUI.transform.position, UIPos);
        // zoomPosition까지 Update 대체
        while (dis > 3)
        {
            Start_EndUI.transform.position = Vector3.Lerp(Start_EndUI.transform.position, UIPos, 0.2f);
            Debug.Log(dis);
            dis=Vector3.Distance(Start_EndUI.transform.position, UIPos);
            yield return new WaitForSeconds(0.02f);
        }
        //while (true) 
        //{
        //    Vector3.Lerp(Start_EndUI.transform.position, UIPos, 0.5f);
        //    if (Vector3.Distance(Start_EndUI.transform.position, UIPos) < 5)
        //    {
        //        break;
        //    }
        //}
        //yield return new Null();
        Debug.Log("tlfgod");
    }

    #endregion


    #region KillCount UI and KillcntUpdate() 함수
    // killUI
    public TextMeshProUGUI KillCnt;
    int kiilcnt = 0;
    // 죽으면 이 함수를 불러옴.
    public void KillcntUpdate()
    {
        kiilcnt++;
        KillCnt.text = kiilcnt.ToString();
    }
    #endregion
    private void Update()
    {
        // 킬수 테스트,  보코블린 Destory시에 KillcntUpdate()실행 해줘야 함.
        if (Input.GetKeyDown(KeyCode.M)) { KillcntUpdate(); StartCoroutine(EndUI()); }
    }

}