using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapMeshView : MonoBehaviour {

    public MeshFilter meshfilter;

    Vector3[] vertices;
    int[] triangles;

	// Use this for initialization
	void Start () {
        float[,] heightmap = new float[256, 256]; //To Test
        CreateMesh(heightmap);
        UpdateMesh(meshfilter.mesh);
    }

    private void CreateMesh(float[,] heightmap)
    {
        int width = heightmap.GetLength(0);
        int length = heightmap.GetLength(1);

        //Create Vertices
        vertices = new Vector3[width * length];
        for (int i = 0, y = 0; y < length; y++)
        {
            
            for (int x = 0; x < width; x++)
            {
                vertices[i] = new Vector3(x, 0, y);
                i++;
            }
        }
        
        //Create Triangles
        triangles = new int[(width - 1) * (length - 1) * 6];
        int verticeCount = 0;
        int triangleCount = 0;
        for (int y = 0; y < width - 1; y++)
        {           
            for (int x = 0; x < length - 1; x++)
            {
                //First Triangle
                triangles[triangleCount + 0] = verticeCount + 0;
                triangles[triangleCount + 1] = verticeCount + width;
                triangles[triangleCount + 2] = verticeCount + 1;
                //Other Side of Triangle
                triangles[triangleCount + 3] = verticeCount + 1;
                triangles[triangleCount + 4] = verticeCount + width;
                triangles[triangleCount + 5] = verticeCount + width + 1;

                verticeCount++;
                triangleCount += 6;
            }
            verticeCount++;
        }
    }

	private void UpdateMesh (Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
	}
}