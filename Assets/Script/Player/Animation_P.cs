using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_P : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int attackStack = 0;
    public static int chargedStack = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            anim.SetBool("attack", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            anim.SetBool("attack2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
        {
            anim.SetBool("attack3", false);
            attackStack = 0;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("charged"))
        {
            anim.SetBool("charged", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("charged2"))
        {
            anim.SetBool("charged2", false);
            chargedStack = 0;
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            attackStack = 0;
            chargedStack = 0;
        }
        if(Time.time> nextFireTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Charged();
            }
        }
    }
    
    void Attack()
    {
        lastClickedTime = Time.time;
        attackStack++;
        if(attackStack == 1)
        {
            anim.SetBool("attack", true);
        }
        attackStack = Mathf.Clamp(attackStack, 0, 3);
        if(attackStack >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            anim.SetBool("attack", false);
            anim.SetBool("attack2", true);
        }
        if(attackStack >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", true);
        }
    }
    void Charged()
    {
        lastClickedTime = Time.time;
        chargedStack++;
        if (chargedStack == 1)
        {
            anim.SetBool("charged", true);
        }
        chargedStack = Mathf.Clamp(chargedStack, 0, 3);
        if (chargedStack >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("charged"))
        {
            anim.SetBool("charged", false);
            anim.SetBool("charged2", true);
        }
    }
}
