using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {      
        // �ݶ��̴��� ���� �� �ִ� �迭�� �����.
        Collider[] colls;

        // �ݰ� 10f�� ��ġ�� ������Ʈ���� �迭�� ��´�
        colls = Physics.OverlapSphere(transform.position, 20);
   
        // foreach���� ���ؼ� colls�迭�� �����ϴ� ������ ����ȿ���� �������ش�.
        foreach (Collider coll in colls)
        {
            
            // ���ǹ��� ����ؼ� Ư�����̾ ���� ������Ʈ���� ������ �� �� �ִ�.(ex-�÷��̾ ���ư�����)
            if (coll.name.Contains("Boco"))
            { 
                // �ش� ������Ʈ�� Rigidbody�� �����ͼ� AddExplosionForce �Լ��� ������ش�.
                // AddExplosionForce(���߷�, ������ġ, �ݰ�, ���� �ڱ��Ŀø��� ��)
                coll.GetComponent<Rigidbody>().AddExplosionForce(300, transform.position, 10, 50);
            }
            // �ڵ� ����.
            // ����� ������Ʈ�� �߿��� 8�� ���̾ ���� ������Ʈ ������,
            // ������ġ(coll������Ʈ��ġ�ƴ�)�������� �ݰ� 20f���� 100f�� ���߷°� 20f�� ��������� ����������.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
