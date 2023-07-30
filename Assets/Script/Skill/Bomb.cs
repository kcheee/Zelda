using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Bomb : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody[] rbs;
    public GameObject Bomb_Explosion_Effect;
    public AudioSource BombSFX;
    //public AudioClip BombSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            BombSFX.PlayOneShot(BombSFX.clip);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        Destroy(gameObject);

        //Instatiate(BombSFX.PlayOneShot(BombSFX.clip);
        Instantiate(Bomb_Explosion_Effect, collision.contacts[0].point, Quaternion.identity);
        // ±¸ ¹Ý°æÀ¸·Î À§Ä¡¸¦ °¡Á®¿È.
        Collider[] cols = Physics.OverlapSphere(collision.contacts[0].point, 20);

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
                    rb.AddExplosionForce(15 * rb.mass, collision.contacts[0].point, 20, 15 * rb.mass, ForceMode.Impulse);
                }

                // ÆøÅº µ¥¹ÌÁö
                RagdollBokoblin.Damage = 5;
                cols[i].GetComponentInParent<RagdollBokoblin>().DamagedProcess();
            }
        }
       


    }
}
