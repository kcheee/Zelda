using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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


    Rigidbody rb;

    public Transform firePosition;

    private void Start()
    {
        skill_state = Skill_state.None;
        rb = GetComponent<Rigidbody>();
        skill_state = Skill_state.None;
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


    }



    // Time.scale 조절하는 불변수
    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            // 스킬 창을 열었을 때 시간 느려짐
            if (!flag) Time.timeScale = 0.3f;
            // 밑에 아이스발판 기술을 사용할 때 시간 돌아오고 스킬 발동
            if (Input.GetKeyDown(KeyCode.P))
            {
                // 스킬 패널 false
                SkillUI.instance.Skillpanel.SetActive(false);
                Time.timeScale = 1f;    // 타임스케일 돌아옴
                flag = true;

                // 플레이어 스킬과 겹치는거 방지하기 위해 y값 살짝 올림.
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                // transform.up 힘
                rb.AddForce(transform.up * 8, ForceMode.Impulse);
                // 스킬 발동
                Instantiate(iceskill, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z)
                    , transform.rotation);
            }

            // 폭탄 던지기.
            if (Input.GetKeyDown(KeyCode.O))
            {
                Time.timeScale = 1f;
                flag = true;
                transform.eulerAngles = new Vector3(0, CameraRotation.eulerAngles.y, 0);
                // 스킬 패널 false
                SkillUI.instance.Skillpanel.SetActive(false);
                StartCoroutine(Bomb());
            }
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            SkillUI.instance.Skillpanel.SetActive(false);
            Time.timeScale = 1f; flag = false;

            if (!flag)
                Time.timeScale = 0.3f;

            #region 아이스 발판
            if (Input.GetKeyDown(KeyCode.P))
            {
                SkillUI.instance.Skillpanel.SetActive(false);
                flag = true; Time.timeScale = 1;

                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                rb.AddForce(transform.up * 8, ForceMode.Impulse);
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

        if (Input.GetKeyUp(KeyCode.K))
        {
            SkillUI.instance.Skillpanel.SetActive(false);
            flag = false; Time.timeScale = 1;

        }

    }
}