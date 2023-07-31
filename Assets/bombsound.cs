using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bombsound : MonoBehaviour
{
    public AudioClip audioClip; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        Invoke("DestroyObject", audioClip.length); 
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    //public AudioClip bombSFX;

    //private AudioSource audioSource;

    //private void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //}

    //private void OnDestroy()
    //{
    //    if (audioSource != null && bombSFX != null)
    //    {
    //        StartCoroutine(PlaySoundAndDestroy());
    //    }
    //}

    //private IEnumerator PlaySoundAndDestroy()
    //{
    //    Debug.Log("Playing sound before destruction");
    //    audioSource.PlayOneShot(bombSFX);

    //    yield return new WaitForSeconds(bombSFX.length); // 오디오 길이만큼 대기

    //    Destroy(gameObject);
    //}
}
