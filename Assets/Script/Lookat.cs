using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       
        transform.LookAt(player.transform.position);
        Vector3 dir = transform.eulerAngles;

        transform.eulerAngles = new Vector3(0,dir.y,0);
       
    }
}
