using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int currentHP;
    public int maxHP;
    public GameObject bulletFactory;
    public Transform firePos;
    Camera camera;
    float speed = 10;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        camera = Camera.main;
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePos.transform.position;
        }

        //// 1.1 입력값을 가져온다
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //// 1.2새로운 방향을 정한다
        //Vector3 dir = new Vector3(h, 0, v);
        //// 1.3캐릭터의 방향을 카메라의 방향으로 한다
        //dir = camera.transform.TransformDirection(dir);
        //// 1.4y 방향값은 변하지 않는다.
        //dir.y = 0;
        //dir.Normalize();

        //// 2.4. 결정된 y 속도가 dir 의 항목에 반영되어야 한다.
        //Vector3 velocity = dir * speed;     // (h', 0, v')
        //       // (h', yVelocity, v')

        //// 1.5 이동한다.
        //// transform.position += velocity * speed * Time.deltaTime;
        //cc.Move(velocity * Time.deltaTime);
    }

    public void Ondamaged()
    {
        currentHP--;
        if(currentHP > 0)
        {
            print("아파");
        }
        else if(currentHP < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
