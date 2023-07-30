using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psoundscript : MonoBehaviour
{
    [SerializeField] AudioClip[] voicesounds = default;
    [SerializeField] AudioClip[] SFXsounds = default;
    //[SerializeField] AudioClip[] Walkingsounds = default;
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
    void dashsoundstop()
    {
        PsoundManager.instance.SFX.PlayOneShot(voicesounds[3]);
    }
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
    #endregion
    #region 효과음
    void runing()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[0]);
    }
    void whirlwindsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[1]);
    }
    //void swordAttacksound()
    //{
    //    PsoundManager.instance.SFX.PlayOneShot(SFXsounds[2]);
    //}
    //void swordAttacksoundII()
    //{
    //    PsoundManager.instance.SFX.PlayOneShot(SFXsounds[3]);

    //}
    //void swordAttacksoundIII()
    //{
    //    PsoundManager.instance.SFX.PlayOneShot(SFXsounds[4]);
    //}
    //void swordAttacksoundIIII()
    //{
    //    PsoundManager.instance.SFX.PlayOneShot(SFXsounds[5]);
    //}
    void arrowsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[2]);
    }
    void iceCreative()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[3]);
    }
    void iceDestroy()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[4]);
    }
    void DashATTACKsound()
    {
        PsoundManager.instance.SFX.PlayOneShot(SFXsounds[5]);
    }

    #endregion
    #region 걷기효과음
    void walking()
    {
        //PsoundManager.instance.SFX.PlayOneShot(Walkingsounds[Random.Range(0, Walkingsounds.Length)]);
    }
    #endregion
}
//    [System.Serializable]
//    public struct AudioType
//    {
//        public string soundname;
//        public AudioClip PSound;
//    }
//    public AudioType[] PSList;

//    AudioSource playersound;

//    public enum arraysound
//    {
//        ATTACKI,
//        CHARGEDI,
//        CHARGEDII,
//        DASHSTART,
//        DASHSTOP
//    }
//    public arraysound state;
//    private void Update()
//    {
//        //switch (state)
//        //{
//        //    case arraysound.ATTACKI:
//        //        ATTACKIsound();
//        //        break;
//        //    case arraysound.CHARGEDI:
//        //        CHARGEDIsound();
//        //        break;
//        //    case arraysound.CHARGEDII:
//        //        CHARGEDIIsound();
//        //        break;
//        //    case arraysound.DASHSTART:
//        //        DASHSTARTsound();
//        //        break;
//        //    case arraysound.DASHSTOP:
//        //        DASHSTOPsound();
//        //        break;
//        //    default:
//        //        break;
//        //}
//    }

//    private void DASHSTOPsound()
//    {
//        //AudioType[""]

//    }

//    private void DASHSTARTsound()
//    {
//        playersound.playOnAwake = true;
//    }

//    private void CHARGEDIIsound()
//    {
//    }

//    private void CHARGEDIsound()
//    {

//    }

//    void ATTACKIsound()
//    {

//    }
//    #region 전
//    //public List<Psound> sounds;
//    //// Start is called before the first frame update
//    //void Start()
//    //{
//    //    DisableSoundes();
//    //}

//    //// Update is called once per frame
//    //void Update()
//    //{

//    //}
//    //void StartSlash()
//    //{
//    //    StartCoroutine(SoundAttack());
//    //}
//    //void StopSlash()
//    //{
//    //    DisableSoundes();
//    //}
//    //void StartSlash2()
//    //{
//    //    StartCoroutine(SoundAttack());
//    //}
//    //void StopSlash2()
//    //{
//    //    DisableSoundes();
//    //}

//    //void StartSlash3()
//    //{
//    //    StartCoroutine(SoundAttack());
//    //}
//    //void StopSlash3()
//    //{
//    //    DisableSoundes();
//    //}

//    //IEnumerator SoundAttack()
//    //{
//    //    for (int i = 0; i < sounds.Count; i++)
//    //    {
//    //        yield return new WaitForSeconds(sounds[i].delay);
//    //        sounds[i].soundef.SetActive(true);
//    //    }
//    //    yield return new WaitForSeconds(1);
//    //}
//    //void DisableSoundes()
//    //{
//    //    for (int i = 0; i < sounds.Count; i++)
//    //        sounds[i].soundef.SetActive(false);
//    //}
//    //[System.Serializable]
//    //public class Psound
//    //{
//    //    public GameObject soundef;
//    //    public float delay;
//    //}
//    #endregion
//}
