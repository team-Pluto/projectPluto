using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object for all text blocks in a dialogue system.
/// All special types of blocks derive from this basic block.
/// Basic block just has text and nothing else.
/// Never create a textblock. Use dialogue or decision blocks instead.
/// DialogueManager will not play nice with just a textblock.
/// </summary>
public class TextBlock : ScriptableObject, ICloneable
{
    /// <summary>
    /// The text that will be displayed on being called.
    /// </summary>
    [SerializeField]
    private string text;

    /// <summary>
    /// The key that is used in the dictionary.
    /// </summary>
    [SerializeField]
    private string key;

    /// <summary>
    /// List of flags that will be set to the shown values in this list when this textblock is reached.
    /// If this list is empty this textblock has no repercussions. (spelling?)
    /// </summary>
    [SerializeField]
    private List<FlagSet> flagSetList;

    /// <summary>
    /// Setter function for text.
    /// </summary>
    /// <param name="in_text"></param>
    public void SetText(string in_text)
    {
        text = in_text;
    }

    /// <summary>
    /// Getter function for text.
    /// </summary>
    /// <returns></returns>
    public string GetText()
    {
        return text;
    }

    /// <summary>
    /// Setter function for key.
    /// </summary>
    /// <param name="in_key"></param>
    public void SetKey(string in_key)
    {
        key = in_key;
    }

    /// <summary>
    /// Getter function for key.
    /// </summary>
    /// <returns></returns>
    public string GetKey()
    {
        return key;
    }

    /// <summary>
    /// Getter function for FlagSetList
    /// </summary>
    /// <returns></returns>
    public List<FlagSet> GetFlagSetList()
    {
        return flagSetList;
    }

    /// <summary>
    /// Setter function for FlagSetList.
    /// </summary>
    /// <param name="in_flagSetList"></param>
    public void SetFlagSetList(List<FlagSet> in_flagSetList)
    {
        flagSetList = in_flagSetList;
    }

    /// <summary>
    /// Function needed to allow the cloning of this object
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

/// <summary>
/// FlagSet class is so that we can easily create a list that shows the a flag that will be set to a given value, if this textblock is reached.
/// </summary>
[System.Serializable]
public class FlagSet
{
    /// <summary>
    /// The flag key that is going to be set.
    /// </summary>
    public string flag_key;

    /// <summary>
    /// The flag val that will be set upon reading this text.
    /// </summary>
    public bool flag_val;
}
