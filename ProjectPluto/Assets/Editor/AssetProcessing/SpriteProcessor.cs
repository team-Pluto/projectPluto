using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Following this unity tutorial: https://unity3d.com/learn/tutorials/topics/scripting/creating-basic-editor-tools?playlist=17117
//Inheriting from a special script from UnityEditor namespace
public class SpriteProcessor : AssetPostprocessor
{
    //Using preprocess instead of postprocess
    void OnPreprocessTexture()
    {
        //Get the asset path, and make it all lower case.
        //assetPath is coming from AssetPostprocessor
        string lowerCaseAssetPath = assetPath.ToLower();

        //Check to see if the file is in the folder sprites. (We're technically checking if there exists a substring "/sprites/", so any folders containg sprites will return true.)
        //IndexOf returns -1 when there are no matches, else some real number, which will be considered true.
        bool isInSpritesDirectory = lowerCaseAssetPath.IndexOf("/sprites/") != -1;

        //If the texture is in the sprites directory
        if (isInSpritesDirectory)
        {
            //Get the asset importer, cast to textureImporter
            TextureImporter textureImporter = (TextureImporter)assetImporter;

            //Change the texture importer to sprite type
            textureImporter.textureType = TextureImporterType.Sprite;

            //Make sure transparency is turned on
            textureImporter.alphaIsTransparency = true;
        }
    }
}
