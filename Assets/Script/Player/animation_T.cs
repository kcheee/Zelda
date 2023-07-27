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

    #region 민경님 변수
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

    private int comboCount = 0; // 현재 콤보 카운트
    private float lastAttackTime = 0f; // 마지막 공격 시간
    public float comboTimeThreshold = 1f; // 콤보 시간 제한
    public string[] attackAnimations; // 공격 애니메이션 이름 배열
    public BoxCollider AttackCollider;

    void AttackCombo()
    {
        // 공격 버튼을 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = ani_state.attack;
            // 콤보 시간 내에 공격 버튼이 눌리면 콤보 카운트 증가
            if (Time.time - lastAttackTime < comboTimeThreshold)
            {

                comboCount++;
                // 콤보 카운트에 따라 공격 애니메이션 재생
                if (comboCount < attackAnimations.Length)
                {
                    animator.SetTrigger(attackAnimations[comboCount]);
                }
                else
                {
                    // 콤보가 끝났을 때 초기화
                    comboCount = 0;
                    animator.SetTrigger(attackAnimations[0]);
                }
            }
            else
            {
                // 콤보 시간을 초과한 경우 콤보 초기화
                comboCount = 0;
                animator.SetTrigger(attackAnimations[0]);

            }
            lastAttackTime = Time.time;
        }
    }

    #region 공중에 뜬 상태

    public enum Ground_state
    {
        grounded,
        air
    }

    public Ground_state G_state;

    #endregion
    bool isGrounded;

    // 그라운드 체크  공중에 있을때 링크 애니메이션
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
            Debug.Log("공중");
            animator.SetBool("AirBorne", true);
        }
    }

    //강공격
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
    #region 임시
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
    //#region 민경님 코드
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
