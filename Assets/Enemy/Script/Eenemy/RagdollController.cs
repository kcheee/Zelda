using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
    }

    public void OnRagdoll()
    {
        rb.isKinematic = false;
        GetComponent<RagdollBocoblin>().state = RagdollBocoblin.BocoblinState.Damaged;
    }

    public void OffRagdoll()
    {
        rb.isKinematic = false;
    }
}
