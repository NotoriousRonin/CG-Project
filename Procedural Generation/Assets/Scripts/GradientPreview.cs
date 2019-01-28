using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientPreview : MonoBehaviour
{
    public MeshRenderer colorPreview;
    public MeshRenderer gradientPreview;
    private int textureWidth = 100;
    private int textureHeight = 100;

    public void rendererTextureOneColor(Color32 color)
    {
        colorPreview.material.color = color;
    }
    
    public void setRendererGradientTexture(Gradient gradient)
    {
        gradientPreview.material.mainTexture = createGradientTexture(gradient);
    }

    private Texture2D createGradientTexture(Gradient gradient)
    {
        Texture2D newTexture2D = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                Color color = gradient.Evaluate((float)x / textureWidth);
                newTexture2D.SetPixel(x, y, color);
            }
        }
        newTexture2D.Apply();
        return newTexture2D;
    }
}
