using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolDamageManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.name);
        if (other.gameObject.layer == 17)
        {
            GetComponentInParent<Molblin1>().state = Molblin1.MolblinState.Damaged;
        }
    }
}