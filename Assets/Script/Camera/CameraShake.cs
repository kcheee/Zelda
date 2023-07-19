using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; // 카메라 트랜스폼
    public float shakeDuration = 0.1f; // 쉐이크 지속 시간
    public float shakeMagnitude = 0.5f; // 쉐이크 강도

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originalPosition = cameraTransform.localPosition; // 원래 위치 저장

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // 랜덤한 방향으로 쉐이크 효과 적용   // Random.indisdeUnitSphere 사용 가능.
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 쉐이크가 끝나면 원래 위치로 돌아옴
        cameraTransform.localPosition = originalPosition;
    }
}