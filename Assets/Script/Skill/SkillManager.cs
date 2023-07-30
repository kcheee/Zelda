using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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
        skill_bowzoom,
        skill_bow,
        skill_bomb,
        skill_coolTime
    }
    public Skill_state skill_state;

    // 스킬 
    public IceSkill iceskill;
    public GameObject bomb;

    public Transform CameraRotation;    // 카메라 회전 값 가져옴. 폭탄 화살 던질때 사용.
    public Transform firePosition;
    
    //필살기.
    public GameObject FinishAttack;

    // 활 쏠때 카메라 줌인 불변수
    static public bool bowCamera = false;

    Rigidbody rb;
    Animator anim;

    private void Start()
    {
        skill_state = Skill_state.None;
        rb = GetComponentInParent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
    }

    // 카메라 방향
    IEnumerator CameraRotate()
    {
        while (skill_state == Skill_state.skill_bomb)
        {
            transform.forward = Camera.main.transform.forward;

            // 플레이어 위아래 회전 제한.
            Vector3 dir = transform.eulerAngles;
            dir = new Vector3(0, dir.y, 0);
            transform.eulerAngles = dir;

            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    #region 폭탄
    public bool Bomb_flag=false;
    public Transform Bomb_po;
    public int Bomb_count = 0;
    GameObject[] BOMB = new GameObject[4];

    // 칼 집어넣기.
    public GameObject Bomb_Ready;
    public GameObject[] sword_shield;
    public GameObject Bomb_startEffect;
    public IEnumerator create_bomb()
    {
        // 파티클과 폭탄 생성
        BOMB[Bomb_count] = Instantiate(bomb, Bomb_po.position, CameraRotation.rotation);
        BOMB[Bomb_count].transform.parent = Bomb_po.transform;
        // 잠깐 멈춤
        if (Bomb_count == 0)
        {
            Instantiate(Bomb_startEffect, Bomb_po.position, CameraRotation.rotation);
            anim.speed = 0.02f;
            yield return new WaitForSeconds(0.5f);
            Bomb_flag = true;   // 애니메이션 상태에 붙어있는 스크립트 함수 제어
        }     
        anim.speed = 1;
        
    }
    public void ThrowBomb()
    {
        // 부모를 뗌 호로자식..
        Bomb_po.transform.DetachChildren();
        // rigidbody 사용해서 공 던지기
        BOMB[Bomb_count].GetComponent<Rigidbody>().useGravity = true;
        // 공 던지는 힘
        if (Bomb_count <3)
        BOMB[Bomb_count].GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(12,20) + transform.up*3, ForceMode.Impulse);
        else
            BOMB[Bomb_count].GetComponent<Rigidbody>().AddForce(transform.forward * 25 + transform.up * 5, ForceMode.Impulse);
        // 공 회전값 무작위로 회전함.
        BOMB[Bomb_count].GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 1.5f, ForceMode.Impulse);
        Bomb_count++;

        if (Bomb_count == 4) Bomb_count = 0;
    }
    // 폭탄 던지는 코루틴
    IEnumerator Bomb()
    {
        skill_state = Skill_state.skill_bomb;
        // 칼 방패 집어넣기
        Bomb_Ready.SetActive(true);
        sword_shield[0].SetActive(false); sword_shield[1].SetActive(false);

        StartCoroutine(CameraRotate());
        anim.SetTrigger("Bomb");
      
        yield return new WaitForSeconds(4f);

        skill_state = Skill_state.None;
        // 스킬 쿨타임
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

        // 칼 방패 꺼내기
        Bomb_Ready.SetActive(false);
        sword_shield[0].SetActive(true); sword_shield[1].SetActive(true);

    }
    #endregion
    // Time.scale 조절하는 불변수
    bool flag = false;

    // Icemaker 중력 변수
   static public bool flag_icemaker=false;
    // Update is called once per frame
    void Update()
    {
        #region item Panel
        if (Input.GetKey(KeyCode.J))
        {
            if (!flag)
                Time.timeScale = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            SkillUI.instance.ItemPanel.SetActive(false);
            flag = false; Time.timeScale = 1;
        }
        #endregion

        #region 스킬 창 스킬
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

                    gameObject.GetComponent<Animator>().applyRootMotion = true;
                    //gameObject.GetComponent<Animator>().SetTrigger("jump");
                    // 스킬 쿨타임
                    CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;
                    flag_icemaker = true;
                    StartCoroutine(IcemakerGravity());
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                    Debug.Log(gameObject.GetComponent<Animator>().applyRootMotion);
                    rb.velocity = new Vector3(0, 0, 0);
                    //rb.AddForce(transform.up * 15, ForceMode.Impulse);
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
        #endregion

        #region 필살기

        if(Input.GetKeyDown(KeyCode.N))
        {
            FinishAttack.SetActive(true);
        }

        #endregion
    }

    // 수정 필요 IceSkill에서 제어
    IEnumerator IcemakerGravity()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (flag_icemaker)
        {
            // 밑으로 주는 힘.
            rb.AddForce(Vector3.down * 40 * rb.mass);
            yield return new WaitForSeconds(0.02f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("IceMaker"))
        {
            // 수정 필요
            //gameObject.GetComponent<Animator>().applyRootMotion = false;    // 루트모션 해제
        }
    }
}
