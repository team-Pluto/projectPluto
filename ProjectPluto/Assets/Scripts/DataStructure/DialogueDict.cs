using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure that contains all of the dialogue blocks that we need in this scene.
/// </summary>
public class DialogueDict : MonoBehaviour
{
    /// <summary>
    /// This is the flag dictionary. 
    /// Key is a string, value is a bool.
    /// </summary>
    private Dictionary<string, TextBlock> dialogueDict = new Dictionary<string, TextBlock>();

    /// <summary>
    /// Function that adds a block to the text block.
    /// </summary>
    /// <param name="block"></param>
    public void AddBlock(TextBlock block)
    {
        dialogueDict.Add(block.GetKey(), block);
    }

    /// <summary>
    /// Function that gets a block given a key from our dialogueDict.
    /// </summary>
    /// <param name="key"></param>
    public TextBlock GetBlock(string key)
    {
        if (dialogueDict.ContainsKey(key))
        {
            return dialogueDict[key];
        }
        else
        {
            Debug.Log(dialogueDict.Count);
            throw new System.Exception("Error, key: " + key + " does not exist in the dialogue dictionary.");
        }
       
    }

    /// <summary>
    /// Function that clears the dialogueDict. Used for loading between scenes.
    /// </summary>
    public void ClearDialogueDict()
    {
        dialogueDict.Clear();
    }
}
