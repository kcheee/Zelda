using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsoundManager : MonoBehaviour
{
    public static PsoundManager instance;
    //[SerializeField] AudioClip[] moveSound = default;

    public AudioSource source;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            source = GetComponent<AudioSource>();

        }
        else
            Destroy(gameObject);
    }
    //public void MoveSoundEffect()
    //{
    //    PsoundManager.instance.source.PlayOneShot(moveSound[0]);
    //}
}
