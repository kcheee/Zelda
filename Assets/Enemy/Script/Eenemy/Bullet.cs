using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    Rigidbody rb;
    public GameObject link;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;    // Time.deltaTime �� �ڵ����� �� ����
    }

    
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹 �浹
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ������Ʈ�� ������ٵ� ������Ʈ�� �޾ƿ´�.
        var otherRB = collision.gameObject.GetComponent<Rigidbody>();   // var �� ���� �ٷ� �ڷ����� ã�Ƽ� ��ȯ���ִ� ���Ǳ��

        // ���࿡ �ε��� ��ü�� Rigidbody �� �ִٸ�
        if (otherRB)
        {
           
            // �� �չ������� ���� 10 ���ϰ� �ʹ�.
            otherRB.AddForce(link.transform.forward * 5 + transform.up * 8, ForceMode.Impulse);   // Forcemode.~~ : ���� �ִ� ���
        }


        if (collision.gameObject.name.Contains("Boco"))
        {
            // Unity_B.instance.state = Unity_B.UnityState.Damaged;
            collision.gameObject.GetComponent<Bocoblin1>().state = Bocoblin1.BocoblinState.Damaged;
        }
        else if (collision.gameObject.name.Contains("Moblin"))
        {
            collision.gameObject.GetComponent<Moblin>().state = Moblin.MoblinState.Damaged;
        }
        // �ҷ��� �ı��Ѵ�.
         Destroy(this.gameObject);
    }
}
