using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHeightmap : MonoBehaviour {

    public Terrain terrain;
    public int width = 256;
    public int length = 256;
    public int mountainHeight = 20;
    public float mountainWidth = 20f;
    public float mountainLength = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    public float[,] heightmap;

    // Use this for initialization
    void Start () {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);       
    }
	
	// Update is called once per frame
	void Update () {
        heightmap = GenerateHeightMap();
        Renderer renderer = this.GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture(heightmap);
	}

    /// <summary>
    /// Generates a Texture based on the HeightMap given 
    /// </summary>
    /// <param name="heightmap">The Heightmap (Pixelcolor depends on Height)</param>
    /// <returns></returns>
    Texture2D GenerateTexture(float[,] heightmap)
    {
        Texture2D texture = new Texture2D(heightmap.GetLength(0), heightmap.GetLength(1));
        for (int x = 0; x < heightmap.GetLength(0); x++)
        {
            for (int y = 0; y < heightmap.GetLength(1); y++)
            {
                float height = heightmap[x, y];
                Color color = new Color(height, height, height);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Creates a Heightmap depending on this Variables
    /// </summary>
    /// <returns>2D Array with Heights on every Position</returns>
    float[,] GenerateHeightMap()
    {
        float[,] heights = new float[width, length];
        float height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                height = GeneratePerlinHeight(x, y);
                heights[x, y] = height;
            }
        }
        return heights;
    }

    /// <summary>
    /// Generate a Height using Perlin Noise
    /// </summary>
    /// <param name="x">X-Coordinate for Map</param>
    /// <param name="y">Y-Coordinate for Map</param>
    /// <returns></returns>
    float GeneratePerlinHeight(int x, int y)
    {
        //Teilen durch width bzw. length fuer Werte -1 bis 1
        float xCoord = (float)x / width * mountainWidth + offsetX;
        float yCoord = (float)y / length * mountainLength + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
