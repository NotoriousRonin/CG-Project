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
    /// Checks if Biome should be added to Map
    /// </summary>
    public bool biome = false;

    /// <summary>
    /// Map Color based on Height and Gradient
    /// </summary>
    public Gradient biomeGradient;

    /// <summary>
    /// Gradient to use If no Biome is added
    /// </summary>
    public Gradient defaultGradient;

    /// <summary>
    /// Rather the View is 3D or not (2D)
    /// </summary>
    public bool render3D = false;

    /// <summary>
    /// Rather multiple Octaves should be added to Noise
    /// </summary>
    public bool multipleOctaves = false;

    /// <summary>
    /// Saves the Octave# Value
    /// </summary>
    private int saveOctaveCount;

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
            if (biome) viewMesh.DrawMesh(heightMap, mountainHeight, biomeGradient);
            else viewMesh.DrawMesh(heightMap, mountainHeight, defaultGradient);
        }
            
        NoiseMap2DView viewTexture = GetComponent<NoiseMap2DView>();
        viewTexture.DrawTexture(heightMap, biome, biomeGradient);
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
    /// Save Octave# before switching multipleOctaves off
    /// and set Octave# back to when switching multipleOctaves on
    /// </summary>
    /// <param name="newMultipleOctaves">Rather multiple Octaves should be added to Noise</param>
    public void setMultipleOctaves(bool newMultipleOctaves)
    {
        if (!newMultipleOctaves)
        {
            saveOctaveCount = octaveCount;
            octaveCount = 1;
        }
        else octaveCount = saveOctaveCount;
    }

    /// <summary>
    /// Switches between Views
    /// </summary>
    /// <param name="newRender3D">Rather the View is a 3D Representation or not</param>
    public void SetRender3D(bool newRender3D)
    {
        if (newRender3D)
        {
            view2D.transform.position = new Vector3(710, -22.5f, 60);
            view2D.transform.localScale = new Vector3(19.5f, 1, 19.5f);
            view3D.SetActive(true);
        }
        else
        {
            view2D.transform.position = new Vector3(447.3f, -22.5f, -206.5f);
            view2D.transform.localScale = new Vector3(70, 1, 70);
            view3D.SetActive(false);
        }
        render3D = newRender3D;
        GenerateView();
    }
}