using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region �̱��� �� Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
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

    #region ���� UI

    IEnumerator EndUI()
    {
        Start_EndUI.SetActive(true);
        Vector3 UIPos = new Vector3(Start_EndUI.transform.position.x, Start_EndUI.transform.position.y + 85, Start_EndUI.transform.position.z);
        // while �� ������ ��.

        float dis = 999; Vector3.Distance(Start_EndUI.transform.position, UIPos);
        // zoomPosition���� Update ��ü
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
    }

}