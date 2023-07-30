using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;
    GameObject link;

    // Start is called before the first frame update
    void Start()
    {
        link = GameObject.Find("Link");
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.forward = Camera.main.transform.forward;
        text.transform.forward = Camera.main.transform.forward;
    }
}
