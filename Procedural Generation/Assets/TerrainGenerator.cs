using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    public int width = 256;
    public int length = 256;
    public int mountainHeight = 20;
    public float mountainWidth = 20f;
    public float mountainLength = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

	// Use this for initialization
	void Start () {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }
	
	// Update is called once per frame
	void Update () {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
	}

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, mountainHeight, length);
        terrainData.SetHeights(0, 0, GenerateHeightMap());
        return terrainData;
    }

    float[,] GenerateHeightMap()
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = GeneratePerlinHeight(x, y);
            }
        }
        return heights;
    }

    float GeneratePerlinHeight(int x , int y)
    {
        //Teilen durch width bzw. length fuer Werte -1 bis 1
        float xCoord = (float)x / width * mountainWidth + offsetX;
        float yCoord = (float)y / length * mountainLength + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
