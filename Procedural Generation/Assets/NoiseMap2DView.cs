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
    private static Texture2D GenerateTexture(float[,] heightmap)
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

    public void DrawTexture(float[,] heightmap)
    {
        renderer.material.mainTexture = GenerateTexture(heightmap);
        //renderer.transform.localScale = new Vector3(heightmap.GetLength(0), 1, heightmap.GetLength(1));
    }
}
