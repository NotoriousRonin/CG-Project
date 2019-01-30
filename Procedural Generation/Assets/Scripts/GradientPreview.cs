using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientPreview
{
    /// <summary>
    /// Width of the new Texture
    /// </summary>
    private const int textureWidth = 100; //Anything would work

    /// <summary>
    /// Height of the new Texture
    /// </summary>
    private const int textureHeight = 100; //Anything would work

    /// <summary>
    /// Set Color of viewRenderer to a single Color
    /// </summary>
    /// <param name="viewRenderer">The Renderer</param>
    /// <param name="color">The single Color</param>
    public static void setRendererTextureOneColor(Color32 color, MeshRenderer viewRenderer)
    {
        viewRenderer.material.color = color;
    }

    /// <summary>
    /// Set Texture of viewRenderer to a Gradient Texture
    /// </summary>
    /// <param name="viewRenderer">The Renderer</param>
    /// <param name="gradient">The Gradient</param>
    public static void setRendererGradientTexture(Gradient gradient, MeshRenderer viewRenderer)
    {
        viewRenderer.material.mainTexture = createGradientTexture(gradient);
    }

    /// <summary>
    /// Creates a Texture2D based on the Gradient given
    /// </summary>
    /// <param name="gradient">The Gradient</param>
    /// <returns></returns>
    private static Texture2D createGradientTexture(Gradient gradient)
    {
        Texture2D newTexture2D = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                Color32 color = gradient.Evaluate((float)x / textureWidth);
                newTexture2D.SetPixel(x, y, color);
            }
        }
        newTexture2D.Apply();
        return newTexture2D;
    }
}