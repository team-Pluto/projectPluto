using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// DialogueBlock differs from a basic TextBlock because it can have multiple outputs for a next_block,
/// depending on specific calls.
/// </summary>
[CreateAssetMenu(fileName = "DialogueBlock", menuName = "Block/DialogueBlock", order = 2)]
public class DialogueBlock : TextBlock
{
    /// <summary>
    /// List of all NextBlocks.
    /// Each nextBlock contains the TextBlock that wants to be called and
    /// the flag vector that is needed for the block to be called.
    /// </summary>
    public List<NextBlock> next_list;

    /// <summary>
    /// Retrieves the next block depending on the flags set.
    /// Returns null if there is no valid next block.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public TextBlock GetNext()
    {
        //First call the flag vector. This gets all the values for all the flags we need to check.
        List<bool> flag_vector = Toolbox.Instance.GetFlagDict().GetFlagVector(GetDependentFlags());

        //Then go through the next list and return the first block that fulfills its requirements.
        for (int i = 0; i < next_list.Count; i++)
        {
            //Get the current check we want to compare
            List<FlagCheck> cur_flag = next_list[i].flag_check_list;

            //This bool checks if the current block is valid.
            bool isValid = true;

            for (int j = 0; j < cur_flag.Count; j++)
            {
                //If our our current flag values disagree with the flags set in our flagdictionary, break
                if (cur_flag[j].GetFlagVal() == FlagCheck.flagvector_value.True && !flag_vector[j] ||
                    cur_flag[j].GetFlagVal() == FlagCheck.flagvector_value.False && flag_vector[j])
                {
                    isValid = false;
                    break;
                }
            }

            //If it is still valid, then we ran through all the values and they were equivalent. Return the next block.
            if (isValid)
            {
                return next_list[i].next_block;
            }
        }
        //If we reach this part of the code, there was no valid block.
        Debug.Log("No valid blocks. Terminate the conversation.");
        return null;
    }

    /// <summary>
    /// Helper function that returns a list of all dependentFlags that are needed to evaluate what block comes after this one.
    /// </summary>
    /// <returns></returns>
    public List<string> GetDependentFlags()
    {
        //The return list
        List<string> flag_list = new List<string>();

        //Piece together all the flag keys we need to evaluate anything in this block.
        foreach(NextBlock block in next_list)
        {
            //For all the flagChecks in this block's list
            foreach(FlagCheck flagCheck in block.flag_check_list)
            {
                //If our flag list does not contain this flag key
                if (!flag_list.Contains(flagCheck.GetFlagKey()))
                {
                    //Add the flag key to the list
                    flag_list.Add(flagCheck.GetFlagKey());
                }
            }
        }

        //Return the flag list.
        return flag_list;
    }

    /// <summary>
    /// Sets the next list to the inputted in_nextBlocks list.
    /// </summary>
    /// <param name="in_nextBlocks"></param>
    public void SetNextList(List<NextBlock> in_nextBlocks)
    {
        next_list = in_nextBlocks;
    }

    /// <summary>
    /// Helper function that just creates a dialogue block.
    /// </summary>
    /// <param name="in_key"></param>
    /// <param name="in_path"></param>
    public static DialogueBlock CreateDialogueBlock(string in_key, string in_path)
    {
        DialogueBlock db = ScriptableObjectUtility.CreateAsset<DialogueBlock>(in_path, in_key);
        db.SetKey(in_key);
        return db;
    }

    /// <summary>
    /// Helper function that sets all the parameters of this block.
    /// </summary>
    /// <param name="in_text"></param>
    /// <param name="in_flagSetList"></param>
    /// <param name="in_next_list"></param>
    public void SetDialogueBlock(string in_text,
        List<FlagSet> in_flagSetList, List<NextBlock> in_next_list)
    {
        SetText(in_text);
        SetFlagSetList(in_flagSetList);
        SetNextList(in_next_list);
    }
}

/// <summary>
/// This is an object  that stores the next block along with the flags it needs to be called upon.
/// </summary>
[System.Serializable]
public class NextBlock
{
    /// <summary>
    /// The next block that is going to be called.
    /// </summary>
    public TextBlock next_block;


    /// <summary>
    /// The flag list that needs to be equivalent for this to be called.
    /// For example, if the list is a count of three, you could have blocks
    /// that are only called when a single flag is true (1 0 0), (0 1 0), (0 0 1).
    /// And also for setups like (1 1 0), (0 1 1), or (1 1 1).
    /// Note: Now uses enum with three values. NoInfluence means it doesn't matter what the value for that flag is.
    /// This gives more flexibility.
    /// </summary>
    public List<FlagCheck> flag_check_list;
}

/// <summary>
/// Class that contains the flag key we are looking at, and what value we want for this to be good.
/// </summary>
[System.Serializable]
public class FlagCheck
{
    //The flag we want to be true or false
    [SerializeField]
    private string flag_key;

    /// <summary>
    /// Instead of booleans in our flag check vector, we need an enum.
    /// We need a value called NoInfluence that just means it doesn't matter what the value is here for the flag.
    /// For example, a block that is called if the first flag is true regardless of any other values would be:
    /// (True, NoInfluence, NoInfluence, .... , NoInfluence).
    /// </summary>
    public enum flagvector_value { NoInfluence, False, True };

    /// <summary>
    /// The flag value we want for this to be true.
    /// </summary>
    [SerializeField]
    private flagvector_value flag_val;

    /// <summary>
    /// Getter for flag key.
    /// </summary>
    /// <returns></returns>
    public string GetFlagKey()
    {
        return flag_key;
    }

    /// <summary>
    /// Setter for the flag key.
    /// </summary>
    /// <param name="in_flagKey"></param>
    public void SetFlagKey(string in_flagKey)
    {
        flag_key = in_flagKey;
    }

    /// <summary>
    /// Getter for the flag val.
    /// </summary>
    /// <returns></returns>
    public flagvector_value GetFlagVal()
    {
        return flag_val;
    }

    /// <summary>
    /// Setter for the flag val.
    /// </summary>
    /// <param name="in_val"></param>
    public void SetFlagVal(flagvector_value in_val)
    {
        flag_val = in_val;
    }
}
