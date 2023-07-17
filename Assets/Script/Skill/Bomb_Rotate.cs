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
        // �������� ȸ������ �����Ͽ� �߱����� ����
        rigidbody.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
    }
}