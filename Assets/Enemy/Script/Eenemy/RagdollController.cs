using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    Rigidbody[] rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].isKinematic = true;
        }
    }

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.B))
        {
            anim.enabled = false;
            for (int i = 0; i < rb.Length; i++)
            {
                rb[i].isKinematic = false;
                Debug.Log(rb[i].isKinematic);
            }
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            for (int i = 0; i < rb.Length; i++)
            {
                rb[i].isKinematic = true;
                Debug.Log(rb[i].isKinematic);
            }
            anim.enabled = true;
            anim.SetTrigger("StacdUp");
        }
    }

}
