using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// Menu function that creates a  worldCanvas with the right scale we want. 
/// </summary>
public class CreateWorldCanvas : ScriptableWizard
{
    /// <summary>
    /// Menu function that creates a worldCanvas with nothing at all.
    /// </summary>
    /// <returns></returns>
    [MenuItem("Create/WorldCanvas/Empty")]
    public static GameObject CreateCanvas()
    {
        GameObject obj = new GameObject("WorldCanvas");

        //Add necessary components
        obj.AddComponent<Canvas>();
        obj.AddComponent<CanvasScaler>();
        obj.AddComponent<GraphicRaycaster>();

        //Set some canvas settings
        Canvas canvas = obj.GetComponent<Canvas>();

        //Change to world space
        canvas.renderMode = RenderMode.WorldSpace;

        //Set scale
        canvas.GetComponent<RectTransform>().localScale = new Vector3(0.001f, 0.001f, 0.001f);

        return obj;
    }
}
