using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerMolblin : MonoBehaviour
{
    public static SoundManagerMolblin instance;
    private void Awake()
    {
        instance = this;
    }

    public AudioSource[] sources;
    public AudioClip[] audios;

    public void OnMyBuffSound()
    {
        //AudioSource.PlayClipAtPoint(audios[0], transform.position);
        sources[0].PlayOneShot(sources[0].clip);
    }

    public void OnMyKickSound()
    {
        //AudioSource.PlayClipAtPoint(audios[1], transform.position);
        sources[1].PlayOneShot(sources[1].clip);
    }

    public void OnMyOneHandSound()
    {
        //AudioSource.PlayClipAtPoint(audios[2], transform.position);
        sources[2].PlayOneShot(sources[2].clip);
    }

    public void OnMyTwoHandSound()
    {
        //AudioSource.PlayClipAtPoint(audios[3], transform.position);
        sources[3].PlayOneShot(sources[3].clip);
    }

    public void OnMyDieSound()
    {
        //AudioSource.PlayClipAtPoint(audios[4], transform.position);
        sources[4].PlayOneShot(sources[4].clip);
    }

    public void OnMyMolClubSound()
    {
        //AudioSource.PlayClipAtPoint(audios[5], transform.position);
        sources[5].PlayOneShot(sources[5].clip);
    }

    public void OnMyBoomSound()
    {
        //AudioSource.PlayClipAtPoint(audios[4], transform.position);
        sources[6].PlayOneShot(sources[6].clip);
    }
}
