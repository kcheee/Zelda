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

    // public AudioClip audioclip;
    public AudioSource[] sources;

    public void OnMyBuffSound()
    {
        //AudioSource.PlayClipAtPoint(audios[0], transform.position);
        sources[0].PlayOneShot(sources[0].clip);
    }

    public void OnMyAttackSound()
    {
        //AudioSource.PlayClipAtPoint(audios[1], transform.position);
        sources[1].PlayOneShot(sources[1].clip);
    }

    public void OnMyClubSound()
    {
        sources[2].PlayOneShot(sources[2].clip);
    }

    //public void OnMyDieSound()
    //{
    //    //AudioSource.PlayClipAtPoint(audios[2], transform.position);
    //    sources[3].PlayOneShot(sources[3].clip);
    //}

    //public void OnMyBoomSound()
    //{
    //    sources[4].PlayOneShot(sources[4].clip);
    //}
}
