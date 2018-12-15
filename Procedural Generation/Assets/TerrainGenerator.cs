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
        terrainData.heightmapResolution = (int)terrainData.bounds.size.x + 1;
        terrainData.size = new Vector3(terrainData.bounds.size.x, heightMapData.mountainHeight, terrainData.bounds.size.z);
        terrainData.SetHeights(0, 0, heightMapData.heightmap);
        return terrainData;
    }
}