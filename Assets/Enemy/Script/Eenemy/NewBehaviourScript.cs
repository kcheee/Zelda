using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector3 rootWorldPosition;
    Animator anim;
    Rigidbody[] rbs;
    Transform hipBone;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rbs = GetComponentsInChildren<Rigidbody>();

        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);
    }

    // Update is called once per frame
    void Update()
    {
        rootWorldPosition = hipBone.position;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.enabled = false;
            foreach (Rigidbody rb in rbs)
            {
                rb.AddForce(Vector3.up * 7, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.enabled = true;


            transform.position = new Vector3(hipBone.position.x, transform.position.y, hipBone.position.z);

            anim.SetTrigger("Wakeup");


        }
    }
}