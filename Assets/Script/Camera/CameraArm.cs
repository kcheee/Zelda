using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraArm : MonoBehaviour
{
   public GameObject player;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, player.transform.localPosition.y, transform.position.z);
    }
}
