using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the mouse cursor according to certain situations
/// </summary>
public class PanAdaptiveCursor : MonoBehaviour
{
    public Texture2D normalTexture, screenTexture;

    RaycastHit rayHit = new RaycastHit();

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse position
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out rayHit, 1000f))
        {
            if (rayHit.collider.CompareTag("Screen"))
            {
                Cursor.SetCursor(screenTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(normalTexture, Vector2.zero, CursorMode.Auto);
            }
        }
        else //Default to normal cursor if nothing is hit.
        {
            Cursor.SetCursor(normalTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}
