using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter filter;
    public MeshRenderer renderer;

    float time = 0;
    void Start()
    {
        mesh = new Mesh();

        filter.mesh = mesh;
    }

    void Update()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        float m = 0.5f;
        int index = 0;
        for (int i = -512; i < 512; i++)
        {
            vertices.Add(new Vector3(i, -4096, 0));
            vertices.Add(new Vector3(i, Mathf.PerlinNoise1D((i + time) * m), 0));
            vertices.Add(new Vector3(i + 1, Mathf.PerlinNoise1D((i + 1 + time) * m), 0));
            vertices.Add(new Vector3(i + 1, -4096, 0));
            indices.Add(index);
            indices.Add(index + 1);
            indices.Add(index + 2);
            indices.Add(index + 2);
            indices.Add(index + 3);
            indices.Add(index + 0);
            index += 4;
        }
        time += Time.deltaTime;

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
    }
}
