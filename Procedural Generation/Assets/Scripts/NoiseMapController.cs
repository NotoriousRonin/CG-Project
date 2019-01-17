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
    public int mountainHeight = 0;

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
    /// Map Color based on Height and Gradient
    /// </summary>
    public Gradient gradient;

    /// <summary>
    /// Gradient to use If no Biome is added
    /// </summary>
    public Gradient defaultGradient;

    /// <summary>
    /// Rather the View is 3D or not (2D)
    /// </summary>
    public bool render3D = false;

    /// <summary>
    /// 2D Representation of Noise
    /// </summary>
    private GameObject view2D;

    /// <summary>
    /// 3D Representation of Noise
    /// </summary>
    private GameObject view3D;

    /// <summary>
    /// Initialise Views
    /// </summary>
    public void Start()
    {
        view2D = GameObject.Find("2DNoiseView");
        view3D = GameObject.Find("3DNoiseView");
        if (render3D)
        {
            view2D.SetActive(false);
            view3D.SetActive(true);
        }
        else
        {
            view2D.SetActive(true);
            view3D.SetActive(false);
        }
    }

    /// <summary>
    /// Permanent Run
    /// </summary>
    public void Update()
    {
        if (autoUpdate) GenerateView();
    }

    /// <summary>
    /// Generates the View using the given Renderer 
    /// </summary>
    public void GenerateView()
    {
        float[,] heightMap = NoiseMapData.GeneratePerlinHeightMap(mapWidth, mapLength, noiseFrequency, octaveCount, persistance, lacunarity, seed);
       
        if (render3D)
        {          
            NoiseMapMeshView viewMesh = GetComponent<NoiseMapMeshView>();
            viewMesh.DrawMesh(heightMap, mountainHeight, biome, gradient, defaultGradient);
        }
        else
        {           
            NoiseMap2DView viewTexture = GetComponent<NoiseMap2DView>();
            viewTexture.DrawTexture(heightMap, biome, terrains);
        }       
    }

    //Setting Methods for UI Controls
    public void SetMountainHeight(float newMountainHeight) 
    {
        mountainHeight = (int)newMountainHeight;
    }

    public void SetNoiseFrequency(float newNoiseFrequency) 
    {
        noiseFrequency = newNoiseFrequency;
    }

    public void SetOctaveCount(float newOctaveCount) 
    {
        octaveCount = (int)newOctaveCount;
    }
    
    public void SetPersistance(float newPersistance) 
    {
        persistance = newPersistance;
    }
    
    public void SetLacunarity(float newLacunarity) 
    {
        lacunarity = newLacunarity;
    }

    public void SetSeed(float newSeed)
    {
        seed = (int)newSeed;
    }

    public void SetAutoUpdate(bool newAutoUpdate)
    {
        autoUpdate = newAutoUpdate;
    }
    
    public void SetBiome(bool newBiome)
    {
        biome = newBiome;
    }

    /// <summary>
    /// Switches between Views
    /// </summary>
    /// <param name="newRender3D">Rather the View is a 3D Representation or not</param>
    public void SetRender3D(bool newRender3D)
    {
        if (newRender3D)
        {
            view2D.SetActive(false);
            view3D.SetActive(true);
        }
        else
        {
            view2D.SetActive(true);
            view3D.SetActive(false);
        }
        render3D = newRender3D;
    }

    /// <summary>
    /// TerrainType with Name, Height, Color
    /// Color of Points will be decided by their Height
    /// Points.Height smaller or equal to TerrainType.Color results in Point.Color = TerrainType.Color 
    /// </summary>
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}