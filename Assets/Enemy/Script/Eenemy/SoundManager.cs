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
        // if(RagdollBokoblin.instance.isBuffSoundPlaying == false)
        // {
            AudioSource.PlayClipAtPoint(audios[0], transform.position);
            //RagdollBokoblin.instance.isBuffSoundPlaying = true;
        // }
        // else if (RagdollBokoblin.instance.isBuffSoundPlaying)
        // {
            // return;
        // }
    }

    public void OnMyAttackSound()
    {
        //if (RagdollBokoblin.instance.isAttackSoundPlaying == false)
        //{
            AudioSource.PlayClipAtPoint(audios[1], transform.position);
        //    RagdollBokoblin.instance.isAttackSoundPlaying = true;
        //}
        //else if (RagdollBokoblin.instance.isAttackSoundPlaying)
        //{
        //    return;
        //}
    }

    public void OnMyDieSound()
    {
        if (RagdollBokoblin.instance.isDieSoundPlaying == false)
        {
            AudioSource.PlayClipAtPoint(audios[2], transform.position);
            RagdollBokoblin.instance.isDieSoundPlaying = true;
        }
        else if (RagdollBokoblin.instance.isDieSoundPlaying)
        {
            return;
        }
    }
}
