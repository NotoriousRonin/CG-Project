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
    /// Activates/Deactivates UI-Elements based on bool
    /// </summary>
    /// <param name="trigger">Bool Value</param>
    public void setBoolTrigger(bool trigger)
    {
        activateMenu(trigger);
    }
}