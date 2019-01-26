using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    /// <summary>
    /// UI Elements belonging to this Menu
    /// </summary>
    public GameObject[] uiElements;

    /// <summary>
    /// Rather the UIElements are active
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// MoveUI Elements if there is such component
    /// </summary>
    private MoveUIElements moveUIElementsScript;

    // Start is called before the first frame update
    void Start()
    {
        activateMenu(false);
        moveUIElementsScript = GetComponent<MoveUIElements>();
        if (moveUIElementsScript != null)
        {
            moveUIElementsScript.setUp(true);
            moveUIElementsScript.moveUIElements();
        }
    }

    /// <summary>
    /// Sets the UI Elements active/inactive
    /// </summary>
    private void activateMenu(bool active)
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(active);
        }

        if (moveUIElementsScript!= null)
        {
            moveUIElementsScript.setUp(!active);
            moveUIElementsScript.moveUIElements();
        }
    }

    /// <summary>
    /// If the Value is above 1 the UI-Elements will be set active
    /// If the Value equals 1 the UI-Elements will be set inactive
    /// </summary>
    /// <param name="trigger">Float Value</param>
    public void setIntTrigger(float trigger)
    {
        if (trigger > 1 && !isOpen) activateMenu(true);
        if (trigger == 1 && isOpen) activateMenu(false);
    }

    /// <summary>
    /// Activates/Deactivates UI-Elements based on bool
    /// </summary>
    /// <param name="trigger">Bool Value</param>
    public void setBoolTrigger(bool trigger)
    {
        activateMenu(trigger);
    }
}