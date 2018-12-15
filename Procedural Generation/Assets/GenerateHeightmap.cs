using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHeightmap : MonoBehaviour {

    /// <summary>
    /// Width and Length to calculate NoiseMap for
    /// </summary>
    public Terrain terrain;

    /// <summary>
    /// Max Height for a Mountain
    /// </summary>
    public int mountainHeight = 30;

    /// <summary>
    /// Frequency of Noise
    /// </summary>
    public float noiseFrequency = 8f;

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
    /// The HeightMap
    /// </summary>
    public float[,] heightmap;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //Makes it Auto-Updatable if Variables are changed
        heightmap = GeneratePerlinHeightMap();
        Renderer renderer = this.GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture(heightmap);
	}

    /// <summary>
    /// Generates a Texture based on the HeightMap given 
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <returns>A Texture with each Pixel having a Color depending on their Height</returns>
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
    float[,] GeneratePerlinHeightMap()
    {
        //Array with HeightValues to Return
        float[,] heights = new float[(int)terrain.terrainData.bounds.size.x, (int)terrain.terrainData.bounds.size.z];
        
        //Save Frequency
        float saveFrequency = noiseFrequency;
      
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Create Offsets for the Octaves
        Vector2[] offsets = GeneratePSRNGOctaveOffsets(octaveCount);

        for (int x = 0; x < terrain.terrainData.bounds.size.x; x++)
        {
            for (int y = 0; y < terrain.terrainData.bounds.size.z; y++)
            {
                
                float amplitude = 1;                   
                float height = 0;
                //Create Octaves
                for (int i = 0; i < octaveCount; i++)
                {                   
                    height += GeneratePerlinHeight(x, y, offsets[i].x, offsets[i].y)  * amplitude;
                    
                    amplitude *= persistance;
                    noiseFrequency *= lacunarity;
                }
                noiseFrequency = saveFrequency; //Reset noiseFrequency to Init Value
                heights[x, y] = height;

                //Find out the Range of Heights in the HeightMap to Normalize it
                if (height > maxNoiseHeight) maxNoiseHeight = height; 
                else if (height < minNoiseHeight) minNoiseHeight = height;                              
            }
        }

        //Normalize HeightMap to Range of 0 to 1
        heights = NormalizeHeightMap(maxNoiseHeight, minNoiseHeight, heights);

        return heights;
    }

    /// <summary>
    /// Normalize the Values of the HeightMap
    /// </summary>
    /// <param name="maxHeight">lowest Height in the HeightMap</param>
    /// <param name="minHeight">highest Height in the HeightMap</param>
    /// <param name="heights">The HeightMap</param>
    /// <returns>normalized HeightMap</returns>
    private float[,] NormalizeHeightMap(float maxHeight, float minHeight, float[,] heights)
    {
        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                heights[x, y] = Mathf.InverseLerp(minHeight, maxHeight, heights[x, y]);
            }
        }
        return heights;
    }

    /// <summary>
    /// Generate a Height using Perlin Noise
    /// </summary>
    /// <param name="x">X-Coordinate for Map</param>
    /// <param name="y">Y-Coordinate for Map</param>
    /// <returns>A Height</returns>
    private float GeneratePerlinHeight(int x, int y, float offsetX, float offsetY)
    {
        float xSample = (float)x / terrain.terrainData.bounds.size.x * noiseFrequency + offsetX;
        float ySample = (float)y / terrain.terrainData.bounds.size.z * noiseFrequency + offsetY;
        //Multiplying by 2 Minus 1 makes Negative Values
        return (Mathf.PerlinNoise(xSample, ySample) * 2 - 1);
    }

    /// <summary>
    /// Generate PseudoRNG Offsets for the Octaves
    /// </summary>
    /// <returns>Vector2[] with Offsets for each Octave</returns>
    private Vector2[] GeneratePSRNGOctaveOffsets(int octaveCount)
    {
        System.Random pseudoRNG = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaveCount];
        for (int i = 0; i < octaveCount; i++)
        {
            float offsetX = pseudoRNG.Next(-9999, 9999);
            float offsetY = pseudoRNG.Next(-9999, 9999);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }
}
