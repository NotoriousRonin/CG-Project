using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapController : MonoBehaviour {

    /// <summary>
    /// The Width of the View to generate a Map for
    /// </summary>
    public int mapWidth = 256;

    /// <summary>
    /// The Length of the View to generate a Map for
    /// </summary>
    public int mapLength = 256;

    /// <summary>
    /// Max Height for a Mountain
    /// </summary>
    public int mountainHeight = 30;

    /// <summary>
    /// Frequency of Noise
    /// </summary>
    public float noiseFrequency = 2f;

    /// <summary>
    /// Number of Octaves to be added
    /// </summary>
    public int octaveCount = 1;

    /// <summary>
    /// Controls Decrease in Amplitude of Octaves
    /// </summary>
    public float persistance = 0.5f;

    /// <summary>
    /// Controls Increase in Frequency of Octaves
    /// </summary>
    public float lacunarity = 2;

    /// <summary>
    /// Perlin Noise gives the same Values, to create Randomness => Seed
    /// </summary>
    public int seed;

    /// <summary>
    /// Checks if GenerateView() should be run in Update()
    /// </summary>
    public bool autoUpdate = false;

    /// <summary>
    /// All Types of Terrains in the Map
    /// </summary>
    public TerrainType[] terrains;

    /// <summary>
    /// Checks if Biome should be added to Map
    /// </summary>
    public bool biome = false;

    /// <summary>
    /// Permanent Run
    /// </summary>
    public void Update()
    {
        if (autoUpdate) GenerateView();
    }

    /// <summary>
    /// Generates the View using NoiseMap2DView given Renderer 
    /// </summary>
    public void GenerateView()
    {
        float[,] heightMap = NoiseMapData.GeneratePerlinHeightMap(mapWidth, mapLength, noiseFrequency, octaveCount, persistance, lacunarity, seed);

        NoiseMap2DView view = GetComponent<NoiseMap2DView>();
        view.DrawTexture(heightMap, biome, terrains);
    }

    //Setting Methods for UI Controls
    public void setMountainHeight(float newMountainHeight) 
    {
        mountainHeight = (int)newMountainHeight;
    }

    public void setNoiseFrequency(float newNoiseFrequency) 
    {
        noiseFrequency = newNoiseFrequency;
    }

    public void setOctaveCount(float newOctaveCount) 
    {
        octaveCount = (int)newOctaveCount;
    }
    
    public void setPersistance(float newPersistance) 
    {
        persistance = newPersistance;
    }
    
    public void setLacunarity(float newLacunarity) 
    {
        lacunarity = newLacunarity;
    }

    public void setSeed(float newSeed)
    {
        seed = (int)newSeed;
    }

    public void setAutoUpdate(bool newAutoUpdate)
    {
        autoUpdate = newAutoUpdate;
    }
    
    public void setBiome(bool newBiome)
    {
        biome = newBiome;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}