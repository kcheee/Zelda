using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Finishattack_post : MonoBehaviour
{
     public Volume volume;
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {   
        if(volume.weight > 0 )
        {
        volume.weight -= Mathf.Lerp(0, 1, Time.deltaTime * 1.8f);
        }

    }
}
