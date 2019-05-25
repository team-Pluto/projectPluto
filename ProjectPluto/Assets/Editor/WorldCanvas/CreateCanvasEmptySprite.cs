using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Creates a world canvas object with a sprite child, but doesn't fill in the sprite.
/// </summary>
public class CreateCanvasEmptySprite : MonoBehaviour
{
    /// <summary>
    /// Menu function that creates a worldCanvas with an empty sprite child.
    /// </summary>
    /// <returns></returns>
    [MenuItem("Create/WorldCanvas/Sprite/Empty")]
    public static GameObject CreateEmptySprite()
    {
        //Create a world canvas like above
        GameObject obj = CreateWorldCanvas.CreateCanvas();

        //Create a child UI object called sprite.
        //Make it match the canvas
        GameObject img = new GameObject("Sprite");
        img.transform.parent = obj.transform;
        img.transform.localPosition = Vector3.zero;
        img.transform.localRotation = Quaternion.identity;
        img.transform.localScale = Vector3.one;
        img.AddComponent<Image>();

        return obj;
    }
}
