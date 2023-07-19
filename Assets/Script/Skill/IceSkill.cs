using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{

    private void OnEnable()
    {
        // ��ų ��Ÿ�� UI On
        CoolTimer.instance.on_Btn();
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(4);
        SkillManager.flag_icemaker = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude < 8f)
        {
            transform.localScale += new Vector3(0, 1.5f, 0) * Time.deltaTime * 20;
        }
        else
        {         
             StartCoroutine(delay());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapBox(collision.transform.position, new Vector3(6f,7,6f));
       
        for (int i = 0; i < cols.Length; i++)
        {
            // ��ź �ݰ濡 �ִ� ������Ʈ rigidbody ������
            if (cols[i].name.Contains("Boko_collider"))
            {
                Rigidbody rigid = cols[i].GetComponent<Rigidbody>();
                // (������ ��, ������ ��ġ�� ���� �߽�, ������ ��ġ�� ���� �ݰ�, ���� �ڱ�ġ�� ��)
                rigid.AddForce(collision.transform.forward*10,ForceMode.Impulse);
                rigid.AddExplosionForce(300, this.transform.position, 10, 30);

            }
        }
    }
}
