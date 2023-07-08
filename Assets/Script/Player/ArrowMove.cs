using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public float speed = 15;
    Rigidbody rb;
    public GameObject effect;

    void Start()
    {
        this.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    void Update()
    {
        transform.forward = rb.velocity.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject arrowEffect = Instantiate(effect, collision.transform.position, collision.transform.rotation);
        //otherRB.AddForce(transform.forward * otherRB.mass * 5, ForceMode.Impulse);
        Destroy(arrowEffect, 0.5f);

        this.gameObject.SetActive(false);
    }
}
