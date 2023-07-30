using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static CoolTimer;


public class SkillManager : MonoBehaviour
{

    // region�� ����ؼ� ������ ���̱�.

    #region �̱���

    static public SkillManager instance;

    private void Awake()
    {

        SkillManager.instance = this;
    }
    #endregion

    // ��ų ����
    public enum Skill_state
    {
        None,
        skill_state,   // ��ų â �� ��,
        skill_bowzoom,
        skill_bow,
        skill_bomb,
        skill_coolTime
    }
    public Skill_state skill_state;

    // ��ų 
    public IceSkill iceskill;
    public GameObject bomb;

    public Transform CameraRotation;    // ī�޶� ȸ�� �� ������. ��ź ȭ�� ������ ���.
    public Transform firePosition;
    
    //�ʻ��.
    public GameObject FinishAttack;

    // Ȱ �� ī�޶� ���� �Һ���
    static public bool bowCamera = false;

    Rigidbody rb;
    Animator anim;

    private void Start()
    {
        skill_state = Skill_state.None;
        rb = GetComponentInParent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
    }

    // ī�޶� ����
    IEnumerator CameraRotate()
    {
        while (skill_state == Skill_state.skill_bomb)
        {
            transform.forward = Camera.main.transform.forward;

            // �÷��̾� ���Ʒ� ȸ�� ����.
            Vector3 dir = transform.eulerAngles;
            dir = new Vector3(0, dir.y, 0);
            transform.eulerAngles = dir;

            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    #region ��ź
    public bool Bomb_flag=false;
    public Transform Bomb_po;
    public int Bomb_count = 0;
    GameObject[] BOMB = new GameObject[4];

    // Į ����ֱ�.
    public GameObject Bomb_Ready;
    public GameObject[] sword_shield;
    public GameObject Bomb_startEffect;
    public IEnumerator create_bomb()
    {
        // ��ƼŬ�� ��ź ����
        BOMB[Bomb_count] = Instantiate(bomb, Bomb_po.position, CameraRotation.rotation);
        BOMB[Bomb_count].transform.parent = Bomb_po.transform;
        // ��� ����
        if (Bomb_count == 0)
        {
            Instantiate(Bomb_startEffect, Bomb_po.position, CameraRotation.rotation);
            anim.speed = 0.02f;
            yield return new WaitForSeconds(0.5f);
            Bomb_flag = true;   // �ִϸ��̼� ���¿� �پ��ִ� ��ũ��Ʈ �Լ� ����
        }     
        anim.speed = 1;
        
    }
    public void ThrowBomb()
    {
        // �θ� �� ȣ���ڽ�..
        Bomb_po.transform.DetachChildren();
        // rigidbody ����ؼ� �� ������
        BOMB[Bomb_count].GetComponent<Rigidbody>().useGravity = true;
        // �� ������ ��
        if (Bomb_count <3)
        BOMB[Bomb_count].GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(12,20) + transform.up*3, ForceMode.Impulse);
        else
            BOMB[Bomb_count].GetComponent<Rigidbody>().AddForce(transform.forward * 25 + transform.up * 5, ForceMode.Impulse);
        // �� ȸ���� �������� ȸ����.
        BOMB[Bomb_count].GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 1.5f, ForceMode.Impulse);
        Bomb_count++;

        if (Bomb_count == 4) Bomb_count = 0;
    }
    // ��ź ������ �ڷ�ƾ
    IEnumerator Bomb()
    {
        skill_state = Skill_state.skill_bomb;
        // Į ���� ����ֱ�
        Bomb_Ready.SetActive(true);
        sword_shield[0].SetActive(false); sword_shield[1].SetActive(false);

        StartCoroutine(CameraRotate());
        anim.SetTrigger("Bomb");
      
        yield return new WaitForSeconds(4f);

        skill_state = Skill_state.None;
        // ��ų ��Ÿ��
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

        // Į ���� ������
        Bomb_Ready.SetActive(false);
        sword_shield[0].SetActive(true); sword_shield[1].SetActive(true);

    }
    #endregion
    // Time.scale �����ϴ� �Һ���
    bool flag = false;

    // Icemaker �߷� ����
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

        #region ��ų â ��ų
        // ��ų â ��ų
        if (CoolTimer.instance.cooltime == CoolTimer.CoolTime.None)
        {
            if (Input.GetKey(KeyCode.K))
            {
                if (!flag)
                    Time.timeScale = 0.3f;

                #region ���̽� ����
                if (Input.GetKeyDown(KeyCode.P))
                {
                    SkillUI.instance.Skillpanel.SetActive(false);
                    flag = true; Time.timeScale = 1;

                    gameObject.GetComponent<Animator>().applyRootMotion = true;
                    //gameObject.GetComponent<Animator>().SetTrigger("jump");
                    // ��ų ��Ÿ��
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

                #region ��ź
                if (Input.GetKeyDown(KeyCode.O))
                {
                    // ��ų �г� ����
                    SkillUI.instance.Skillpanel.SetActive(false);
                    flag = true; Time.timeScale = 1;
                    //Debug.Log(CameraRotation.eulerAngles.y);
                    transform.eulerAngles = new Vector3(0, CameraRotation.eulerAngles.y, 0);
                    // ��ź
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

        #region �ʻ��

        if(Input.GetKeyDown(KeyCode.N))
        {
            FinishAttack.SetActive(true);
        }

        #endregion
    }

    // ���� �ʿ� IceSkill���� ����
    IEnumerator IcemakerGravity()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (flag_icemaker)
        {
            // ������ �ִ� ��.
            rb.AddForce(Vector3.down * 40 * rb.mass);
            yield return new WaitForSeconds(0.02f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("IceMaker"))
        {
            // ���� �ʿ�
            //gameObject.GetComponent<Animator>().applyRootMotion = false;    // ��Ʈ��� ����
        }
    }
}
