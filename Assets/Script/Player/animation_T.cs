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
        dash,
        run,
        attack
    }
    public ani_state state;
    public Animator animator;

    private void Start()
    {
        state = ani_state.idle;
    }

    private void Update()
    {

        //camera_PlayerMove ø°º≠ º≥¡§.
        Moving();       
    }

    private void Moving()
    {
        //if(state == ani_state.move) 
        //{
        //    animator.SetBool("move", true);
        //}
        //else animator.SetBool("move", false);

        //if (state == ani_state.dash)
        //{
        //    animator.SetBool("dash", true);       
        //}
        //else animator.SetBool("dash", false);

        //if (state == ani_state.run)
        //{
        //    animator.SetBool("run", true);
        //}
        //else animator.SetBool("run", false);
    }

}
