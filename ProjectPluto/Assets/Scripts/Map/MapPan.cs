using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pans the map
/// </summary>
public class MapPan : MonoBehaviour
{
    //Cur position of all objects
    public Vector2 cur_position;

    //Min/max positions
    public float minX, maxX, minY, maxY;

    /// <summary>
    /// How much of the screen is used for horizontal panning. 5 means one fifth is used for left pan, one fifth used for right pan.
    /// So only 3/5s are left for gameplay.
    /// </summary>
    public float width_section_size = 5;

    /// <summary>
    /// How much of the screen is used for vertical panning. 5 means one fifth slice is used for bottom panning, and another one fifth slice is used for up panning.
    /// So only 3/5s of the screen is left without panning.
    /// </summary>
    public float height_section_size = 5;

    // Update is called once per frame
    void Update()
    {
        //Get Mouse positions
        Vector2 mousePos = Input.mousePosition;

        //Clamp the mouse position to the screen size. It shouldn't be as larger than the screen size.
        //Stops values over 1
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);

        //Init rot delta
        Vector2 mapDelta = Vector2.zero;

        float width_section = Screen.width / width_section_size;
        float height_section = Screen.height / height_section_size;

        //Bound the screen so delta is modified accordingly.
        //MapDElta y is y position, which uses y mouse position.
        if (mousePos.x <= width_section)
        {
            mapDelta.x = (1 - mousePos.x / width_section);
        }
        else if (mousePos.x >= width_section * (width_section_size - 1))
        {
            mapDelta.x = (1 - ((mousePos.x - (width_section * (width_section_size - 2))) / width_section));
        }

        if (mousePos.y <= height_section)
        {
            mapDelta.y = (1 - mousePos.y / height_section);
        }
        else if (mousePos.y >= height_section * (height_section_size - 1))
        {
            mapDelta.y = (1 - ((mousePos.y - (height_section * (height_section_size - 2))) / height_section));
        }

        //Set current position to mapDelta
        cur_position += mapDelta;

        //Apply cur_position
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = cur_position;
        }
    }
}
