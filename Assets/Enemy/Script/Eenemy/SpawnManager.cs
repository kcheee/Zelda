using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // ���ں� ����
    public GameObject wave;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.BossGage.GetComponent<Slider>().value <= 30)
        {
            wave.SetActive(true);
        } 
    }
}
