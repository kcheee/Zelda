using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEditor.Rendering;

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
    public TextMeshProUGUI[] ScoreText;
    public CanvasGroup BackGround;
    public GameObject minimap;
    public GameObject PlayerUI;
    float clearTime = 0;

    IEnumerator EndUI()
    {
        state = State.Victory;
        // 3초 후 
        yield return new WaitForSeconds(3f);
        // 시작 UI 켜짐
        Start_EndUI.SetActive(true);

        // 다른 모든 UI 꺼지게
        PlayerUI.SetActive(false);
        minimap.SetActive(false);
        // 보스 게이지 상태는 테스트 할때 안켜둠으로 null 체크
        if (BossGage.activeSelf)
        BossGage.SetActive(false);

        string path = "D:\\zelda\\Assets\\Screenshots\\Resources\\ending_screenshot.png";
        ScreenCapture.CaptureScreenshot(path);

        // 클리어 타임;
        ScoreText[0].text = Mathf.FloorToInt(clearTime).ToString(); // 소수점 버리기
        ScoreText[1].text = (100+ Mathf.FloorToInt(clearTime)).ToString();

        // 킬 카운트
        ScoreText[3].text = ScoreText[2].text;
        ScoreText[4].text = ScoreText[3].text;

        // 합계
        int a = int.Parse(ScoreText[1].text);
        int b = int.Parse(ScoreText[4].text);
        ScoreText[5].text = (300+(a + b)).ToString();

        // victory 켜짐
        yield return new WaitForSeconds(1f);
        VictoryText.SetActive(true);
        yield return new WaitForSeconds(3f);

        var obj = Resources.Load(path);
        Debug.Log(obj);
        // 모리블린 죽이는 장면 캡쳐
        //string path = "D:\\zelda\\Assets\\Screenshots\\Resources\\ending_screenshot.png";
        //ScreenCapture.CaptureScreenshot(path);

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

    #endregion

    #region Defeat state
    public GameObject Defeat_T;
    public GameObject Defeat_B;
    public Camera_PlayerMove cameraPlayerMove_onoff;
    public Volume volume;
    public IEnumerator Playerdie()
    {
        state = State.Die;
        // 죽고 나서 딜레이 주고 UI
        yield return new WaitForSeconds(2);
        // 카메라 플레이어 이동 꺼짐.
        cameraPlayerMove_onoff.enabled = false;
        Start_EndUI.SetActive(true);
        Defeat_T.SetActive(true);
        Defeat_B.SetActive(true);
        StartCoroutine(Fade(Defeat_B.GetComponent<CanvasGroup>(), 0, 1, 1));
        // 피사계 심도
        volume.enabled = true;

        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
    }
    // button
    public void Retry()
    {
        Time.timeScale = 1f;
        // 피사계 심도
        volume.enabled = false;
        // UI false
        Start_EndUI.SetActive(false);
        Defeat_T.SetActive(false);
        Defeat_B.SetActive(false);

        // 카메라 플레이어 이동 킴
        cameraPlayerMove_onoff.enabled = true;

        // 체력 채움
        PlayerManager.instance.PlayerRetry();
    }
    // button
    public void BattleStop()
    {
        // LoadScene(0);
    }

    #endregion

    #region KillCount UI and KillcntUpdate() 함수
    // killUI
    public TextMeshProUGUI KillCnt;
    int kiilcnt = 0;
    public Slider FinishATKGaze;

    // 죽으면 이 함수를 불러옴.
    public void KillcntUpdate()
    {
        kiilcnt++;
        if (FinishATKGaze.value <= FinishATKGaze.maxValue)
            GameManager.instance.FinishATKGaze.value++;
        KillCnt.text = kiilcnt.ToString();
    }
    #endregion

    #region 보스방 입장
    public GameObject BossGage;
    public GameObject Mobline;
    bool EndUI_flag = false;
    bool boss_enable = true;
    public GameObject wave;
    void UpdateBoss()
    {
        // 보코블린 잡을 때 게이지 줄어듦
        // 2. 20퍼 남았을 때 보스 출현
        // 3. 종료 후 UI 
         
        if(BossGage.GetComponent<Slider>().value<=50&& boss_enable)
        {
            boss_enable =false;
            Mobline.SetActive(true);
            wave.SetActive(true);
        }

        // 종료 UI 한번만 실행되게 해야함.
        if(BossGage.GetComponent<Slider>().value<=0&& !EndUI_flag)
        {
            StartCoroutine(EndUI());
            EndUI_flag=true;
        }

    }
    #endregion


    private void Update()
    {
        if(state!=State.Victory)
        clearTime += Time.deltaTime;

        // 킬수 테스트,  보코블린 Destory시에 KillcntUpdate()실행 해줘야 함.
        if (Input.GetKeyDown(KeyCode.M)) {  StartCoroutine(EndUI()); }
        
        // 보스전.
        if(state ==State.Boss)
        {
            UpdateBoss();
        }
    }

}