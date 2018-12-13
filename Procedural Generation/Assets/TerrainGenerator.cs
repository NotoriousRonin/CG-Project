using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public GameObject HeightMap;
    private GenerateHeightmap heightMapData;
    // Use this for initialization
    void Start()
    {
        heightMapData = HeightMap.GetComponent<GenerateHeightmap>();
    }

    // Update is called once per frame
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData, heightMapData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData, GenerateHeightmap heightMapData)
    {
        terrainData.heightmapResolution = heightMapData.width + 1;
        terrainData.size = new Vector3(heightMapData.width, heightMapData.mountainHeight, heightMapData.length);
        terrainData.SetHeights(0, 0, heightMapData.heightmap);
        return terrainData;
    }
}