using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DecisionBlock differs from a basic TextBlock because it outputs decisions for the player to make that then
/// influences what block comes after, instead of relying on flags set in past events in the game.
/// </summary>
[CreateAssetMenu(fileName = "DecisionBlock", menuName = "Block/DecisionBlock", order = 3)]
public class DecisionBlock : TextBlock
{
    /// <summary>
    /// List of the blocks that can come after this block.
    /// </summary>
    [SerializeField]
    private List<DecisionInfo> nextBlocks;

    /// <summary>
    /// Getter function for the next blocks.
    /// </summary>
    /// <returns></returns>
    public List<DecisionInfo> GetDecisionInfo()
    {
        return nextBlocks;
    }

    /// <summary>
    /// Setter function for decision info list.
    /// </summary>
    /// <param name="in_nextBlocks"></param>
    public void SetNextList(List<DecisionInfo> in_nextBlocks)
    {
        nextBlocks = in_nextBlocks;
    }
    
    /// <summary>
    /// Helper function that creates a decision block with the name key at the given path.
    /// </summary>
    /// <param name="in_key"></param>
    /// <param name="in_path"></param>
    public static DecisionBlock CreateDecisionBlock(string in_key, string in_path)
    {
        DecisionBlock db = ScriptableObjectUtility.CreateAsset<DecisionBlock>(in_path, in_key);
        db.SetKey(in_key);
        return db;
    }

    /// <summary>
    /// Helper function that sets all paramters in this decision block.
    /// </summary>
    /// <param name="in_text"></param>
    /// <param name="in_flagSetList"></param>
    /// <param name="in_next_list"></param>
    public void SetDecisionBlock(string in_text,
        List<FlagSet> in_flagSetList, List<DecisionInfo> in_next_list)
    {
        SetText(in_text);
        SetFlagSetList(in_flagSetList);
        SetNextList(in_next_list);
    }
}

[System.Serializable]
public class DecisionInfo
{
    /// <summary>
    /// The next block that results in choosing this decision.
    /// </summary>
    [SerializeField]
    private TextBlock next_block;

    /// <summary>
    /// The decision that is displayed for the player to interpret and choose.
    /// </summary>
    [SerializeField]
    private string decision;

    /// <summary>
    /// Getter function for next block.
    /// </summary>
    /// <returns></returns>
    public TextBlock GetNextBlock()
    {
        return next_block;
    }

    /// <summary>
    /// Setter for the next block.
    /// </summary>
    /// <param name="in_next_block"></param>
    public void SetNextBlock(TextBlock in_next_block)
    {
        next_block = in_next_block;
    }

    /// <summary>
    /// Getter function for decision text.
    /// </summary>
    /// <returns></returns>
    public string GetDecisionText()
    {
        return decision;
    }

    /// <summary>
    /// Setter for decision text.
    /// </summary>
    /// <param name="in_text"></param>
    public void SetDecisionText(string in_text)
    {
        decision = in_text;
    }
}