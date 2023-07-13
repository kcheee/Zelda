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

        //// 1.1 �Է°��� �����´�
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //// 1.2���ο� ������ ���Ѵ�
        //Vector3 dir = new Vector3(h, 0, v);
        //// 1.3ĳ������ ������ ī�޶��� �������� �Ѵ�
        //dir = camera.transform.TransformDirection(dir);
        //// 1.4y ���Ⱚ�� ������ �ʴ´�.
        //dir.y = 0;
        //dir.Normalize();

        //// 2.4. ������ y �ӵ��� dir �� �׸� �ݿ��Ǿ�� �Ѵ�.
        //Vector3 velocity = dir * speed;     // (h', 0, v')
        //       // (h', yVelocity, v')

        //// 1.5 �̵��Ѵ�.
        //// transform.position += velocity * speed * Time.deltaTime;
        //cc.Move(velocity * Time.deltaTime);
    }

    public void Ondamaged()
    {
        currentHP--;
        if(currentHP > 0)
        {
            print("����");
        }
        else if(currentHP < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
