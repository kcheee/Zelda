using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ¹× Awake.
    static public GameManager instance;
    private void Awake()
    {
        instance = this;
        StartCoroutine(StartUIdelay());
    }
    #endregion
    
    // StartUI
    public GameObject StartUI;
    IEnumerator StartUIdelay()
    {
        yield return new WaitForSeconds(1.5f);
        StartUI.SetActive(true);
        yield return new WaitForSeconds(4f);
        StartUI.SetActive(false);
    }

}




