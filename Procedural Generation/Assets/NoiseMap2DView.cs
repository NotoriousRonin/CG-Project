using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap2DView : MonoBehaviour {

    /// <summary>
    /// The Renderer of the Plane
    /// </summary>
    public new Renderer renderer;    

    /// <summary>
    /// Generates a Texture based on the HeightMap given 
    /// </summary>
    /// <param name="heightmap">The Heightmap</param>
    /// <returns>A Texture with each Pixel having a Color depending on their Height</returns>
    private static Texture2D GenerateTexture(float[,] heightmap, bool biome, NoiseMapController.TerrainType[] terraintypes)
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

    public void DrawTexture(float[,] heightmap, bool biome, NoiseMapController.TerrainType[] terrainTypes)
    {
        renderer.material.mainTexture = GenerateTexture(heightmap, biome, terrainTypes);
    }
}