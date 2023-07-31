using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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
        attack,
        dashattack,
        FinishAttack
    }

    public ani_state state;

    #region dash

    public static bool Dash_flag;
    float ti;

    IEnumerator dashattack()
    {
        if(Dash_flag)
        {
            yield return new WaitForSeconds(0.1f);
            Collider[] colliders =
              Physics.OverlapSphere(this.transform.position, 8);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Bokoblin"))
                {
                    Rigidbody[] rigid = colliders[i].GetComponentsInChildren<Rigidbody>();
                    foreach (Rigidbody rb in rigid)
                    {
                        rb.AddForce(transform.up * 2, ForceMode.Impulse);
                        rb.velocity = gameObject.GetComponent<Rigidbody>().velocity;
                    }
                    colliders[i].GetComponentInParent<RagdollBokoblin>().DamagedProcess();
                }
            }
            StartCoroutine(dashattack());
        }
        else
        {
            Collider[] colliders_ =
            Physics.OverlapSphere(this.transform.position, 10);
            for (int i = 0; i < colliders_.Length; i++)
            {
                if (colliders_[i].CompareTag("Bokoblin"))
                {
                    Rigidbody[] rigid = colliders_[i].GetComponentsInChildren<Rigidbody>();
                 
                    foreach (Rigidbody rb in rigid)
                    {
                        rb.velocity = gameObject.GetComponent<Rigidbody>().velocity;
                        rb.AddForce(transform.forward * 8, ForceMode.Impulse);
                    }
                    colliders_[i].GetComponentInParent<RagdollBokoblin>().DamagedProcess();

                }
            }
        }

        yield return null;
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state = ani_state.idle;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !FinishAttack_.Finishattack)
        {
            state = ani_state.idle;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("move"))
            state = ani_state.move;

        if (state == ani_state.attack || FinishAttack_.Finishattack 
            || G_state == Ground_state.air || SkillManager.instance.skill_state == SkillManager.Skill_state.skill_bomb)
        {
            //AttackCollider.enabled = true;
            animator.applyRootMotion = true;
        }
        else
        {
            //AttackCollider.enabled = false;
            animator.applyRootMotion = false;
        }


        #region 대쉬
        if (Input.GetKeyDown(KeyCode.R))
        {
            Dash_flag = true;
            StartCoroutine(dashattack());
            animator.SetTrigger("DashAttack");
        }
        if (Dash_flag)
        {
            state = ani_state.dashattack;
            ti += Time.deltaTime;
            if (ti > 2.5f)
            {
                Dash_flag = false;
                ti = 0;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = transform.forward * 20;
            }
        }
        #endregion

        #region 차지어택
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("ChargeAttack");
            animator.SetBool("charged", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("charged", false);
        }
        #endregion

        AttackCombo();
        CheckGrounded();

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
                G_state = Ground_state.grounded;
            }
        }
        // 공중에 떠있을때 애니메이션 실행 가능하게
        else if (state != ani_state.dashattack && state != ani_state.attack
            && SkillManager.instance.skill_state != SkillManager.Skill_state.skill_bomb
            && SkillManager.instance.skill_state != SkillManager.Skill_state.skill_bowzoom
            && SkillManager.instance.skill_state != SkillManager.Skill_state.skill_bow)
        {

            animator.SetBool("AirBorne", true);
            G_state = Ground_state.air;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bokoblin_sword"))
        {
            Debug.Log(other.gameObject);
            // 0보다 크면
            if (PlayerManager.instance.HP > 0)
            {
                animator.SetTrigger("Hit");
                PlayerManager.instance.PlayerDamaged();
            }
        }
    }

    public GameObject DashEft;
    // Dash ani Event;
    public bool Dash()
    {
        DashEft.SetActive(true);
        return animation_T.Dash_flag = true;
    }
    public void DashEnd()
    {
        DashEft.SetActive(false);
    }

    #region ChargeAtk
    public GameObject ChargeEft;

    #region charge Event
    public void chargedstart()
    {
        ChargeEft.SetActive(true);
    }
    public void ChargeAtkend()
    {
        ChargeEft.SetActive(false);
    }
    #endregion
    #endregion


}
