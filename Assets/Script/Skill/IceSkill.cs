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
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude < 8f)
        {
            transform.localScale += new Vector3(0, 1.5f, 0) * Time.deltaTime * 10;
        }
        else
        {
        StartCoroutine(delay());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapBox(collision.transform.position, new Vector3(3.2f,4,3.2f));
        Debug.Log(collision.gameObject);
        for (int i = 0; i < cols.Length; i++)
        {
            // ��ź �ݰ濡 �ִ� ������Ʈ rigidbody ������
            if (cols[i].name.Contains("Boco"))
            {
                Rigidbody rigid = cols[i].GetComponent<Rigidbody>();
                // (������ ��, ������ ��ġ�� ���� �߽�, ������ ��ġ�� ���� �ݰ�, ���� �ڱ�ġ�� ��)
                rigid.AddForce(collision.transform.forward*10,ForceMode.Impulse);
                rigid.AddExplosionForce(300, this.transform.position, 10, 30);

            }
        }
    }
}
