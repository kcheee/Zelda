using UnityEngine;

public class Bomb_Rotate : MonoBehaviour
{

    Rigidbody rigidbody;
    public float torqueForce = 2;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ApplyRotation();
    }
    void ApplyRotation()
    {
        // 무작위한 회전력을 생성하여 야구공에 적용
        rigidbody.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
    }
}