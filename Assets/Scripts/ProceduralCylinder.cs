using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCylinder : MonoBehaviour
{
    [SerializeField] private float height = 2.0f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private int meridian = 8;
    [SerializeField] private int truncated = 0;

    private void OnEnable()
    {
        Mesh mesh = new Mesh { name = "Procedural Cylinder" };

        int gridWidth = meridian - Mathf.Max(truncated - 1, 0);

        int verticesNmb = gridWidth * 2;
        Vector3[] vertices = new Vector3[verticesNmb + 2];

        for (int y = 0; y < 2; y++)
        {
            float posY = height / 2.0f + (-height * y);
            // Vertices of grid
            for (int x = 0; x < gridWidth; x++)
            {
                float angle = ((2 * Mathf.PI) / meridian) * x;
                float posX = radius * Mathf.Cos(angle);
                float posZ = radius * Mathf.Sin(angle);
                vertices[x + y * gridWidth] = new Vector3(posX, posY, posZ);
            }
            // Extra Vertices at middle of each opposite face
            vertices[verticesNmb + y] = new Vector3(0.0f, posY, 0.0f);
        }

        mesh.vertices = vertices;

        int trianglesNmb = gridWidth * 4;
        int[] triangles = new int[trianglesNmb * 3];

        for (int y = 0; y < 1; y++)
        {
            for (int x = 0; x < (meridian - truncated); x++)
            {
                // Triangles of grid
                triangles[(x + y * gridWidth) * 4 * 3 + 0] = (x + y * gridWidth); // top left
                triangles[(x + y * gridWidth) * 4 * 3 + 1] = ((x + y * gridWidth) + 1 ) % gridWidth + y * gridWidth; // top right
                triangles[(x + y * gridWidth) * 4 * 3 + 2] = (x + y * gridWidth) + gridWidth; // bot left
                triangles[(x + y * gridWidth) * 4 * 3 + 3] = ((x + y * gridWidth) + 1 ) % gridWidth + y * gridWidth; // top right
                triangles[(x + y * gridWidth) * 4 * 3 + 4] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot right
                triangles[(x + y * gridWidth) * 4 * 3 + 5] = (x + y * gridWidth) + gridWidth; // bot left
                // Triangles of the opposite faces
                triangles[(x + y * gridWidth) * 4 * 3 + 6] = verticesNmb + 0; // up middle
                triangles[(x + y * gridWidth) * 4 * 3 + 7] = ((x + y * gridWidth) + 1) % gridWidth + y * gridWidth; // top right
                triangles[(x + y * gridWidth) * 4 * 3 + 8] = (x + y * gridWidth); // top left
                triangles[(x + y * gridWidth) * 4 * 3 + 9] = verticesNmb + 1; // down middle
                triangles[(x + y * gridWidth) * 4 * 3 + 10] = (x + y * gridWidth) + gridWidth; // bot left
                triangles[(x + y * gridWidth) * 4 * 3 + 11] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot right
                if (truncated <= 0) continue;
                // Inner triangles of truncated parts
                if (x == 0)
                {
                    triangles[trianglesNmb * 3 - 12] = verticesNmb + 0; // up middle
                    triangles[trianglesNmb * 3 - 11] = (x + y * gridWidth); // top left
                    triangles[trianglesNmb * 3 - 10] = verticesNmb + 1; // down middle
                    triangles[trianglesNmb * 3 - 9] = (x + y * gridWidth); // top left
                    triangles[trianglesNmb * 3 - 8] = (x + y * gridWidth) + gridWidth; // bot left
                    triangles[trianglesNmb * 3 - 7] = verticesNmb + 1; // down middle
                }
                if (x == (meridian - truncated - 1))
                {
                    triangles[trianglesNmb * 3 - 6] = ((x + y * gridWidth) + 1) % gridWidth + y * gridWidth; // top right
                    triangles[trianglesNmb * 3 - 5] = verticesNmb + 0; // up middle
                    triangles[trianglesNmb * 3 - 4] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot right
                    triangles[trianglesNmb * 3 - 3] = verticesNmb + 0; // up middle
                    triangles[trianglesNmb * 3 - 2] = verticesNmb + 1; // down middle
                    triangles[trianglesNmb * 3 - 1] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot right
                }
            }
        }

        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}