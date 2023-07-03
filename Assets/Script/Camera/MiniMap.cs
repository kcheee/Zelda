using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MiniMap : MonoBehaviour
{
    private void Start()
    {
        this.CreateMiniMapMesh();

    }
    void CreateMiniMapMesh()
    {
        NavMeshTriangulation meshData = NavMesh.CalculateTriangulation();

        Mesh mesh = new Mesh();
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.indices;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
