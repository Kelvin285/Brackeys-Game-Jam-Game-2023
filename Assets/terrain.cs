using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.U2D;

public class terrain : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    private Mesh mesh;

    private byte[] blocks = new byte[256 * 1024];

    void Start()
    {
        mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();


        int index = 0;
        for (int x = 0; x < 256; x++)
        {
            Debug.Log("X: " + x);
            for (int y = 0; y < 1024; y++)
            {
                float density = Mathf.PerlinNoise(x * 0.25f * 0.5f, y * 0.25f * 0.5f) * 2 - 1;
                float dist = 1.0f - Mathf.Abs(x - 128) / 128.0f;
                dist = Mathf.Pow(dist, 4.0f);
                density += dist;
                if (density < 0)
                {
                    blocks[x + y * 256] = 1;

                    vertices.Add(new Vector3(x, y));
                    vertices.Add(new Vector3(x, y + 1));
                    vertices.Add(new Vector3(x + 1, y + 1));
                    vertices.Add(new Vector3(x + 1, y));
                    indices.Add(index);
                    indices.Add(index + 1);
                    indices.Add(index + 2);
                    indices.Add(index + 2);
                    indices.Add(index + 3);
                    indices.Add(index);
                    index += 4;
                }
            }
        }

        for (int x = 1; x < 255; x++)
        {
            for (int y = 1; y < 1023; y++)
            {
                if (blocks[x + y * 256] == 1)
                {
                    if (blocks[x + (y + 1) * 256] == 0 || blocks[x + (y - 1) * 256] == 0 ||
                        blocks[(x + 1) + (y) * 256] == 0 || blocks[(x - 1) + (y) * 256] == 0)
                    {
                        var collider = gameObject.AddComponent<BoxCollider2D>();
                        collider.offset = new Vector2(x + 0.5f, y + 0.5f);
                    }
                }
            }
        }

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        meshFilter.mesh = mesh;

    }
    void Update()
    {
        
    }
}
