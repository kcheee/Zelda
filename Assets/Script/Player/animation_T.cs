using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_T : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    static public animation_T instance;
    private void Awake()
    {
            instance = this;
    }
    #endregion

    public enum ani_state
    {
        idle,
        move,
        run,
        dash,
        attack
    }
    public ani_state state;
    Animator animator;

    private void Start()
    {
        state = ani_state.idle;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //camera_PlayerMove ø°º≠ º≥¡§.
        if(state == ani_state.move) 
        {
            animator.SetBool("move", true);
        }
        else
            animator.SetBool("move", false);
    }
}
