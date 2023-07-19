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

    public bool isBuffSoundPlaying;
    public bool isAttackSoundPlaying;
    public bool isDieSoundPlaying;
    public AudioClip[] audios;

    public void OnMyBuffSound()
    {
        // ũ�ƾ�
        AudioSource.PlayClipAtPoint(audios[0], transform.position);
        isBuffSoundPlaying = true;
    }

    public void OnMyAttackSound()
    {
        // ũ�ƾ�
        AudioSource.PlayClipAtPoint(audios[1], transform.position);
        isAttackSoundPlaying = true;
    }

    public void OnMyDieSound()
    {
        // ũ�ƾ�
        AudioSource.PlayClipAtPoint(audios[2], transform.position);
        isDieSoundPlaying = true;
    }
}
