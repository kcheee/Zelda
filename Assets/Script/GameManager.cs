using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region �̱��� �� Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        // ���� UI
        StartCoroutine(Start_EndUIdelay());
    }
    #endregion

    // ����
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

    #region Start UI �� End UI
    // �ʱ� ���� UI
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

    // Ŭ���� Ÿ�� ����, koCnt �־�� ��.
    #region ���� UI
    public CanvasGroup[] endUI;
    public CanvasGroup BackGround;
    
    IEnumerator EndUI()
    {
        // ���� UI ����
        Start_EndUI.SetActive(true);
        // victory ����
        yield return new WaitForSeconds(1f);
        VictoryText.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fade(BackGround, 0f, 1f, 1f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fade(BackGround, 1f, 0.6f, 0.5f));

        Vector3 UIPos = new Vector3(Start_EndUI.transform.position.x, Start_EndUI.transform.position.y + 85, Start_EndUI.transform.position.z);

        float dis = 999; // ������ ��
        // StartUI ���� �ö󰡴� �ڵ�.
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

    // fade �Լ�
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

    #region Defeat state
    public GameObject Defeat_T;
    public GameObject Defeat_B;
    public Camera_PlayerMove cameraPlayerMove_onoff;
    public Volume volume;
    public IEnumerator Playerdie()
    {
        state = State.Die;
        // �װ� ���� ������ �ְ� UI
        yield return new WaitForSeconds(2);
        // ī�޶� �÷��̾� �̵� ����.
        cameraPlayerMove_onoff.enabled = false;
        Start_EndUI.SetActive(true);
        Defeat_T.SetActive(true);
        Defeat_B.SetActive(true);
        StartCoroutine(Fade(Defeat_B.GetComponent<CanvasGroup>(), 0, 1, 1));
        // �ǻ�� �ɵ�
        volume.enabled = true;

        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
    }
    // button
    public void Retry()
    {
        Time.timeScale = 1f;
        // �ǻ�� �ɵ�
        volume.enabled = false;
        // UI false
        Start_EndUI.SetActive(false);
        Defeat_T.SetActive(false);
        Defeat_B.SetActive(false);

        // ī�޶� �÷��̾� �̵� Ŵ
        cameraPlayerMove_onoff.enabled = true;

        // ü�� ä��
        PlayerManager.instance.PlayerRetry();
    }
    // button
    public void BattleStop()
    {
        // LoadScene(0);
    }

    #endregion

    #region KillCount UI and KillcntUpdate() �Լ�
    // killUI
    public TextMeshProUGUI KillCnt;
    int kiilcnt = 0;
    // ������ �� �Լ��� �ҷ���.
    public void KillcntUpdate()
    {
        kiilcnt++;
        KillCnt.text = kiilcnt.ToString();
    }
    #endregion

    private void Update()
    {
        // ų�� �׽�Ʈ,  ���ں� Destory�ÿ� KillcntUpdate()���� ����� ��.
        if (Input.GetKeyDown(KeyCode.M)) { KillcntUpdate(); StartCoroutine(EndUI()); }
        if(Input.GetKeyDown(KeyCode.C)) { StartCoroutine(Playerdie()); }
    }

}