using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation_anim : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Molblin1.anim_rotation = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Molblin1.anim_rotation = false;
    }

}
