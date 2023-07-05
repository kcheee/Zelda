using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CoolTimer;


public class SkillManager : MonoBehaviour
{

    // region을 사용해서 가독성 높이기.

    #region 싱글톤

    static public SkillManager instance;

    private void Awake()
    {

        SkillManager.instance = this;
    }
    #endregion

    // 스킬 상태
    public enum Skill_state
    {
        None,
        skill_state,   // 스킬 창 열 때,
        skill_bow,
        skill_bomb,
        skill_coolTime
    }

    public Skill_state skill_state;
    public IceSkill iceskill;
    public Bomb bomb;
    public Transform CameraRotation;    // 카메라 회전 값 가져옴. 폭탄 화살 던질때 사용.
    public 

    Rigidbody rb;

    public Transform firePosition;

    private void Start()
    {
        skill_state = Skill_state.None;
        rb = GetComponentInParent<Rigidbody>();
    }
    // 폭탄 스킬.

    IEnumerator Bomb()
    {
        skill_state = Skill_state.skill_bomb;
        yield return new WaitForSeconds(0.5f);
        Instantiate(bomb, firePosition.position, CameraRotation.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(bomb, firePosition.position, CameraRotation.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(bomb, firePosition.position, CameraRotation.rotation);
        yield return new WaitForSeconds(0.8f);
        Instantiate(bomb, firePosition.position, CameraRotation.rotation);
        skill_state = Skill_state.None;
        // 스킬 쿨타임
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

    }

    // Time.scale 조절하는 불변수
    bool flag = false;
    // Update is called once per frame
    void Update()
    {     
        // 스킬 창 스킬
        if (CoolTimer.instance.cooltime == CoolTimer.CoolTime.None)
        {
            if (Input.GetKey(KeyCode.K))
            {
                if (!flag)
                    Time.timeScale = 0.3f;

                #region 아이스 발판
                if (Input.GetKeyDown(KeyCode.P))
                {
                    SkillUI.instance.Skillpanel.SetActive(false);
                    flag = true; Time.timeScale = 1;

                    // 스킬 쿨타임
                    CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                    rb.AddForce(transform.up * 10, ForceMode.Impulse);
                    Instantiate(iceskill, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z)
                        , transform.rotation);
                }
                #endregion

                #region 폭탄
                if (Input.GetKeyDown(KeyCode.O))
                {
                    // 스킬 패널 지움
                    SkillUI.instance.Skillpanel.SetActive(false);
                    flag = true; Time.timeScale = 1;
                    //Debug.Log(CameraRotation.eulerAngles.y);
                    transform.eulerAngles = new Vector3(0, CameraRotation.eulerAngles.y, 0);
                    // 폭탄
                    StartCoroutine(Bomb());
                }
                #endregion
            }
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            SkillUI.instance.Skillpanel.SetActive(false);
            flag = false; Time.timeScale = 1;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 대쉬할때 보코블린 튕겨나가는 코드
        if (collision.collider.name.Contains("Boco"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb);
            rb.AddForce(Vector3.up * 10 + -transform.forward * 15, ForceMode.Impulse);
            //rb.velocity=GMrb.velocity*2.1f;
            
        }
    }
}
