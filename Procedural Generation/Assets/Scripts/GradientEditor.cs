using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientEditor : MonoBehaviour
{
    /// <summary>
    /// Panel with Color Key Editor Menu
    /// </summary>
    public GameObject colorKeyEditor;

    /// <summary>
    /// Panel with Alpha Key Editor Menu
    /// </summary>
    public GameObject alphaKeyEditor;

    /// <summary>
    /// Panel containing both Editor Menus to resize accordingly to active Menu
    /// </summary>
    public GameObject panelBackground;

    /// <summary>
    /// Dropdown for the Gradient Color Keys
    /// </summary>
    public Dropdown colorKeyDropdown;

    /// <summary>
    /// Dropdown for the Gradient Alpha Keys
    /// </summary>
    public Dropdown alphaKeyDropdown;

    /// <summary>
    /// Inputfield for the Name of a ColorKey
    /// </summary>
    public InputField inputColorKeyName;

    /// <summary>
    /// Inputfield for creating a ColorKey, Red Value
    /// </summary>
    public InputField inputRed;

    /// <summary>
    /// Inputfield for creating a ColorKey, Green Value
    /// </summary>
    public InputField inputGreen;

    /// <summary>
    /// Inputfield for creating a ColorKey, Blue Value
    /// </summary>
    public InputField inputBlue;

    /// <summary>
    /// Inputfield for creating Alpha Key
    /// </summary>
    public InputField inputAlpha;

    /// <summary>
    /// Inputfield for creating ColorKey, Time Value
    /// </summary>
    public InputField inputColorHeight;

    /// <summary>
    /// Inputfield for creating AlphaKey, Time Value
    /// </summary>
    public InputField inputAlphaHeight;

    /// <summary>
    /// Array of Names selected for the Colors
    /// </summary>
    private List<string> colorNames = new List<string>();

    /// <summary>
    /// Needed to provide an Gradient
    /// </summary>
    private NoiseMapController noiseMapController;

    /// <summary>
    /// Label Component of the ColorKeys Dropdown
    /// </summary>
    private Text colorDropdownLabel;

    //Initialize Controller
    public void Start()
    {
        noiseMapController = GetComponent<NoiseMapController>();
        colorDropdownLabel = colorKeyDropdown.GetComponentInChildren<Text>();
        //Default Names
        colorNames.Add("Water");
        colorNames.Add("Grass");
        colorNames.Add("Mountain");
        colorNames.Add("High Mountain");
        colorNames.Add("Snow");
        //Show First Biome Color
        showColorKey(0);
    }

    /// <summary>
    /// Depending on the Value given a Editor Window will be opened
    /// Used for Dropdown Menu
    /// </summary>
    /// <param name="value">Value from the Dropdown Value</param>
    public void openEditor(int value)
    {
        if (value == 0)
        {
            colorKeyEditor.SetActive(true);
            alphaKeyEditor.SetActive(false);
            panelBackground.GetComponent<RectTransform>().offsetMin = new Vector2(panelBackground.GetComponent<RectTransform>().offsetMin.x, -4.949979e-05f);
            showColorKey(0);
        }
        else
        {
            colorKeyEditor.SetActive(false);
            alphaKeyEditor.SetActive(true);
            panelBackground.GetComponent<RectTransform>().offsetMin = new Vector2(panelBackground.GetComponent<RectTransform>().offsetMin.x, 73.175f);
            showAlphaKey(0);
        }
    }

    public void addColorKey()
    {
        colorNames.Insert(0,"Unnamed Color Key");
        colorKeyDropdown.ClearOptions();
        colorKeyDropdown.AddOptions(colorNames);

       
    }

    public void addAlphaKey() { }

    public void deleteColorKey() { }

    public void deleteAlphaKey() { }

    /// <summary>
    /// Update a Color Key
    /// </summary>
    public void updateColorKey()
    {
        GradientColorKey[] gradientColorKeys = noiseMapController.biomeGradient.colorKeys;
        bool sucessfullParse = true;
        int index = colorKeyDropdown.value;
        byte red;
        byte green;
        byte blue;
        float time;
        
        //Validate Red Input
        if (!byte.TryParse(inputRed.text, out red))
        {
            inputRed.text = "ERR";
            sucessfullParse = false;
        }
        else if (red > 255 || red < 0)
        {
            inputRed.text = "ERR";
            sucessfullParse = false;
        }

        //Validate Green Input
        if (!byte.TryParse(inputGreen.text, out green))
        {
            inputGreen.text = "ERR";
            sucessfullParse = false;
        }
        else if (green > 255 || green < 0)
        {
            inputGreen.text = "ERR";
            sucessfullParse = false;
        }

        //Validate Blue Input
        if (!byte.TryParse(inputBlue.text, out blue))
        {
            inputBlue.text = "ERR";
            sucessfullParse = false;
        }
        else if (blue > 255 || blue < 0)
        {
            inputBlue.text = "ERR";
            sucessfullParse = false;
        }

        //Validate Time Input
        if (!float.TryParse(inputColorHeight.text, out time))
        {
            inputColorHeight.text = "ERR";
            sucessfullParse = false;
        }
        else if (time > 1f || time < 0f)
        {
            inputColorHeight.text = "ERR";
            sucessfullParse = false;
        }

        //Add if Validation of everything was successful
        if (sucessfullParse)
        {
            gradientColorKeys[index].color = new Color32(red, green, blue, 255);
            gradientColorKeys[index].time = time;
            noiseMapController.biomeGradient.SetKeys(gradientColorKeys, noiseMapController.biomeGradient.alphaKeys);
            string colorName = inputColorKeyName.text;
            colorNames[index] = colorName;
            colorDropdownLabel.text = colorName;
            colorKeyDropdown.options[index].text = colorName;
            sortValues(colorKeyDropdown, index, time, "ColorKeys");
        }
    }

    /// <summary>
    /// Update a AlphaKey
    /// </summary>
    public void updateAlphaKey()
    {
        GradientAlphaKey[] gradientAlphaKeys = noiseMapController.biomeGradient.alphaKeys;
        int alpha;
        float time;
        bool successfullParse = true;
        int index = colorKeyDropdown.value;

        //Validate Alpha Input
        if (!int.TryParse(inputAlpha.text, out alpha))
        {
            inputAlpha.text = "ERR";
            successfullParse = false;
        }
        else if (alpha > 255 || alpha < 0)
        {
            inputAlpha.text = "ERR";
            successfullParse = false;
        }

        //Validate Time Input
        if (!float.TryParse(inputAlphaHeight.text, out time))
        {
            inputAlphaHeight.text = "ERR";
            successfullParse = false;
        }
        else if (time > 1f || time < 0f)
        {
            inputAlphaHeight.text = "ERR";
            successfullParse = false;
        }

        //Add if Validation of everything was successful
        if (successfullParse)
        {
            gradientAlphaKeys[index].alpha = alpha;
            gradientAlphaKeys[index].time = time;
            noiseMapController.biomeGradient.SetKeys(noiseMapController.biomeGradient.colorKeys, gradientAlphaKeys);
            sortValues(alphaKeyDropdown, index, time, "AlphaKeys");
        }
    }

    /// <summary>
    /// Shows the ColorKey Values in the InputFields
    /// </summary>
    public void showColorKey(int index)
    {
        Color32 color = noiseMapController.biomeGradient.colorKeys[index].color;
        inputColorKeyName.text = colorNames[index];
        inputRed.text = "" + color.r;
        inputGreen.text = "" + color.g;
        inputBlue.text = "" + color.b;
        inputColorHeight.text = "" + noiseMapController.biomeGradient.colorKeys[index].time;
    }

    /// <summary>
    /// Shows the AlphaKey Values in the InputFields
    /// </summary>
    public void showAlphaKey(int index)
    {
        inputAlpha.text = "" + noiseMapController.biomeGradient.alphaKeys[index].alpha;
        inputAlphaHeight.text = "" + noiseMapController.biomeGradient.alphaKeys[index].time;
    }

    /// <summary>
    /// Sort Dropdown List after Changes
    /// </summary>
    /// <param name="dropdown">Dropdown Menu that is currently open</param>
    /// <param name="currentIndex">Index of the Item that got changed</param>
    /// <param name="time">Height Value of that Item</param>
    /// <param name="keys">"ColorKeys" or "AlphaKeys"</param>
    private void sortValues(Dropdown dropdown, int currentIndex, float time, string keys)
    {
        //newIndex is the Index of the Element in the sorted ColorKeys of the actual gradient
        int newIndex = 0;

        //Search for the newIndex
        if (keys == "ColorKeys")
        {
            GradientColorKey[] gradientColorKeys = noiseMapController.biomeGradient.colorKeys;
            for (newIndex = 0; newIndex < gradientColorKeys.Length; newIndex++)
            {
                if (gradientColorKeys[newIndex].time > time) break;
            }
            if (newIndex == gradientColorKeys.Length) newIndex--;
        }
        if (keys == "AlphaKeys")
        {
            GradientAlphaKey[] gradientAlphaKeys = noiseMapController.biomeGradient.alphaKeys;
            for (newIndex = 0; newIndex < gradientAlphaKeys.Length; newIndex++)
            {
                if (gradientAlphaKeys[newIndex].time > time) break;
            }
            if (newIndex == gradientAlphaKeys.Length) newIndex--;
        }

        /* 
         * This s**t was needed when colorNames was still an Array
         * 
        //Sort the Dropdown Options
        string save = colorNames[newIndex];
        colorNames[newIndex] = colorNames[currentIndex];
        colorNames.
        dropdown.options[newIndex].text = colorNames[currentIndex];
        if (newIndex > currentIndex)
        {        
            for (int i = currentIndex; i < newIndex - 1; i++)
            {
                dropdown.options[i].text = colorNames[i + 1];
                colorNames[i] = colorNames[i + 1];                
            }
            dropdown.options[newIndex - 1].text = save;
            colorNames[newIndex - 1] = save;
        }
        if (newIndex < currentIndex)
        {
            for (int i = currentIndex; i > newIndex + 1; i--)
            {
                dropdown.options[i].text = colorNames[i - 1];
                colorNames[i] = colorNames[i - 1];
            }
            dropdown.options[newIndex + 1].text = save;
            colorNames[newIndex + 1] = save;
        }
        */

        string newName = colorNames[currentIndex];
        colorNames.RemoveAt(currentIndex);
        colorNames.Insert(newIndex, newName);
        dropdown.ClearOptions();
        dropdown.AddOptions(colorNames);
        dropdown.value = newIndex;
    }

    private void copyColorKeys(int index,GradientColorKey[] gradientColorKeys, out GradientColorKey[] copy)
    {
        copy = new GradientColorKey[gradientColorKeys.Length+1];
        for (int i = index; i < gradientColorKeys.Length; i++)
        {
            copy[i] = gradientColorKeys[i];
        }
    }
}