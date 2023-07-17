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
        rb.velocity = transform.forward * speed;    // Time.deltaTime 이 자동으로 들어가 있음
    }

    
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌 충돌
    private void OnCollisionEnter(Collision collision)
    {
        // 게임 오브젝트의 리지드바디 컴포넌트를 받아온다.
        var otherRB = collision.gameObject.GetComponent<Rigidbody>();   // var 를 쓰면 바로 자료형을 찾아서 반환해주는 편의기능

        // 만약에 부딪힌 물체에 Rigidbody 가 있다면
        if (otherRB)
        {
           
            // 내 앞방향으로 힘을 10 가하고 싶다.
            otherRB.AddForce(link.transform.forward * 5 + transform.up * 8, ForceMode.Impulse);   // Forcemode.~~ : 힘을 주는 방법
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
        // 불렛을 파괴한다.
         Destroy(this.gameObject);
    }
}
