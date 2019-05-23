using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Creates parallax object from selected sprites.
/// </summary>
public class CreateParallaxObj : MonoBehaviour
{
    [MenuItem("Create/ParallaxObject/From Selected")]
    public static void CreateParallaxObject()
    {
        //Create a new world canvas with sprite object under the main one
        //Run this first so if there are issues we don't run anything else.
        GameObject[] all_sprites = CreateCanvasWithSprite.CreateSpriteCanvas();

        //Create an empty game object to be our Parallax Object
        GameObject par_object = new GameObject();
        par_object.name = "Parallax Object";

        //Add the component ParallaxSprites
        par_object.AddComponent<ParallaxSprites>();

        //Add the camera to relativeTo so we don't have to call Camera.main at runtime
        if(Camera.main != null)
        {
            par_object.GetComponent<ParallaxSprites>().relativeTo = Camera.main;
        }

        //Init list of all objs. Empty for now, but needs to be the right length.
        Transform[] sprite_layers = new Transform[Selection.assetGUIDs.Length];

        //For all sprites
        int index = 0;
        foreach (GameObject img in all_sprites)
        {
            //Put temporary distance so it doesn't look wrong in editor
            img.transform.localPosition = new Vector3(0, 0, index * 0.1f);

            //Flip the sprites, we need them to be oppsite the forward of par_object
            img.transform.localEulerAngles = new Vector3(0, 180, 0);

            //Get the number in this sprite's name to place in the array.
            string[] split_string = img.name.Split('_');

            //Using minus one so that the base layer can be layer 1
            int extracted_index = int.Parse(split_string[split_string.Length - 1]) - 1;

            //NOT SURE IF THIS WORKS. THIS IS MEANT TO CHECK THAT WE CAN EXTRACT SOME NUMBER.
            //IF IT IS INVALID WE WANT TO THROW AN ERROR
            if(extracted_index == null)
            {
                //Since it is invalid, destroy all objects that we wanted to use.
                Destroy(par_object);
                for(int i = 0; i < all_sprites.Length; i++)
                {
                    Destroy(all_sprites[0]);
                }
            }

            //Save the sprite's transforms
            sprite_layers[extracted_index] = img.transform;

            index++;
        }

        //For every sprite, since it is now ordered...
        index = 0;
        foreach(Transform sprite in sprite_layers)
        {
            sprite.parent = par_object.transform;
            sprite.SetSiblingIndex(index);
            sprite.name = "Layer" + index;
            index++;
        }
    }
}
