using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region 싱글톤 및 Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        // 시작 UI
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
    static Sequence sequenceFadeInOut;

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

    // 클리어 타임 변수, koCnt 넣어야 함.
    #region 종료 UI
    public CanvasGroup[] endUI;
    public CanvasGroup BackGround;
    
    IEnumerator EndUI()
    {
        // 시작 UI 켜짐
        Start_EndUI.SetActive(true);
        // victory 켜짐
        yield return new WaitForSeconds(1f);
        VictoryText.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fade(BackGround, 0f, 1f, 1f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fade(BackGround, 1f, 0.6f, 0.5f));

        Vector3 UIPos = new Vector3(Start_EndUI.transform.position.x, Start_EndUI.transform.position.y + 85, Start_EndUI.transform.position.z);

        float dis = 999; // 임의의 수
        // StartUI 위로 올라가는 코드.
        while (dis > 3)
        {
            Start_EndUI.transform.position = Vector3.Lerp(Start_EndUI.transform.position, UIPos, 0.2f);
            //Debug.Log(dis);
            dis=Vector3.Distance(Start_EndUI.transform.position, UIPos);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(Fade(endUI[i], 0f, 1f, 0.05f));
            yield return new WaitForSeconds(0.1f);
        }
       
    }
    // fade 함수
    private IEnumerator Fade(CanvasGroup group, float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            group.alpha = alpha;
            currentTime += Time.deltaTime;
            yield return null;
        }
        group.alpha = targetAlpha;
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