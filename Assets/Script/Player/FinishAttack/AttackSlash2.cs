using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlash2 : MonoBehaviour
{
    public Transform target;


    private void OnEnable()
    {
        Collider[] cols = Physics.OverlapSphere(target.transform.position, 30);

        for (int i = 0; i < cols.Length; i++)
        {
            // ÆøÅº ¹Ý°æ¿¡ ÀÖ´Â ¿ÀºêÁ§Æ® rigidbody °¡Á®¿È
            if (cols[i].CompareTag("Bokoblin"))
            {

                Rigidbody[] rigid = cols[i].GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigid)
                {
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = new Vector3(0, 0, 0);
                    rb.AddExplosionForce(80 * rb.mass, target.transform.position, 30, 15 * rb.mass, ForceMode.Impulse);
                }

                // ÆøÅº µ¥¹ÌÁö
                RagdollBokoblin.Damage = 10;
                cols[i].GetComponentInParent<RagdollBokoblin>().state = RagdollBokoblin.BocoblinState.Damaged;

            }
        }
    }
}
