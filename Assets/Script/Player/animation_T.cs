using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class animation_T : MonoBehaviour
{
    static public animation_T instance;

    public Animator animator;

    //public List<Slash> slashes;
    public enum ani_state
    {
        idle,
        move,
        dash,
        run,
        attack,
        charged
    }

    public ani_state state;

    #region �ΰ�� ����
    int anistack = 0;
    int chargestack = 0;
    int combostack = 0;
    float durationOfCombo = 0.1f;
    float charged2WindowDuration = 0.5f;

    bool isCombomotionRunning = false;
    bool isCharged = false;
    bool isCharged2Ready = false;
    bool isCharged2WindowActive = false;
    #endregion 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //DisableSlashes();
        state = ani_state.idle;
        anistack = 0;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            state = ani_state.idle;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("move"))
            state = ani_state.move;

        if (state == ani_state.attack || G_state == Ground_state.air)
        {
            //AttackCollider.enabled = true;
            animator.applyRootMotion = true;
        }
        else
        {
            //AttackCollider.enabled = false;
            animator.applyRootMotion = false;
        }

        //if(state == ani_state.charged||G_state == Ground_state.air)
        //{
        //    animator.applyRootMotion = true;
        //}
        //else
        //{
        //    animator.applyRootMotion = false;
        //}
        //Attackmotion();
        //Chargedmotion();
        //CheckCharged2Input();
        //Combomotion();

        AttackCombo();
        CheckGrounded();
        Chargedmotion();


    }

    private int comboCount = 0; // ���� �޺� ī��Ʈ
    private float lastAttackTime = 0f; // ������ ���� �ð�
    public float comboTimeThreshold = 1f; // �޺� �ð� ����
    public string[] attackAnimations; // ���� �ִϸ��̼� �̸� �迭
    public BoxCollider AttackCollider;

    void AttackCombo()
    {
        // ���� ��ư�� ������ ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = ani_state.attack;
            // �޺� �ð� ���� ���� ��ư�� ������ �޺� ī��Ʈ ����
            if (Time.time - lastAttackTime < comboTimeThreshold)
            {

                comboCount++;
                // �޺� ī��Ʈ�� ���� ���� �ִϸ��̼� ���
                if (comboCount < attackAnimations.Length)
                {
                    animator.SetTrigger(attackAnimations[comboCount]);
                }
                else
                {
                    // �޺��� ������ �� �ʱ�ȭ
                    comboCount = 0;
                    animator.SetTrigger(attackAnimations[0]);
                }
            }
            else
            {
                // �޺� �ð��� �ʰ��� ��� �޺� �ʱ�ȭ
                comboCount = 0;
                animator.SetTrigger(attackAnimations[0]);

            }
            lastAttackTime = Time.time;
        }
    }

    #region ���߿� �� ����

    public enum Ground_state
    {
        grounded,
        air
    }

    public Ground_state G_state;

    #endregion
    bool isGrounded;

    // �׶��� üũ  ���߿� ������ ��ũ �ִϸ��̼�
    private void CheckGrounded()
    {
        RaycastHit hitinfo;
        Vector3 dir = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Debug.DrawRay(dir, -transform.up, Color.red);
        if (Physics.Raycast(dir, -transform.up, out hitinfo, 1))
        {
            if (hitinfo.collider.CompareTag("Floor") || hitinfo.collider.CompareTag("IceMaker"))
            {
                animator.SetBool("AirBorne", false);
            }
        }
        else
        {
            Debug.Log("����");
            animator.SetBool("AirBorne", true);
        }
    }

    //������
    private void Chargedmotion()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Psoundscript.arraysound.ATTACKI();
            state = ani_state.charged;
            animator.SetTrigger("charged");
            //animator.SetTrigger("charged1");

            //if (chargestack == 1 && anistack == 0)
            //{
            //    animator.SetTrigger(Chargedmotions[]);
            //    //isCharged = true;
            //    //isCharged2WindowActive = true;
            //    //StartCoroutine(Charged2WindowCoroutine());
            //}
            //else if (chargestack == 2 && anistack == 0 && isCharged2Ready)
            //{
            //    animator.SetTrigger(Chargedmotions[0]);
            //    chargestack = 0;
            //    isCharged = false;
            //    isCharged2Ready = false;
            //    isCharged2WindowActive = false;
            //    //StopCoroutine(Charged2WindowCoroutine());
            //}

        }
    }
    #region �ӽ�
    //IEnumerator Charged2WindowCoroutine()
    //{
    //    yield return new WaitForSeconds(charged2WindowDuration);

    //    isCharged2Ready = true;
    //    isCharged2WindowActive = false;
    //}

    //IEnumerator SlashAttack()
    //{
    //    for(int i=0; i<slashes.Count; i++)
    //    {
    //        yield return new WaitForSeconds(slashes[i].delay);
    //        slashes[i].slashvfx.SetActive(true);
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    DisableSlashes();
    //}
    //void DisableSlashes()
    //{
    //    for (int i = 0; i < slashes.Count; i++)
    //        slashes[i].slashvfx.SetActive(false);
    //}
    //[System.Serializable]
    //public class Slash
    //{
    //    public GameObject slashvfx;
    //    public float delay;
    //}
    //#region �ΰ�� �ڵ�
    //void Attackmotion()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        anistack++;

    //        if (anistack == 1 && chargestack == 0)
    //        {
    //            animator.SetTrigger("attack");
    //            chargestack = 0;
    //        }
    //        else if (anistack == 2 && chargestack == 0)
    //        {
    //            animator.SetTrigger("attack2");
    //        }
    //        else if (anistack == 3 && chargestack == 0)
    //        {
    //            animator.SetTrigger("attack3");
    //            anistack = 0;
    //        }
    //    }
    //}



    //void CheckCharged2Input()
    //{
    //    if (isCharged && Input.GetKeyDown(KeyCode.Mouse1))
    //    {
    //        animator.SetTrigger("charged");
    //        isCharged = false;
    //        isCharged2Ready = false;
    //        isCharged2WindowActive = true;
    //        StopCoroutine(Charged2WindowCoroutine());
    //    }
    //    else if (isCharged2WindowActive && Input.GetKeyDown(KeyCode.Mouse1))
    //    {
    //        animator.SetTrigger("charged2");
    //        isCharged = false;
    //        isCharged2Ready = false;
    //        isCharged2WindowActive = false;
    //        StopCoroutine(Charged2WindowCoroutine());
    //    }
    //}

    //void Combomotion()
    //{
    //    combostack++;
    //    if (Input.GetKeyDown(KeyCode.Mouse2))
    //    {
    //        if (combostack == 1)
    //        {
    //            animator.SetTrigger("combo");
    //        }
    //        else if (combostack == 2)
    //        {
    //            animator.SetTrigger("combo2");
    //        }
    //    }

    //}

    //IEnumerator Charged2WindowCoroutine()
    //{
    //    yield return new WaitForSeconds(charged2WindowDuration);

    //    isCharged2Ready = true;
    //    isCharged2WindowActive = false;
    //}

    //IEnumerator CombomotionCoroutine()
    //{
    //    isCombomotionRunning = true;
    //    animator.SetTrigger("combo");
    //    print("p");

    //    yield return new WaitForSeconds(durationOfCombo);

    //    animator.SetTrigger("combo2");
    //    isCombomotionRunning = false;

    //    anistack = 0;
    //    chargestack = 0;
    //    isCharged2Ready = false;
    //    isCharged = false;
    //}

    #endregion
}
