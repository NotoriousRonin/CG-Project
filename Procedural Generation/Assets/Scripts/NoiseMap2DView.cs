﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides Functionality to put a 2DTexture to a Renderer 
/// based on the HeightMap
/// </summary>
public class NoiseMap2DView : MonoBehaviour {

    /// <summary>
    /// The Renderer of the Plane
    /// </summary>
    public MeshRenderer renderer2D;

    /// <summary>
    /// Generates a Texture based on the HeightMap given 
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <param name="biome">Checks if Biome should be added to the 2DTexture</param>
    /// <param name="terrainTypes">Struct TerrainType(Name,Color,Height) needed for adding Biome</param>
    /// <returns>A Texture with each Pixel having a Color depending on their Height</returns>
    private Texture2D GenerateTexture(float[,] heightmap, bool biome, NoiseMapController.TerrainType[] terraintypes)
    {
        Texture2D texture = new Texture2D(heightmap.GetLength(0), heightmap.GetLength(1));
        for (int x = 0; x < heightmap.GetLength(0); x++)
        {
            if (biome)
            {
                for (int y = 0; y < heightmap.GetLength(1); y++)
                {
                    float height = heightmap[x, y];
                    Color color = Color.white;             
                    for (int i = 0; i < terraintypes.Length; i++)
                    {
                        if (height <= terraintypes[i].height)
                        {
                            color = terraintypes[i].color;
                            break;
                        }
                    }                 
                    texture.SetPixel(x, y, color);                   
                }
            }
            else
            {
                for (int y2 = 0; y2 < heightmap.GetLength(1); y2++)
                {
                    float height = heightmap[x, y2];
                    Color color = Color.Lerp(Color.black, Color.white, height);
                    texture.SetPixel(x, y2, color);
                }             
            }           
        }
        if (biome)
        {
            //Makes it easier to look at
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
        }       
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Sets the Texture of the Plane using it Renderer
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <param name="biome">True if Biome should be added</param>
    /// <param name="terrainTypes">Struct TerrainType(Name,Color,Height) needed for adding Biome</param>
    public void DrawTexture(float[,] heightmap, bool biome, NoiseMapController.TerrainType[] terrainTypes)
    {
        Texture2D texture = GenerateTexture(heightmap, biome, terrainTypes);
        renderer2D.material.mainTexture = texture;    
    }
}