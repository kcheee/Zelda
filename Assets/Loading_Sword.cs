using UnityEngine;
using UnityEngine.UI;

public class Loading_Sword : MonoBehaviour
{
    public Image dependentImage;
    public bool isImageActive = false;
    public float rotationSpeed = 10f;

    private RectTransform uiRectTransform;
    private bool isRotating = false;

    private void Start()
    {
        uiRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isImageActive && !isRotating)
        {
            StartRotation();
        }
        else if (!isImageActive && isRotating)
        {
            StopRotation();
        }

        if (isRotating)
        {
            RotateUIObject();
        }
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    private void RotateUIObject()
    {
        uiRectTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
