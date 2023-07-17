using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Rigidbody GMrb; 
 
    // Start is called before the first frame update
    void Start()
    {
        GMrb = gameObject.GetComponent<Rigidbody>();
    }
    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.forward * 5 * Time.deltaTime;
            //transform.position += Vector3.forward * 10;
            flag = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            flag = false;

        }

    }
    public float ForceUp;
    public float ForceForward;

    IEnumerator dashattack(Collision collision)
    {
        while (flag)
        {
            collision.transform.Translate(transform.forward);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject);
    //    if (collision.collider.name.Contains("Capsule"))
    //    {
    //        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
    //        StartCoroutine(dashattack(collision));
    //        //rb.AddForce(Vector3.up * ForceUp+ Vector3.forward * ForceForward, ForceMode.Impulse);
    //        //rb.velocity=GMrb.velocity*2.1f;
    //        //rb.AddForce(GMrb.velocity* ForceForward + Vector3.up*ForceUp, ForceMode.Force);

    //    }
    //}

}
