using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralPlane : MonoBehaviour
{
    [SerializeField] private float height = 1.0f;
    [SerializeField] private float width = 1.0f;
    [SerializeField] private int lines = 2;
    [SerializeField] private int columns = 2;

    private void OnEnable()
    {
        Mesh mesh = new Mesh { name = "Procedural Plane" };

        Vector3 offsetPos = new Vector3(-width / 2.0f, -height / 2.0f, 0.0f);
        Vector2 faceSize = new Vector3((1.0f / (columns - 1)) * width, (1.0f / (lines - 1)) * height, 0.0f);

        Vector3[] vertices = new Vector3[lines * columns];

        for (int y = 0; y < lines; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                vertices[x + y * columns] = new Vector3(faceSize.x * x, faceSize.y * y, 0.0f) + offsetPos;
            }
        }

        mesh.vertices = vertices;
        
        int[] triangles = new int[((lines - 1) * (columns - 1) * 2) * 3];

        for (int y = 0; y < (lines-1); y++)
        {
            for (int x = 0; x < (columns-1); x++)
            {
                triangles[(x + y * (columns - 1)) * 6 + 0] = (x + y * columns); // top left
                triangles[(x + y * (columns - 1)) * 6 + 1] = (x + y * columns) + columns; // bot left
                triangles[(x + y * (columns - 1)) * 6 + 2] = (x + y * columns) + 1; // top right
                triangles[(x + y * (columns - 1)) * 6 + 3] = (x + y * columns) + 1; // top right
                triangles[(x + y * (columns - 1)) * 6 + 4] = (x + y * columns) + columns; // bot left
                triangles[(x + y * (columns - 1)) * 6 + 5] = (x + y * columns) + columns + 1; // bot right
            }
        }

        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
