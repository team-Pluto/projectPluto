using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Allows for scriptable objects to be created programmatically.
/// Code found here: http://wiki.unity3d.com/index.php/CreateScriptableObjectAsset
/// </summary>
public static class ScriptableObjectUtility
{
    /// <summary>
    ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// Modified the code so it returns the asset it created for further modification.
    /// </summary>
    public static T CreateAsset<T>(string path, string name) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return asset;
    }
}