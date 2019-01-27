using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIElements : MonoBehaviour
{
    /// <summary>
    /// All UI Elements to moved up
    /// </summary>
    public GameObject[] uiElements;

    /// <summary>
    /// How much one UI Element should be moved up
    /// </summary>
    public float moveUpValue;

    /// <summary>
    /// How much one UI Element should be moved to the side
    /// </summary>
    public float moveSideValue;

    /// <summary>
    /// Rather the Elements should be moved up or down
    /// </summary>
    private bool up;

    /// <summary>
    /// Rather the Elements should be moved to the right
    /// </summary>
    private bool side;

    /// <summary>
    /// True if the Elements been moved up once
    /// Needed so the Elements can't be moved up and up again
    /// or down and down again
    /// </summary>
    private bool movedUp;

    /// <summary>
    /// True if the Elements been moved to the side once
    /// Needed so the Elements can't be moved right and right again
    /// or left and left again
    /// </summary>
    private bool movedSide;

    public void moveUIElements()
    {
        if ((up && moveUpValue < 0) || (!up && moveUpValue > 0)) moveUpValue *= -1;
        if (movedUp != up) //Move Elements when Variable "up" was actually changed
        {
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].GetComponent<RectTransform>().transform.Translate(0, moveUpValue, 0);
            }
            movedUp = up;
        }
        if ((side && moveSideValue < 0) || (!side && moveSideValue > 0)) moveSideValue *= -1;
        if (movedSide != side) //Move Elements when Variable "up" was actually changed
        {
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].GetComponent<RectTransform>().transform.Translate(moveSideValue, 0, 0);
            }
            movedSide = side;
        }

    }

    /// <summary>
    /// Set Method for up
    /// </summary>
    /// <param name="newBool"></param>
    public void setUp(bool newBool)
    {
        up = newBool;
        side = newBool;
    }
}