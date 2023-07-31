using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{
    public AudioSource Icesound;
    public AudioClip IceMaking;
    private void OnEnable()
    {
        Icesound.clip = IceMaking;
        PlayIceMakingSound();
        // ½ºÅ³ ÄðÅ¸ÀÓ UI On
        CoolTimer.instance.on_Btn();
    }
    bool flag = false;
    IEnumerator delay()
    {
        //Icesound.clip = IceDestroy;
        //Icesound.Play();
        yield return new WaitForSeconds(4);
        SkillManager.flag_icemaker = false;
        Destroy(gameObject);
    }

    void Update()
    {
        if (transform.localScale.magnitude < 8f)
        {
            transform.localScale += new Vector3(0, 1.5f, 0) * Time.deltaTime * 20;
        }
        else if(!flag)
        {
            flag=true;
            SkillManager.instance.GetComponent<Rigidbody>().AddForce(Vector3.up* 18*20 , ForceMode.Impulse);
            StartCoroutine(delay());
        }
        else
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapBox(collision.transform.position, new Vector3(6f, 7, 6f));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("Bokoblin"))
            {
                Rigidbody[] rigid = cols[i].GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigid)
                {
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = new Vector3(0, 0, 0);
                    //rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
                    rb.AddExplosionForce(5 * rb.mass, collision.contacts[0].point, 20, 10 * rb.mass, ForceMode.Impulse);
                }

                // ÆøÅº µ¥¹ÌÁö
                RagdollBokoblin.Damage = 3;
                cols[i].GetComponentInParent<RagdollBokoblin>().DamagedProcess();

            }

        }
    }
    private void OnDisable()
    {
        Psoundscript.icetrigger = true;
    }
    private void PlayIceMakingSound()
    {
        Icesound.PlayOneShot(IceMaking);
    }

}
