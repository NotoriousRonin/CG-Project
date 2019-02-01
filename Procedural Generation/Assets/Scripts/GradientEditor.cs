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
    /// Background Panel of BiomePanel
    /// </summary>
    public GameObject panel;

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
    ///  Button for deleting an AlphaKey
    /// </summary>
    public Button deleteAlphaKeyButton;

    /// <summary>
    /// Button for deleting a ColorKey
    /// </summary>
    public Button deleteColorKeyButton;

    /// <summary>
    /// Needed to provide an Gradient
    /// </summary>
    private NoiseMapController noiseMapController;

    /// <summary>
    /// Label Component of the ColorKeys Dropdown
    /// </summary>
    private Text colorDropdownLabel;

    /// <summary>
    /// Label Component of the AlphaKeys Dropdown
    /// </summary>
    private Text alphaDropdownLabel;

    public MeshRenderer colorPreview;

    public MeshRenderer gradientPreviewColorEditor;

    public MeshRenderer gradientViewColorEditor;

    public MeshRenderer gradientPreviewAlphaEditor;

    public MeshRenderer gradientViewAlphaEditor;

    //Initialize Controller
    public void Start()
    {
        noiseMapController = GetComponent<NoiseMapController>();
        colorDropdownLabel = colorKeyDropdown.GetComponentInChildren<Text>();
        alphaDropdownLabel = alphaKeyDropdown.GetComponentInChildren<Text>();
        showColorKey(0); //Show first Color of the Biome, ColorKeysEditor open by default
        deleteColorKeyButton.interactable = isDeletePossible(colorKeyDropdown);
        deleteAlphaKeyButton.interactable = isDeletePossible(alphaKeyDropdown);
        GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewColorEditor);
        resizePanel(71f);
    }

    /// <summary>
    /// Setup if Editor was changed
    /// Used for Dropdown Menu
    /// </summary>
    /// <param name="value">Value from the Dropdown Value</param>
    public void openEditor(int value)
    {
        if (value == 0)
        {
            alphaKeyEditor.GetComponentInChildren<ToggleMenu>().setBoolTrigger(false);
            colorKeyEditor.SetActive(true);
            alphaKeyEditor.SetActive(false);           
            showColorKey(0);
            deleteColorKeyButton.interactable = isDeletePossible(colorKeyDropdown);
            GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewColorEditor);
            //Hardcoding sucks I know! Tried not doing it. Sucked and didn't work making the UI look weird. 
            resizePanel(71f);
        }
        else
        {
            colorKeyEditor.GetComponentInChildren<ToggleMenu>().setBoolTrigger(false);
            colorKeyEditor.SetActive(false);
            alphaKeyEditor.SetActive(true);        
            showAlphaKey(0);
            deleteAlphaKeyButton.interactable = isDeletePossible(alphaKeyDropdown);
            GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewAlphaEditor);
            //Hardcoding sucks I know! Tried not doing it. Sucked and didn't work making the UI look weird. 
            resizePanel(155f);
        }
    }

    /// <summary>
    /// Adds a ColorKey to the NoiseMapController.biomeGradient
    /// </summary>
    public void addColorKey()
    {
        List<string> newColorKeyOptions = convertDropdownOptionsToStringList(colorKeyDropdown);
        newColorKeyOptions.Insert(0,"Unnamed Color Key");
        colorKeyDropdown.ClearOptions();
        colorKeyDropdown.AddOptions(newColorKeyOptions);

        GradientColorKey[] newGradientColorKeys;
        copyColorKeys(noiseMapController.biomeGradient.colorKeys, out newGradientColorKeys);
        newGradientColorKeys[0].color = new Color32(0, 0, 0, 255);
        newGradientColorKeys[0].time = 0;
        noiseMapController.biomeGradient.SetKeys(newGradientColorKeys, noiseMapController.biomeGradient.alphaKeys);

        colorKeyDropdown.value = 0;
        showColorKey(0);

        deleteColorKeyButton.interactable = isDeletePossible(colorKeyDropdown);

        GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewColorEditor);
    }

    /// <summary>
    /// Adds a AlphaKey to the NoiseMapController.biomeGradient
    /// </summary>
    public void addAlphaKey()
    {
        List<string> newAlphaKeyOptions = convertDropdownOptionsToStringList(alphaKeyDropdown);
        newAlphaKeyOptions.Insert(0, "New Key");
        alphaKeyDropdown.ClearOptions();
        alphaKeyDropdown.AddOptions(newAlphaKeyOptions);

        GradientAlphaKey[] newGradientAlphaKeys;
        copyAlphaKeys(noiseMapController.biomeGradient.alphaKeys, out newGradientAlphaKeys);
        newGradientAlphaKeys[0].alpha = 0;
        newGradientAlphaKeys[0].time = 0;
        noiseMapController.biomeGradient.SetKeys(noiseMapController.biomeGradient.colorKeys, newGradientAlphaKeys);

        alphaKeyDropdown.value = 0;
        showAlphaKey(0);

        deleteAlphaKeyButton.interactable = isDeletePossible(alphaKeyDropdown);

        GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewAlphaEditor);
    }

    /// <summary>
    /// Deletes the current ColorKey in the ColorKeyDropdown from NoiseMapController.biomeGradient
    /// </summary>
    public void deleteColorKey()
    {
        int index = colorKeyDropdown.value;

        List<string> newColorKeyOptions = convertDropdownOptionsToStringList(colorKeyDropdown);
        newColorKeyOptions.RemoveAt(index);
        colorKeyDropdown.ClearOptions();
        colorKeyDropdown.AddOptions(newColorKeyOptions);

        GradientColorKey[] newGradientColorKeys = removeColorKey(index, noiseMapController.biomeGradient.colorKeys);
        noiseMapController.biomeGradient.SetKeys(newGradientColorKeys, noiseMapController.biomeGradient.alphaKeys);       

        colorKeyDropdown.value = index; //index = the key that would have followed after the removed one
        showColorKey(index);

        deleteColorKeyButton.interactable = isDeletePossible(colorKeyDropdown);

        GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewColorEditor);
    }

    /// <summary>
    /// Delete the current AlphaKey in the AlphaKeyDropdown from NoiseMapController.biomeGradient
    /// </summary>
    public void deleteAlphaKey()
    {
        int index = alphaKeyDropdown.value;

        List<string> newAlphaKeyOptions = convertDropdownOptionsToStringList(alphaKeyDropdown);
        newAlphaKeyOptions.RemoveAt(index);
        alphaKeyDropdown.ClearOptions();
        alphaKeyDropdown.AddOptions(newAlphaKeyOptions);

        GradientAlphaKey[] newGradientAlphaKeys = removeAlphaKey(index, noiseMapController.biomeGradient.alphaKeys);
        noiseMapController.biomeGradient.SetKeys(noiseMapController.biomeGradient.colorKeys, newGradientAlphaKeys);

        alphaKeyDropdown.value = index; //index = the key that would have followed after the removed one
        updateAlphaNames();
        showAlphaKey(index);

        deleteAlphaKeyButton.interactable = isDeletePossible(alphaKeyDropdown);

        GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewAlphaEditor);
    }

    /// <summary>
    /// Update the current ColorKey in the ColorKeyDropdown from the NoiseMapController.biomeGradient
    /// </summary>
    public void updateColorKey()
    {
        //Outputs for Parse
        GradientColorKey[] gradientColorKeys;
        int index;
        float time;

        //Validate Parse
        bool successfullParse = getColorKeysFromInput(out gradientColorKeys, out index, out time);

        //Add if Validation of everything was successful
        if (successfullParse)
        {   
            //Update the Gradient
            noiseMapController.biomeGradient.SetKeys(gradientColorKeys, noiseMapController.biomeGradient.alphaKeys);
            //Update Dropdown
            string colorName = inputColorKeyName.text;
            sortValues(colorKeyDropdown, index, time, "ColorKeys");
            colorDropdownLabel.text = colorName;
            //Update GradientView
            GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewColorEditor);
        }     
    }

    /// <summary>
    /// Make a Preview of the Color and the resulting Gradient from InputFields
    /// </summary>
    public void previewColorKeyChange()
    {
        //Outputs for Parse
        GradientColorKey[] gradientColorKeys;
        int index;
        float time;

        //Validate Parse
        bool successfullParse = getColorKeysFromInput(out gradientColorKeys, out index, out time);

        if (successfullParse)
        {
            //Color Preview
            Color32 color = gradientColorKeys[index].color;
            color.a = 255;
            GradientPreview.setRendererTextureOneColor(color, colorPreview);
            //Gradient Preview
            Gradient previewGradient = new Gradient();
            previewGradient.SetKeys(gradientColorKeys, noiseMapController.biomeGradient.alphaKeys);
            GradientPreview.setRendererGradientTexture(previewGradient, gradientPreviewColorEditor);
            //Hardcoding sucks I know! Tried not doing it. Sucked and didn't work while making the UI look weird. 
            resizePanel(0f);
        } 
    }

    /// <summary>
    /// Create a GradientColorKey[] based on the InputFields
    /// </summary>
    /// <param name="gradientColorKeys">Output for GradientColorKey[]</param>
    /// <param name="index">Output for the Index of that new GradientColorKey</param>
    /// <param name="time">Output for the Time of that new GradientColorKey</param>
    /// <returns></returns>
    private bool getColorKeysFromInput(out GradientColorKey[] gradientColorKeys, out int index, out float time)
    {
        gradientColorKeys = noiseMapController.biomeGradient.colorKeys;
        bool sucessfullParse = true;
        index = colorKeyDropdown.value;
        byte red;
        byte green;
        byte blue;

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
        if (sucessfullParse)
        {
            Color color = new Color32(red, green, blue, 255);
            gradientColorKeys[index].color = color;
            gradientColorKeys[index].time = time;
        }
        return sucessfullParse;
    }
    /// <summary>
    /// Update the current AlphaKey in the AlphaKeyDropdown from the NoiseMapController.biomeGradient
    /// </summary>
    public void updateAlphaKey()
    {
        //Outputs for Parse
        GradientAlphaKey[] gradientAlphaKeys;
        int index;
        float time;

        //Validate Parse
        bool successfullParse = getAlphaKeysFromInput(out gradientAlphaKeys, out index, out time);

        //Add if Validation of everything was successful
        if (successfullParse)
        {       
            //Update Gradient
            noiseMapController.biomeGradient.SetKeys(noiseMapController.biomeGradient.colorKeys, gradientAlphaKeys);
            //Update Dropdown
            sortValues(alphaKeyDropdown, index, time, "AlphaKeys");            
            updateAlphaNames();
            alphaDropdownLabel.text = alphaKeyDropdown.options[alphaKeyDropdown.value].text;
            //Update GradientView
            GradientPreview.setRendererGradientTexture(noiseMapController.biomeGradient, gradientViewAlphaEditor);
        }
    }

    /// <summary>
    /// Make a Preview of the Change to the Alpha Key
    /// </summary>
    public void previewAlphaKeyChange()
    {
        //Outputs for Parse
        GradientAlphaKey[] gradientAlphaKeys;
        int index;
        float time;

        //Validate Parse
        bool successfullParse = getAlphaKeysFromInput(out gradientAlphaKeys, out index, out time);

        if (successfullParse)
        {
            Gradient previewGradient = new Gradient();
            previewGradient.SetKeys(noiseMapController.biomeGradient.colorKeys, gradientAlphaKeys);
            GradientPreview.setRendererGradientTexture(previewGradient, gradientPreviewAlphaEditor);
            //Hardcoding sucks I know! Tried not doing it. Sucked and didn't work making the UI look weird. 
            resizePanel(89.15f);
        }
    }

    /// <summary>
    /// Create an GradientAlphaKey[] based on the InputFields
    /// </summary>
    /// <param name="gradientAlphaKeys">Output for the GradientAlphaKey[]</param>
    /// <param name="index">Output for the Index of that new GradientAlphaKey</param>
    /// <param name="time">Output for the Time of that new GradientAlphaKey</param>
    /// <returns>TRUE if Input to the Inputfields could be parsed</returns>
    private bool getAlphaKeysFromInput(out GradientAlphaKey[] gradientAlphaKeys, out int index, out float time)
    {
        gradientAlphaKeys = noiseMapController.biomeGradient.alphaKeys;
        int alpha;
        bool successfullParse = true;
        index = alphaKeyDropdown.value;

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

        if (successfullParse)
        {
            gradientAlphaKeys[index].alpha = alpha;
            gradientAlphaKeys[index].time = time;
        }

        //Return rather Parse was successful
        return successfullParse;
    }
    /// <summary>
    /// Shows the ColorKey Values in the InputFields
    /// </summary>
    public void showColorKey(int index)
    {
        Color32 color = noiseMapController.biomeGradient.colorKeys[index].color;
        List<string> colorKeyOptions = convertDropdownOptionsToStringList(colorKeyDropdown);
        inputColorKeyName.text = colorKeyOptions[index];
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
                //Not Rounding causes an Error in the comparison due Float Value
                float timeInGradient = (float)System.Math.Round(gradientColorKeys[newIndex].time, 2);
                time = (float)System.Math.Round(time, 3);
                if (time <= timeInGradient) break;
            }
            if (newIndex == gradientColorKeys.Length) newIndex--;
        }
        if (keys == "AlphaKeys")
        {
            GradientAlphaKey[] gradientAlphaKeys = noiseMapController.biomeGradient.alphaKeys;
            for (newIndex = 0; newIndex < gradientAlphaKeys.Length; newIndex++)
            {
                //Not Rounding causes an Error in the comparison due Float Value
                float timeInGradient = (float)System.Math.Round(gradientAlphaKeys[newIndex].time, 2);
                time = (float)System.Math.Round(time, 2);
                if (time <= timeInGradient) break;
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
        if (newIndex != currentIndex)
        {
            List<string> newOptions = new List<string>();
            if (keys == "ColorKeys") newOptions = convertDropdownOptionsToStringList(colorKeyDropdown);
            if (keys == "AlphaKeys") newOptions = convertDropdownOptionsToStringList(alphaKeyDropdown);
            string newName = newOptions[currentIndex];
            newOptions.RemoveAt(currentIndex);
            newOptions.Insert(newIndex, newName);
            dropdown.ClearOptions();
            dropdown.AddOptions(newOptions);
            dropdown.value = newIndex;
        }      
    }

    /// <summary>
    /// Updates the Dropdown Options after change to the AlphaKeyDropdown
    /// </summary>
    private void updateAlphaNames()
    {
        for (int i = 0; i < alphaKeyDropdown.options.Count; i++)
        {
            alphaKeyDropdown.options[i].text = "Alpha Key [" + i + "]";
        }
    }

    /// <summary>
    /// Copys the Value from a GradientColorKey[]
    /// </summary>
    /// <param name="gradientColorKeys">Values getting copied from</param>
    /// <param name="copy">Result containing the copied Values, copy[0] is null </param>
    private void copyColorKeys(GradientColorKey[] gradientColorKeys, out GradientColorKey[] copy)
    {
        copy = new GradientColorKey[gradientColorKeys.Length + 1];
        for (int i = 0; i < gradientColorKeys.Length; i++)
        {
            copy[i + 1] = gradientColorKeys[i];
        }
    }

    /// <summary>
    /// Copys the Value from a GradientAlphaKey[]
    /// </summary>
    /// <param name="gradientAlphaKeys">Values getting copied from</param>
    /// <param name="copy">Result containing the copied Values starting at 1</param>
    private void copyAlphaKeys(GradientAlphaKey[] gradientAlphaKeys, out GradientAlphaKey[] copy)
    {
        copy = new GradientAlphaKey[gradientAlphaKeys.Length + 1];
        for (int i = 0; i < gradientAlphaKeys.Length; i++)
        {
            copy[i + 1] = gradientAlphaKeys[i];
        }
    }

    /// <summary>
    /// Dropdown Options as a List
    /// </summary>
    /// <param name="dropdown">The Dropdown to create a List of</param>
    /// <returns>AllDropdown Options Text in a List</returns>
    private List<string> convertDropdownOptionsToStringList(Dropdown dropdown)
    {
        List<string> options = new List<string>();
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            options.Add(dropdown.options[i].text);
        }
        return options;
    }

    /// <summary>
    /// Removes a Key from the GradientColorKey[]
    /// </summary>
    /// <param name="index">Index of the Key to be removed</param>
    /// <param name="gradientColorKeys">GradientColorKeys of a Gradient</param>
    /// <returns>A GradientColorKey[] with the Key at index removed</returns>
    private GradientColorKey[] removeColorKey(int index, GradientColorKey[] gradientColorKeys)
    {
        GradientColorKey[] newGradientColorKeys = new GradientColorKey[gradientColorKeys.Length - 1];
        for (int h = 0; h < index; h++)
        {
            newGradientColorKeys[h] = gradientColorKeys[h];
        }
        for (int i = index; i < gradientColorKeys.Length - 1; i++)
        {
            newGradientColorKeys[i] = gradientColorKeys[i + 1];
        }
        return newGradientColorKeys;
    }

    /// <summary>
    /// Removes a Key from the GradientAlphaKey[]
    /// </summary>
    /// <param name="index">Index of the Key to be removed</param>
    /// <param name="gradientAlphaKeys">GradientAlphaKeys of a Gradient</param>
    private GradientAlphaKey[] removeAlphaKey(int index, GradientAlphaKey[] gradientAlphaKeys)
    {
        GradientAlphaKey[] newGradientAlphaKeys = new GradientAlphaKey[gradientAlphaKeys.Length - 1];
        for (int h = 0; h < index; h++)
        {
            newGradientAlphaKeys[h] = gradientAlphaKeys[h];
        }
        for (int i = index; i < gradientAlphaKeys.Length - 1; i++)
        {
            newGradientAlphaKeys[i] = gradientAlphaKeys[i + 1];
        }        
        return newGradientAlphaKeys;
    }

    /// <summary>
    /// Checks if Deleting a Color/Alpha Key is possible
    /// </summary>
    /// <param name="dropdown">ColorKeyDropdown if checking for ColorKeys, equivalent for AlphaKeys</param>
    /// <returns>True if Delete is possible, else false</returns>
    private bool isDeletePossible(Dropdown dropdown)
    {
        return (dropdown.options.Count > 2);
    }

    /// <summary>
    /// Resize Panel based on size given
    /// </summary>
    /// <param name="size">The new Size of the Panel</param>
    public void resizePanel(float size)
    {
        panel.GetComponent<RectTransform>().offsetMin = new Vector2(panel.GetComponent<RectTransform>().offsetMin.x, size);
    }
}