using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverAnimation : MonoBehaviour
{
    public GameObject imageObject;
    public float rotationSpeed = 100f;

    private Button button;
    private bool isHovering = false;

    private void Start()
    {
        button = imageObject.GetComponent<Button>();
        imageObject.SetActive(false);
    }

    private void Update()
    {
        if (isHovering)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMouseEnterButton()
    {
        imageObject.SetActive(true);
        isHovering = true;
    }

    public void OnMouseExitButton()
    {
        imageObject.SetActive(false);
        isHovering = false;
    }
}
