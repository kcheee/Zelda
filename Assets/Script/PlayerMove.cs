using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.right*h+transform.forward*v;
        dir.Normalize();
        dir.y = 0;
        dir = cam.transform.TransformDirection(dir);

        transform.position += dir*10*Time.deltaTime;
    }
}
