using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck2 : MonoBehaviour
{
    public GameObject enemySpotOn;
    public GameObject enemySpotOff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Link")
        {
            enemySpotOn.SetActive(true);
            Destroy(enemySpotOff.gameObject);
        }
    }
}
