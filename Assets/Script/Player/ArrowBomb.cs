using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {      
        // �ݶ��̴��� ���� �� �ִ� �迭�� �����.

        // �ݰ� 10f�� ��ġ�� ������Ʈ���� �迭�� ��´�
        Collider[] cols = Physics.OverlapSphere(transform.position, 8);

        // foreach���� ���ؼ� colls�迭�� �����ϴ� ������ ����ȿ���� �������ش�.
        foreach (Collider coll in cols)
        {
            //Debug.Log(coll);
            // ���ǹ��� ����ؼ� Ư�����̾ ���� ������Ʈ���� ������ �� �� �ִ�.(ex-�÷��̾ ���ư�����)
            if (coll.CompareTag("Bokoblin"))
            {
                Rigidbody[] rigid = coll.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigid)
                {
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = new Vector3(0, 0, 0);
                    rb.AddExplosionForce(3 * rb.mass, transform.position, 10, 8 * rb.mass, ForceMode.Impulse);
                }

                // ��ź ������
                RagdollBokoblin.Damage = 2;
                coll.GetComponentInParent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;

            }
            if (coll.CompareTag("Moblin"))
            {
                Molblin1.instance.UpdateDamaged();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
