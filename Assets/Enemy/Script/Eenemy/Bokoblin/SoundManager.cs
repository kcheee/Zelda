using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }


    public AudioClip[] audios;

    public void OnMyBuffSound()
    {   
        AudioSource.PlayClipAtPoint(audios[0], transform.position);
    }

    public void OnMyAttackSound()
    {
        AudioSource.PlayClipAtPoint(audios[1], transform.position);
    }

    public void OnMyDieSound()
    {
        //if(gameObject != null)
        //{
        //    Debug.Log(transform.position);
        //    AudioSource.PlayClipAtPoint(audios[2], transform.position);
        //}       
    }
}
