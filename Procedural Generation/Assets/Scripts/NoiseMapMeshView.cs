using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapMeshView : MonoBehaviour {

    /// <summary>
    /// Access to Mesh Data
    /// </summary>
    public MeshFilter meshfilter;

    /// <summary>
    /// Vertices of the New Mesh
    /// </summary>
    private Vector3[] vertices;

    /// <summary>
    /// Triangles of the new Mesh
    /// </summary>
    private int[] triangles;

    /// <summary>
    /// UVS of the new Mesh
    /// </summary>
    //private Vector2[] uvs;

    /// <summary>
    /// Colors of the new Mesh
    /// </summary>
    private Color[] colors;

    /// <summary>
    /// Renders the Mesh onto the Scene
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <param name="maxHeight">The highest Mountain</param>
    /// <param name="gradient">Depending on the Height[0-1] a certain Color is chosen from the Gradient</param>
    /// <param name="exponent">Exponent of the Curve (Exponential Function: f(x) = x^exp)</param>
    public void DrawMesh(float[,] heightmap, float maxHeight, Gradient gradient, float exponent)
    {
        meshfilter.mesh = CreateMesh(heightmap, maxHeight, gradient, exponent);
    }

    /// <summary>
    /// Creates a Mesh
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <param name="maxHeight">The highest Mountain</param>
    /// <param name="gradient">Depending on the Height[0-1] a certain Color is chosen from the Gradient</param>
    /// <param name="exponent">Exponent of the Curve (Exponential Function: f(x) = x^exp)</param>
    /// <returns>A Mesh having the vertices at the given height from heightmap * maxHeight</returns>
    private Mesh CreateMesh(float[,] heightmap, float maxHeight, Gradient gradient, float exponent)
    {
        Mesh mesh = new Mesh();
        int width = heightmap.GetLength(0);
        int length = heightmap.GetLength(1);

        //Create Vertices
        vertices = new Vector3[width * length];
        //uvs = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];
        
        for (int i = 0, y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float height = Mathf.Pow(heightmap[x, y], exponent) * maxHeight;
                vertices[i] = new Vector3(x, height, y);
                //uvs[i] = new Vector2((float)x / width, (float)y / length);
                colors[i] = gradient.Evaluate(heightmap[x, y]);
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

        //Set Values onto mesh
        UpdateMesh(mesh);
        return mesh;
    }

    /// <summary>
    /// Sets Vertices, Triangles, Colors onto a Mesh
    /// </summary>
    /// <param name="mesh">The Mesh to be updated</param>
	private void UpdateMesh (Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        //mesh.uv = uvs;       
        mesh.RecalculateNormals();
	}
}