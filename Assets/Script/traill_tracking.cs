using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class traill_tracking : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Trail.traill_track==true)
        {
            GameObject T = GameObject.Find("traill_po");
            Trail.instance.trail_offsets[Trail.instance.trail_index] = T.transform.position;
            Trail.instance.trail_index++;         
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Trail.instance.StartCoroutine(Trail.instance.Attack());
       // traill_offset √ ±‚»≠
        int flag = 0;
        while (true)
        {
            if (Trail.instance.trail_offsets[flag].magnitude == 0)
                break;
            Trail.instance.trail_offsets[flag] = Vector3.zero;
            flag++;
        }
    }
}
