using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    int kiilcnt;
    // ������ �� �Լ��� �ҷ���.
    public void KillcntUpdate()
    {
        kiilcnt++;
        KillCnt.text = KillCnt.ToString();
    }
    #endregion


}




