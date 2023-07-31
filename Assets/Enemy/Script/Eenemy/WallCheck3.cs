using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck3 : MonoBehaviour
{
    public GameObject enemySpotOn;
    public GameObject enemySpotOff;

    public GameObject[] bokos1;
    public GameObject[] bokos2;
    public GameObject[] bokos3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Link")
        {
            enemySpotOn.SetActive(true);
            Destroy(enemySpotOff.gameObject);

            for (int i = 0; i < bokos1.Length; i++)
            {
                bokos1[i].gameObject.GetComponent<RagdollBokoblin>().detectDistance = 20;
                bokos1[i].gameObject.GetComponent<RagdollBokoblin>().attackPossibleDistance = 12;
            }
            for (int j = 0; j < bokos2.Length; j++)
            {
                bokos1[j].gameObject.GetComponent<RagdollBokoblin>().detectDistance = 25;
                bokos1[j].gameObject.GetComponent<RagdollBokoblin>().attackPossibleDistance = 17;
            }
            for (int k = 0; k < bokos3.Length; k++)
            {
                bokos1[k].gameObject.GetComponent<RagdollBokoblin>().detectDistance = 50;
                bokos1[k].gameObject.GetComponent<RagdollBokoblin>().attackPossibleDistance = 39;
            }
        }
    }
}
