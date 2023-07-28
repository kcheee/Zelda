using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsoundManager : MonoBehaviour
{
    public static PsoundManager instance;
    //[SerializeField] AudioClip[] moveSound = default;
    public AudioSource BGM;
    public AudioClip stageBGM;
    public AudioSource SFX;
    public AudioClip SFXsound;

    private void Start()
    {
        BGM.loop = true;
        BGM.playOnAwake = true;
        BGM.clip = stageBGM;
        BGM.Play();

        SFX.loop = false;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SFX = GetComponent<AudioSource>();

        }
        else
            Destroy(gameObject);
    }

    //public void MoveSoundEffect()
    //{
    //    PsoundManager.instance.source.PlayOneShot(moveSound[0]);
    //}
}

