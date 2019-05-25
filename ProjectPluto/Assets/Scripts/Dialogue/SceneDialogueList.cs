using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// A non singleton data structure that stores all the starting dialogue blocks we need in this scene.
/// </summary>
public class SceneDialogueList : MonoBehaviour
{
    /// <summary>
    /// List of all starting dialogue blocks only in this scene.
    /// Private because we never want this editted at run time.
    /// </summary>
    [SerializeField]
    public Dictionary<string, TextBlock> sceneDialogueDict = new Dictionary<string, TextBlock>();

    private void Start()
    {
        PopulateSceneDialogueList();
    }

    /// <summary>
    /// Function that will be called on start to populate it from the text file.
    /// </summary>
    public void PopulateSceneDialogueList()
    {
        //First find the scene name.
        string sceneName = SceneManager.GetActiveScene().name;

        //Load all the blocks from the text
        StreamReader streamReader = new StreamReader("Assets/Resources/DialogueSystem/SceneLists/" + sceneName + ".txt");

        string line = streamReader.ReadLine();
        while(line != null)
        {
            //Split the line by comma
            string[] split_line = line.Split(","[0]);
            string key = split_line[0];
            TextBlock block = Resources.Load(split_line[1]) as TextBlock;
            sceneDialogueDict.Add(key, block);
            line = streamReader.ReadLine();
        }
    }

    /// <summary>
    /// Getter function.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, TextBlock> GetSceneDialogueList()
    {
        return sceneDialogueDict;
    }

    /// <summary>
    /// Setter function.
    /// </summary>
    /// <param name="in_sceneDialogueList"></param>
    /// <returns></returns>
    public void SetSceneDialogueList(Dictionary<string, TextBlock> in_sceneDialogueDict)
    {
        sceneDialogueDict = DictionaryUtil.CloneDictionaryCloningValues(in_sceneDialogueDict);
    }
}
