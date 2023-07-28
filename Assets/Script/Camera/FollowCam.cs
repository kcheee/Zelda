using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public GameObject followcam;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,followcam.transform.position,Time.deltaTime*10);
        
    }
}
