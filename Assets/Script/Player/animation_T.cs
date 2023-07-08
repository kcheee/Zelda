using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class animation_T : MonoBehaviour
{
    #region ½Ì±ÛÅæ
    static public animation_T instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    int anistack = 0;
    float timestack = 0;
    int chargestack = 0;
    float maxstack = 0.5f;

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
        anistack = 0;
    }

    private void Update()
    {

        // animator°¡ move »óÅÂÀÏ ¶§ »óÅÂ´Â move·Î ¹Ù²ñ.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) state = ani_state.idle;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("move")) state = ani_state.move;

        //if (Input.GetKeyDown(KeyCode.Mouse0)) animator.SetTrigger("attack");
        //if (Input.GetKeyDown(KeyCode.Mouse1)) animator.SetTrigger("attack2");
        //if (Input.GetKeyDown(KeyCode.Mouse2)) animator.SetTrigger("attack3");
        if(anistack > 0)
        {
            timestack += Time.deltaTime;
            Debug.Log(timestack);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anistack++;

            if (anistack == 1)
            {
                animator.SetTrigger("attack");
            }
            else if (anistack == 2 )
            {
                if (timestack <= maxstack)
                {
                    animator.SetTrigger("attack2");
                }
                if(timestack >= maxstack)
                {
                    anistack = 0;
                }

            }
            else if (anistack == 3 )
            {
                if (timestack <= maxstack)
                {
                    animator.SetTrigger("attack3");
                    anistack = 0;

                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            chargestack++;
            if (chargestack == 1)
            {
                animator.SetTrigger("charged");
            }
            else if (chargestack == 2)
            {
                animator.SetTrigger("charged2");
                chargestack = 0;
            }
        }

        if (chargestack == 1 && anistack == 1)
        {
            animator.SetTrigger("combo1");
        }
        else if (chargestack == 2 && anistack == 1)
        {
            animator.SetTrigger("combo2");
            chargestack = 0;
            anistack = 0;
        }

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
