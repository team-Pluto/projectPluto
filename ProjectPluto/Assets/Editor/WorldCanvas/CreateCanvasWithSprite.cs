using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Creates a world canvas object for every asset selected. Fills each one with a sprite.
/// </summary>
public class CreateCanvasWithSprite : MonoBehaviour
{
    [MenuItem("WorldCanvas/Create/Sprite/From Selected")]
    public static GameObject[] CreateSpriteCanvas()
    {
        //Init list of all objs
        GameObject[] objs = new GameObject[Selection.assetGUIDs.Length];
        int index = 0;

        //If there is more than one 
        foreach (string guid in Selection.assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
            if (sprite == null)
            {
                throw new System.Exception("Error: Selected asset is not a sprite. Cannot create a WorldCanvas with a sprite. Please select valid sprites.");
            }

            //Create an empty sprite (like above)
            GameObject obj = CreateCanvasEmptySprite.CreateEmptySprite();
            objs[index] = obj;
            Transform img = obj.transform.GetChild(0);

            img.GetComponent<Image>().sprite = sprite;
            obj.name = sprite.name;

            //Increment index
            index++;
        }

        //Only attempt match if we managed to do something
        if (objs.Length > 0)
        {
            //Match all objs
            MatchSpriteAndCanvas.MatchObjects(objs);
            return objs;
        }
        else
        {
            throw new System.Exception("Error: You don't seem to have selected any valid sprites to make WorldCanvas objects.");
        }
    }
}
