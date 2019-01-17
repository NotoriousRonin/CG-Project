using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMapData{

    /// <summary>
    /// Creates a Heightmap depending on this Variables
    /// </summary>
    /// <param name="mapWidth">Width of the Map</param>
    /// <param name="mapLength">Length of the Map</param>
    /// <param name="noiseFrequency">Frequency of Noise</param>
    /// <param name="octaveCount">Number of Octaves</param>
    /// <param name="persistance">Controls Decrease in Amplitude of Octaves</param>
    /// <param name="lacunarity">Controls Increase in Frequency of Octaves</param>
    /// <param name="seed">Perlin Noise gives the same Values, to create Randomness => Seed</param>
    /// <returns>2D Array with Heights on every Position</returns>
    public static float[,] GeneratePerlinHeightMap(int mapWidth, int mapLength, float noiseFrequency, int octaveCount, float persistance, float lacunarity, int seed)
    {
        //Array with HeightValues to Return
        float[,] heights = new float[mapWidth, mapLength];

        //Save Frequency
        float saveFrequency = noiseFrequency;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Create Offsets for the Octaves
        Vector2[] offsets = GeneratePSRNGOctaveOffsets(octaveCount, seed);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {

                float amplitude = 1;
                float height = 0;
                //Create Octaves
                for (int i = 0; i < octaveCount; i++)
                {
                    height += GeneratePerlinHeight(x, mapWidth, y, mapLength, noiseFrequency, offsets[i].x, offsets[i].y) * amplitude;

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
    private static float[,] NormalizeHeightMap(float maxHeight, float minHeight, float[,] heights)
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
    /// <param name="mapWidth">Width of the Map</param>
    /// <param name="y">Y-Coordinate for Map</param>
    /// <param name="mapLength">Length of the Map</param>
    /// <param name="noiseFrequency">Frequency of Noise</param>
    /// <param name="offsetX">X-Offset for Octave</param>
    /// <param name="offsetY">Y-Offset for Octave</param>
    /// <returns>A Height</returns>
    private static float GeneratePerlinHeight(int x, int mapWidth, int y, int mapLength, float noiseFrequency, float offsetX, float offsetY)
    {
        //Frequency "Zoom" Right Upper Corner
        //float xSample = (float)x / mapWidth * noiseFrequency + offsetX;
        //float ySample = (float)y / mapLength * noiseFrequency + offsetY;

        //Frequency "Zoom" Center
        float xSample = ((float)x * 4 / mapWidth - 2) * noiseFrequency + offsetX;
        float ySample = ((float)y * 4 / mapLength - 2) * noiseFrequency + offsetY;

        //Multiplying by 2 Minus 1 makes Negative Values
        return (Mathf.PerlinNoise(xSample, ySample) * 2 - 1);
    }

    /// <summary>
    /// Generate PseudoRNG Offsets for the Octaves
    /// </summary>
    /// <param name="octaveCount">Number of Octaves</param>
    /// <param name="seed">Perlin Noise gives the same Values, to create Randomness => Seed</param>
    /// <returns>Vector2[] with Offsets for each Octave</returns>
    private static Vector2[] GeneratePSRNGOctaveOffsets(int octaveCount, int seed)
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