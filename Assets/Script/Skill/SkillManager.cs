using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public IEnumerator create_bomb()
    {
        // ��ƼŬ�� ��ź ����
        BOMB[Bomb_count] = Instantiate(bomb, Bomb_po.position, CameraRotation.rotation);
        BOMB[Bomb_count].transform.parent = Bomb_po.transform;
        // ��� ����
        if (Bomb_count == 0)
        {
            anim.speed = 0.02f;
            yield return new WaitForSeconds(0.5f);
            Bomb_flag = true;   // �ִϸ��̼� ���¿� �پ��ִ� ��ũ��Ʈ �Լ� ����
        }     
        anim.speed = 1;
        
    }
    public void ThrowBomb()
    {
        Bomb_po.transform.DetachChildren();
        // rigidbody ����ؼ� �� ������
        BOMB[Bomb_count].GetComponent<Rigidbody>().useGravity = true;
        // �� ������ ��
        BOMB[Bomb_count].GetComponent<Rigidbody>().AddForce(transform.forward * 10+transform.up*5, ForceMode.Impulse);
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
                    gameObject.GetComponent<Animator>().SetTrigger("charged2");
                    // ��ų ��Ÿ��
                    CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;
                    flag_icemaker = true;
                    StartCoroutine(IcemakerGravity());
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                    rb.AddForce(transform.up * 15 * rb.mass, ForceMode.Impulse);                   
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
        // �뽬�Ҷ� ���ں� ƨ�ܳ����� �ڵ�
        if (collision.collider.name.Contains("Boco"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            //Debug.Log(rb);

            rb.AddForce(Vector3.up * 10 + -transform.forward * 15, ForceMode.Impulse);
            //rb.velocity=GMrb.velocity*2.1f;
        }

        if (collision.collider.CompareTag("IceMaker"))
        {
            // ���� �ʿ�
            gameObject.GetComponent<Animator>().applyRootMotion = false;    // ��Ʈ��� ����
        }
    }
}
