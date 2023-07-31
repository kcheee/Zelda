using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psoundscript : MonoBehaviour
{
    private void Update()
    {
        if (icetrigger == true)
        {
            iceDestroy();
            icetrigger = false;
        }
    }
    public static Psoundscript instance;

    [SerializeField] AudioClip[] voicesounds = default;
    [SerializeField] AudioClip[] SFXsounds = default;
    [SerializeField] AudioClip[] Walkingsounds = default;
    #region 음성효과음
    void chargedstart()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[0]);
    }
    void charging()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[1]);
    }
    void dashsoundstart()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[2]);
    }
    //void dashsoundstop()
    //{
    //    PsoundManager.instance.SFX.PlayOneShot(voicesounds[3]);
    //}
    void attacksound()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[4]);
    }
    void attacksoundII()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[5]);
    }
    void attacksoundIII()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[6]);
    }
    void attacksoundIIII()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[7]);
    }
    void archeryStart()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[8]);
    }
    void archeryEnd()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[9]);
    }
    void bombStart()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[10]);
    }
    void bombThrowing()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[11]);
    }
    void downsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[12]);
    }
    public void BombExplosion()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[13]);
    }
    #endregion
    #region 효과음
    void whirlwindsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[1]);
    }
    void arrowsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[2]);
    }
    public void iceCreative()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[3]);
    }
    public static bool icetrigger = false;
    public void iceDestroy()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[4]);
    }
    void DashATTACKsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[5]);
    }

    #endregion
    
    #region 걷기
    void walking()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[0]);
    }
    void running()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[6]);
    }
    #endregion
}
