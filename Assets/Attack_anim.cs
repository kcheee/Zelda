using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_anim : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animation_T.instance.state = animation_T.ani_state.attack;
    }
}
