using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psoundscript : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds = default;
    void chargedstart()
    {
        PsoundManager.instance.source.PlayOneShot(sounds[0]);
    }
    void charging()
    {
        PsoundManager.instance.source.PlayOneShot(sounds[1]);
    }
    void dashsoundstart()
    {
        PsoundManager.instance.source.PlayOneShot(sounds[2]);
    }
    void dashsoundstop()
    {
        PsoundManager.instance.source.PlayOneShot(sounds[3]);
    }
    void attacksound()
    {
        PsoundManager.instance.source.PlayOneShot(sounds[4]);
    }
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
//    #region Àü
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
