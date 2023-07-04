using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude < 6f)
        {
            transform.localScale += new Vector3(0, 2f, 0) * Time.deltaTime * 10;
        }
        else
        {
        StartCoroutine(delay());
        }
    }
}
