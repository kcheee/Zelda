using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class OverlapSphereTest : MonoBehaviour
{
    public float radius = 0.1f;
    public Material detectedMat;
    Rigidbody RB;
    bool rbflag = false;
    bool flag = false;  
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
    }
    // ���� �ٲ�.
    void changeMaterial(GameObject go, Material changeMat)
    {
        if(go == null) return;
        Renderer rd = go.GetComponent<MeshRenderer>();
        Material[] mat = rd.materials;
        mat[0] = changeMat;
        rd.materials = mat;
    }

    // ���信���� ���� ����
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    IEnumerator dashattack()
    {
        while (flag)
        {
            transform.position += transform.up * 0.2f;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    public GameObject link;

    void Update()
    {

        Vector3 dir = new Vector3(link.transform.position.x, 0, link.transform.position.z);

        // �� ������ �ٶ󺻴�.
        Debug.Log(dir.ToString());
        transform.LookAt(dir);
        if (Input.GetKeyDown(KeyCode.C))
        {
            flag = true;
            rbflag = true;
            StartCoroutine(dashattack());
        }
        if (Input.GetKey(KeyCode.C))
        {

            collision_check();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            flag = false;
            ResetParent();
        }
    }

        // �ݶ��̴� üũ // Dash���.
        Collider[] collision_check()
        {
            // ���߿��� layermask ��߉�
            Collider[] colliders =
                Physics.OverlapSphere(this.transform.position, radius);

            foreach (Collider col in colliders) // ����� ������ŭ �ݶ��̴� ������.
            {
                if (col.name == "Sphere" || col.name == "Plane"/* �ڱ� �ڽ��� ���� */) continue;
                Rigidbody rb = col.GetComponent<Rigidbody>();

                rb.AddForce(rb.transform.up * 4, ForceMode.Force);

                if (rb != null)
                {
                    // �ε��� ������Ʈ�� �÷��̾��� �ڽ����� ����
                    col.transform.SetParent(transform);
                }

                changeMaterial(col.gameObject, detectedMat);
            }
            return colliders;
        }
        void ResetParent()
        {

        List<Transform> objectsToRelease = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.name == "Main Camera"/* �ڱ� �ڽ��� ���� */) continue;

            objectsToRelease.Add(child);
        }
        foreach (Transform child in objectsToRelease)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * 5, ForceMode.Impulse);
            child.SetParent(null);
        }
    }
}
