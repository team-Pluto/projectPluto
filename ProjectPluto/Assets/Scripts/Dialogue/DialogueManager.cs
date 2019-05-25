using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// DialogueManager uses the dialogueblock system to transfer it to the screen.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    //The textbox where we display the text.
    public Text textbox;

    //The textblock we are currently processing.
    private TextBlock cur_block;

    //The decision bar object
    public Transform decisionBar;

    //Key used to continue. Change out later so it can be configured.
    public KeyCode continue_key;

    //The dialogue manager states.
    // Sleeping : When the player isn't engaging with the dialogue system.
    // Init: When the player first engages with dialogue.
    // Displaying: When the system is displaying the text. State exists in case we want to build in some effects for how it comes out.
    // WaitingText: When the system is done displaying and can move on to the next block.
    // WaitingDecision: When the system is done displaying and is waiting on a decision to move on to next block.
    public enum DM_STATE { Sleeping, Init, Displaying, WaitingText, WaitingDecision };
    public DM_STATE cur_state = DM_STATE.Sleeping;

    // Update is called once per frame
    void Update()
    {
        //State machine.
        switch (cur_state)
        {
            case DM_STATE.Sleeping:
                HandleSleeping();
                break;
            case DM_STATE.Init:
                HandleInit();
                break;
            case DM_STATE.Displaying:
                HandleDisplaying();
                break;
            case DM_STATE.WaitingText:
                HandleWaitingText();
                break;
            case DM_STATE.WaitingDecision:
                HandleWaitingDecision();
                break;
        }
    }

    /// <summary>
    /// Function call that will start dialogue given a key to the first dialogue block in that conversation.
    /// </summary>
    /// <param name="key"></param>
    public void StartDialogue(string key)
    {
        //Set the state to init.
        SetState(DM_STATE.Init);

        //Set the cur_block
        cur_block = GetBlock(key);
    }

    /// <summary>
    /// Helper function to handle state Sleeping.
    /// </summary>
    void HandleSleeping()
    {
        //Filler code, should instead be waiting for a call.
        if (Input.GetKeyDown(continue_key))
        {
            StartDialogue("0");
        }
    }

    /// <summary>
    /// Helper function to handle state Init.
    /// </summary>
    void HandleInit()
    {
        //Default don't do anything in this init state. Just move to displaying.
        SetState(DM_STATE.Displaying);
    }

    /// <summary>
    /// Helper function to handle state Displaying.
    /// </summary>
    void HandleDisplaying()
    {
        //Set the textbox to the right text.
        textbox.text = cur_block.GetText();

        //Check to see if there are any flags we need to set since we have read this textblock.
        List<FlagSet> flagsToSet = cur_block.GetFlagSetList();

        //If there are any in the list at all
        if (flagsToSet.Count > 0)
        {
            //For every flagSet, set the proper flags.
            foreach (FlagSet flagSet in flagsToSet)
            {
                Toolbox.Instance.GetFlagDict().SetFlag(flagSet.flag_key, flagSet.flag_val);
            }
        }

        //Set the state depending on the type of block.
        if (cur_block.GetType().Equals(typeof(DialogueBlock)))
        {
            SetState(DM_STATE.WaitingText);
        }//If the state is a decision block, display those decisions now!
        else if (cur_block.GetType().Equals(typeof(DecisionBlock)))
        {
            PopulateDecisionBar((DecisionBlock)cur_block);
            SetState(DM_STATE.WaitingDecision);
        }
        else
        {
            //If neither of the two states occur then we have the wrong block type occuring.
            throw new System.Exception("Error, the block was of type: " + cur_block.GetType() + " instead of type DialogueBlcok or DecisionBlock.");
        }
    }

    /// <summary>
    /// Helper function to handle state WaitingText.
    /// </summary>
    void HandleWaitingText()
    {
        //If we're waiting on text, on the continue key we need to move on to the next correct block.
        //If there is no next  block, exit dialogue.
        if (Input.GetKeyDown(continue_key))
        {
            //Need to cast the block since we normally store it as TextBlock.
            DialogueBlock DB = (DialogueBlock)cur_block;

            //Get the next block. Store in memory as temp.
            TextBlock tempBlock = DB.GetNext();

            //If we there is no next block, exit dialogue.
            if (tempBlock == null)
            {
                cur_block = null;
                textbox.text = "";
                SetState(DM_STATE.Sleeping);
            }
            else //If there is a valid next block, set it and init once again.
            {
                cur_block = tempBlock;
                SetState(DM_STATE.Init);
            }
        }
    }

    /// <summary>
    /// Helper function to handle state WaitingDecision
    /// </summary>
    void HandleWaitingDecision()
    {
        //Do nothing. Wait until a button is clicked.
    }

    /// <summary>
    /// Helper function that populates the decision bar given a decision block
    /// </summary>
    void PopulateDecisionBar(DecisionBlock decisionBlock)
    {
        //Enable the decision bar, which is by default not there when there are no decisions.
        decisionBar.gameObject.SetActive(true);

        //Load the decision button prefab
        GameObject decisionButton = Resources.Load("DialogueSystem/Prefab/DecisionButton") as GameObject;

        //For every decision in our decisionInfo list on this block
        foreach(DecisionInfo info in decisionBlock.GetDecisionInfo())
        {
            //Create a decision button
            GameObject cur_db = Instantiate(decisionButton, decisionBar);

            //Add the right function call to the button
            cur_db.GetComponent<Button>().onClick.AddListener(() => ChooseDecision(info));

            //Populate the text on the button
            cur_db.GetComponentInChildren<Text>().text = info.GetDecisionText();

            //Enable the object so it shows up in the bar.
            cur_db.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Function that chooses a decision when called. 
    /// Handles changing states.
    /// </summary>
    /// <param name="info"></param>
    public void ChooseDecision(DecisionInfo info)
    {
        //Set the decision bar back to inactive
        decisionBar.gameObject.SetActive(false);

        //Clear the decision bar.
        for(int i = decisionBar.childCount - 1; i >= 0; i--)
        {
            Destroy(decisionBar.GetChild(i).gameObject);
        }

        //Set the next block, and set onto the next init state.
        cur_block = info.GetNextBlock();
        SetState(DM_STATE.Init);
    }

    /// <summary>
    /// Helper function that gets a block from the dialogue dictionary.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    TextBlock GetBlock(string key)
    {
        //Get the dialogue dictionary.
        Dictionary<string, TextBlock> dialogueDict = GameObject.Find("SceneManager").GetComponent<SceneDialogueList>().GetSceneDialogueList();
        if (dialogueDict.ContainsKey(key))
        {
            return dialogueDict[key];
        }
        else
        {
            throw new System.Exception("Error, dialogue dictionary in scene manager doesn't contain key: " + key + ". Make sure to load everything properly.");
        }
        
    }

    //Helper function that sets our state to the inputted state.
    void SetState(DM_STATE STATE)
    {
        cur_state = STATE;
    }
}
