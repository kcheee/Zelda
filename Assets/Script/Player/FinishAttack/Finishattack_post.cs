using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Finishattack_post : MonoBehaviour
{
    Volume volume;
    private void Start()
    {
        volume = GetComponent<Volume>();
    }
    // Update is called once per frame
    void Update()
    {

        if(!FinishAttack_.Finishattack)
        {
            if(volume.weight > 0) 
            volume.weight -= Mathf.Lerp(0, 1, Time.deltaTime * 0.8f);
        }
    }
}
