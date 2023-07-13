using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public GameObject followcam;
    // Update is called once per frame
    void Update()
    {
        transform.position = followcam.transform.position;
    }
}
