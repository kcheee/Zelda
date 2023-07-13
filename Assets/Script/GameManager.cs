using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region �̱��� �� Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        StartCoroutine(StartUIdelay());
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

    #region �ʱ� ���� UI
    // StartUI
    public GameObject StartUI;
    // �ʱ� ���� UI
    IEnumerator StartUIdelay()
    {
        yield return new WaitForSeconds(1.5f);
        StartUI.SetActive(true);
        yield return new WaitForSeconds(4f);
        StartUI.SetActive(false);
    }
    #endregion

    #region KillCount UI and KillcntUpdate() �Լ�
    // killUI
    public TextMeshProUGUI KillCnt;
    int kiilcnt=0;
    // ������ �� �Լ��� �ҷ���.
    public void KillcntUpdate()
    {
        kiilcnt++;
        KillCnt.text = kiilcnt.ToString();
    }
    #endregion
    private void Update()
    {
        // ų�� �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.M)) { KillcntUpdate(); }
    }

}




