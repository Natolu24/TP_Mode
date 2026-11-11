using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSphere : MonoBehaviour
{
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private int meridian = 8;
    [SerializeField] private int parallel = 8;
    [SerializeField] private int truncated = 0;

    private void OnEnable()
    {
        Mesh mesh = new Mesh { name = "Procedural Sphere" };

        int gridWidth = meridian - Mathf.Max(truncated - 1, 0);

        int extraVertices = (truncated > 0) ? parallel : 0;
        int verticesNmb = gridWidth * parallel + extraVertices;
        Vector3[] vertices = new Vector3[verticesNmb + 2];

        for (int y = 0; y < parallel; y++)
        {
            float angleVertical = ((Mathf.PI) / (parallel + 1)) * (y + 1);
            float posY = radius * Mathf.Cos(angleVertical);
            // Vertices of grid
            for (int x = 0; x < gridWidth; x++)
            {
                float angleHorizontal = ((2 * Mathf.PI) / meridian) * x;
                float posX = radius * Mathf.Sin(angleVertical) * Mathf.Cos(angleHorizontal);
                float posZ = radius * Mathf.Sin(angleVertical) * Mathf.Sin(angleHorizontal);
                vertices[x + y * gridWidth] = new Vector3(posX, posY, posZ);
            }
            
            if (truncated <= 0) continue;
            // Vertices in the sphere for truncated parts
            vertices[verticesNmb - extraVertices + y] = new Vector3(0.0f, posY, 0.0f);
        }
        // Extra Vertices at each pole of sphere
        vertices[verticesNmb + 0] = new Vector3(0.0f, radius, 0.0f);
        vertices[verticesNmb + 1] = new Vector3(0.0f, -radius, 0.0f);

        mesh.vertices = vertices;

        int extraTriangles = (truncated > 0) ? parallel * 2 : 0;
        int trianglesNmb = gridWidth * (2 + (parallel - 1) * 2) + extraTriangles;
        int[] triangles = new int[trianglesNmb * 3];
        int indexTriangles = 0; // too tired of manually setting the indexes...

        for (int y = 0; y < (parallel - 1); y++)
        {
            for (int x = 0; x < (meridian - truncated); x++)
            {
                // Triangles of grid
                triangles[indexTriangles++] = (x + y * gridWidth); // top left
                triangles[indexTriangles++] = ((x + y * gridWidth) + 1) % gridWidth + y * gridWidth; // top right
                triangles[indexTriangles++] = (x + y * gridWidth) + gridWidth; // bot left

                triangles[indexTriangles++] = ((x + y * gridWidth) + 1) % gridWidth + y * gridWidth; // top right
                triangles[indexTriangles++] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot right
                triangles[indexTriangles++] = (x + y * gridWidth) + gridWidth; // bot left
                
                if (truncated <= 0) continue;
                
                // Inner triangles of truncated parts (in the grid)
                if (x == 0)
                {
                    triangles[indexTriangles++] = verticesNmb - extraVertices + y; // top inner
                    triangles[indexTriangles++] = (x + y * gridWidth); // top outside
                    triangles[indexTriangles++] = verticesNmb - extraVertices + y + 1; // bot inner

                    triangles[indexTriangles++] = (x + y * gridWidth); // top outside
                    triangles[indexTriangles++] = (x + y * gridWidth) + gridWidth; // bot outside
                    triangles[indexTriangles++] = verticesNmb - extraVertices + y + 1; // bot inner
                }
                if (x == (meridian - truncated - 1))
                {
                    triangles[indexTriangles++] = ((x + y * gridWidth) + 1) % gridWidth + y * gridWidth; // top outside
                    triangles[indexTriangles++] = verticesNmb - extraVertices + y; // top inner
                    triangles[indexTriangles++] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot outside

                    triangles[indexTriangles++] = verticesNmb - extraVertices + y; // top inner
                    triangles[indexTriangles++] = verticesNmb - extraVertices + y + 1; // bot inner
                    triangles[indexTriangles++] = ((x + y * gridWidth) + gridWidth + 1) % gridWidth + (y + 1) * gridWidth; // bot outside
                }
            }
        }

        // Triangles at each pole
        for (int x = 0; x < (meridian - truncated); x++)
        {
            triangles[indexTriangles++] = verticesNmb + 0; // north pole
            triangles[indexTriangles++] = (x + 1) % gridWidth; // top right corner
            triangles[indexTriangles++] = x; // top left corner

            triangles[indexTriangles++] = verticesNmb + 1; // south pole
            triangles[indexTriangles++] = (gridWidth * parallel - gridWidth) + x; // bot left corner
            triangles[indexTriangles++] = (gridWidth * parallel - gridWidth) + ((x + 1) % gridWidth); // bot right corner
            
            if (truncated <= 0) continue;
            
            // Inner triangles of truncated parts (touching the poles)
            if (x == 0)
            {
                triangles[indexTriangles++] = verticesNmb + 0; // north pole
                triangles[indexTriangles++] = x; // top outside
                triangles[indexTriangles++] = verticesNmb - extraVertices + 0; // top inner

                triangles[indexTriangles++] = verticesNmb - extraVertices + (extraVertices - 1); // bot inner
                triangles[indexTriangles++] = (gridWidth * parallel - gridWidth) + x; // bot outside
                triangles[indexTriangles++] = verticesNmb + 1; // south pole
            }
            if (x == (meridian - truncated - 1))
            {
                triangles[indexTriangles++] = verticesNmb + 0; // north pole
                triangles[indexTriangles++] = verticesNmb - extraVertices + 0; // top inner
                triangles[indexTriangles++] = (x + 1) % gridWidth; // top outside

                triangles[indexTriangles++] = verticesNmb - extraVertices + (extraVertices - 1); // bot inner
                triangles[indexTriangles++] = verticesNmb + 1; // south pole
                triangles[indexTriangles++] = (gridWidth * parallel - gridWidth) + ((x + 1) % gridWidth); // bot outside
            }
        }

        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}