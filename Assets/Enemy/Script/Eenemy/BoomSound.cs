using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSound : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.instance.OnMyBoomSound();

    }
}
