using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; // ī�޶� Ʈ������
    public float shakeDuration = 0.1f; // ����ũ ���� �ð�
    public float shakeMagnitude = 0.5f; // ����ũ ����

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originalPosition = cameraTransform.localPosition; // ���� ��ġ ����

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // ������ �������� ����ũ ȿ�� ����   // Random.indisdeUnitSphere ��� ����.
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // ����ũ�� ������ ���� ��ġ�� ���ƿ�
        cameraTransform.localPosition = originalPosition;
    }
}