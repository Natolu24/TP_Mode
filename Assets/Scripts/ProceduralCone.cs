using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCone : MonoBehaviour
{
    [SerializeField] private bool isTruncated;

    private void OnEnable()
    {
        Mesh mesh = new Mesh { name = "Procedural Plane" };

        mesh.vertices = new Vector3[]
        {
            Vector3.zero, Vector3.right, Vector3.up, Vector3.right + Vector3.up
        };

        mesh.triangles = new int[]
        {
            0, 2, 1,
            1, 2, 3,
        };

        GetComponent<MeshFilter>().mesh = mesh;
    }
}