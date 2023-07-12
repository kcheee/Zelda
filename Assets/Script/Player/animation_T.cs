using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_T : MonoBehaviour
{
    static public animation_T instance;

    public Animator animator;

    public enum ani_state
    {
        idle,
        move,
        dash,
        run,
        attack
    }

    public ani_state state;

    int anistack = 0;
    int chargestack = 0;
    int combostack = 0;
    float durationOfCombo = 0.1f;
    float charged2WindowDuration = 0.5f;

    bool isCombomotionRunning = false;
    bool isCharged = false;
    bool isCharged2Ready = false;
    bool isCharged2WindowActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state = ani_state.idle;
        anistack = 0;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            state = ani_state.idle;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("move"))
            state = ani_state.move;

        Attackmotion();
        Chargedmotion();
        CheckCharged2Input();
        Combomotion();
    }

    void Attackmotion()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anistack++;

            if (anistack == 1 && chargestack == 0)
            {
                animator.SetTrigger("attack");
                chargestack = 0;
            }
            else if (anistack == 2 && chargestack == 0)
            {
                animator.SetTrigger("attack2");
            }
            else if (anistack == 3 && chargestack == 0)
            {
                animator.SetTrigger("attack3");
                anistack = 0;
            }
        }
    }

    void Chargedmotion()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            chargestack++;

            if (chargestack == 1 && anistack == 0)
            {
                animator.SetTrigger("charged");
                isCharged = true;
                isCharged2WindowActive = true;
                StartCoroutine(Charged2WindowCoroutine());
            }
            else if (chargestack == 2 && anistack == 0 && isCharged2Ready)
            {
                animator.SetTrigger("charged2");
                chargestack = 0;
                isCharged = false;
                isCharged2Ready = false;
                isCharged2WindowActive = false;
                StopCoroutine(Charged2WindowCoroutine());
            }
        }
    }

    void CheckCharged2Input()
    {
        if (isCharged && Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("charged");
            isCharged = false;
            isCharged2Ready = false;
            isCharged2WindowActive = true;
            StopCoroutine(Charged2WindowCoroutine());
        }
        else if (isCharged2WindowActive && Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("charged2");
            isCharged = false;
            isCharged2Ready = false;
            isCharged2WindowActive = false;
            StopCoroutine(Charged2WindowCoroutine());
        }
    }

    void Combomotion()
    {
        combostack++;
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (combostack == 1)
            {
                animator.SetTrigger("combo");
            }
            else if(combostack == 2)
            {
                animator.SetTrigger("combo2");
            }
        }

    }

    IEnumerator Charged2WindowCoroutine()
    {
        yield return new WaitForSeconds(charged2WindowDuration);

        isCharged2Ready = true;
        isCharged2WindowActive = false;
    }

    IEnumerator CombomotionCoroutine()
    {
        isCombomotionRunning = true;
        animator.SetTrigger("combo");
        print("p");

        yield return new WaitForSeconds(durationOfCombo);

        animator.SetTrigger("combo2");
        isCombomotionRunning = false;

        anistack = 0;
        chargestack = 0;
        isCharged2Ready = false;
        isCharged = false;
    }
}
