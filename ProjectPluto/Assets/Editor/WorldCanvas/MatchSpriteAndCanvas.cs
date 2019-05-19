using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//Matches the sizes for both sprite and canvas so they are 1-to-1
public class MatchSpriteAndCanvas : MonoBehaviour
{
    /// <summary>
    /// Helpful function that will match the scales and sizes of our sprites and canvases to be one to one.
    /// Can be called from the menu
    /// </summary>
    [MenuItem("Modify/Match")]
    public static void Match()
    {
        //Check the selected game objects
        GameObject[] objs = Selection.gameObjects;

        MatchObjects(objs);
    }

    /// <summary>
    /// Helper function that can be called from other scripts.
    /// </summary>
    /// <param name="objs"></param>
    public static void MatchObjects(GameObject[] objs)
    {
        //Check that all are valid
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<Canvas>() == null || obj.GetComponentInChildren<Image>() == null)
            {
                throw new System.Exception("Error: Can't match an object that doesn't have a Canvas component and Image component. Make sure all selected objects have those properties.");
            }
            else if (obj.GetComponentInChildren<Image>().sprite == null)
            {
                throw new System.Exception("Error: Can't match an object that doesn't have a sprite in its Image component. Make sure all selected objects has a sprite.");
            }
        }

        //Now process all of them
        foreach (GameObject obj in objs)
        {
            //Get dimensions of the sprite in this canvas
            Vector2 dimensions = new Vector2(obj.GetComponentInChildren<Image>().sprite.texture.width, obj.GetComponentInChildren<Image>().sprite.texture.height);

            //Set the canvas size to be one to one with the sprite
            obj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dimensions.x);
            obj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dimensions.y);

            //Get the sprite
            Transform imgTransform = obj.transform.GetChild(0);

            //Set the sprite dimensions to be one to one with the canvas.
            imgTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dimensions.x);
            imgTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dimensions.y);
        }
    }
}
