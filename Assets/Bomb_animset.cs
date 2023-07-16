using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_animset : StateMachineBehaviour
{
    // ������ �ִϸ��̼� ���� ��
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ó���� Ÿ�� ������ ��������
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
