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
        skill_bow,
        skill_bomb,
        skill_coolTime
    }

    public Skill_state skill_state;
    public IceSkill iceskill;
    public Bomb bomb;
    public Transform CameraRotation;    // ī�޶� ȸ�� �� ������. ��ź ȭ�� ������ ���.
    public 

    Rigidbody rb;

    public Transform firePosition;

    private void Start()
    {
        skill_state = Skill_state.None;
        rb = GetComponentInParent<Rigidbody>();
    }
    // ��ź ��ų.

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
        // ��ų ��Ÿ��
        CoolTimer.instance.on_Btn();
        CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

    }

    // Time.scale �����ϴ� �Һ���
    bool flag = false;
    // Update is called once per frame
    void Update()
    {     
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

                    // ��ų ��Ÿ��
                    CoolTimer.instance.cooltime = CoolTimer.CoolTime.skill_cooltime;

                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                    rb.AddForce(transform.up * 10, ForceMode.Impulse);
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
    }
    private void OnCollisionEnter(Collision collision)
    {
        // �뽬�Ҷ� ���ں� ƨ�ܳ����� �ڵ�
        if (collision.collider.name.Contains("Boco"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb);
            rb.AddForce(Vector3.up * 10 + -transform.forward * 15, ForceMode.Impulse);
            //rb.velocity=GMrb.velocity*2.1f;
            
        }
    }
}
