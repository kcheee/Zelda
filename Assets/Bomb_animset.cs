using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_animset : StateMachineBehaviour
{
    // 던지는 애니메이션 시작 시
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 처음만 타임 스케일 느려지게
        if (!SkillManager.instance.Bomb_flag)
        {
            Time.timeScale = 0.5f;
            animator.speed = 1.2f;
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SkillManager.instance.Bomb_flag)
        {
            Time.timeScale = 1;

        }
    }
}
