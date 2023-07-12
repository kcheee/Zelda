using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤 및 Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        StartCoroutine(StartUIdelay());
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

    #region 초기 시작 UI
    // StartUI
    public GameObject StartUI;
    // 초기 시작 UI
    IEnumerator StartUIdelay()
    {
        yield return new WaitForSeconds(1.5f);
        StartUI.SetActive(true);
        yield return new WaitForSeconds(4f);
        StartUI.SetActive(false);
    }
    #endregion

    #region KillCount UI and KillcntUpdate() 함수
    // killUI
    public TextMeshProUGUI KillCnt;
    int kiilcnt;
    // 죽으면 이 함수를 불러옴.
    public void KillcntUpdate()
    {
        kiilcnt++;
        KillCnt.text = KillCnt.ToString();
    }
    #endregion


}




