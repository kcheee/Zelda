using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {      
        // 콜라이더를 담을 수 있는 배열을 만든다.
        Collider[] colls;

        // 반경 10f에 위치한 오브젝트들을 배열에 담는다
        colls = Physics.OverlapSphere(transform.position, 20);
   
        // foreach문을 통해서 colls배열에 존재하는 각각에 폭발효과를 적용해준다.
        foreach (Collider coll in colls)
        {
            
            // 조건문을 사용해서 특정레이어에 속한 오브젝트에만 영향을 줄 수 있다.(ex-플레이어만 날아가도록)
            if (coll.name.Contains("Boco"))
            { 
                // 해당 오브젝트의 Rigidbody를 가져와서 AddExplosionForce 함수를 사용해준다.
                // AddExplosionForce(폭발력, 폭발위치, 반경, 위로 솟구쳐올리는 힘)
                coll.GetComponent<Rigidbody>().AddExplosionForce(300, transform.position, 10, 50);
            }
            // 코드 정리.
            // 검출된 오브젝트들 중에서 8번 레이어에 속한 오브젝트 각각을,
            // 폭발위치(coll오브젝트위치아님)기준으로 반경 20f까지 100f의 폭발력과 20f의 상향력으로 날려버린다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
