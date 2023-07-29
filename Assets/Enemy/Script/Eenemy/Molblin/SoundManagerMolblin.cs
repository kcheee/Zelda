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

    public AudioClip[] audios;

    public void OnMyBuffSound()
    {
        AudioSource.PlayClipAtPoint(audios[0], transform.position);
    }

    public void OnMyKickSound()
    {
        AudioSource.PlayClipAtPoint(audios[1], transform.position);
    }

    public void OnMyOneHandSound()
    {
        AudioSource.PlayClipAtPoint(audios[2], transform.position);
    }

    public void OnMyTwoHandSound()
    {
        AudioSource.PlayClipAtPoint(audios[3], transform.position);
    }

    public void OnMyDieSound()
    {
        AudioSource.PlayClipAtPoint(audios[4], transform.position);
    }
}
