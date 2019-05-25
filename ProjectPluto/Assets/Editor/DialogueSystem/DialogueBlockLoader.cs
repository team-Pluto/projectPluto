using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Utilty script that can be called on to load in all dialogue in a csv and make dialogue blocks for our system to use.
/// </summary>
public class DialogueBlockLoader : ScriptableWizard
{
    //Path where we load in the csv.
    static string path = "Assets/Data/";

    /// <summary>
    /// Dictionary where the key is the scene name and the value is another dictionary which holds all the blocks in a given scene.
    /// </summary>
    static Dictionary<string, Dictionary<string, TextBlock>> AllSceneBlockDictionary 
        = new Dictionary<string, Dictionary<string, TextBlock>>();

    //Dictionary of all blocks in a given scene associated with their keys.
    static Dictionary<string, TextBlock> SceneBlockDictionary;

    /// <summary>
    /// Function that loads dialogue sheets and reinits flags.
    /// </summary>
    [MenuItem("DialogueSystem/Reload all flags and blocks")]
    static void LoadEverything()
    {
        //Clear the dictionary completely.
        AllSceneBlockDictionary.Clear();

        //List of all scene names
        List<string> sceneList = new List<string>();

        string[] pathArray = Directory.GetFiles("Assets/Scenes");
        for(int i = 0; i < pathArray.Length; i++)
        {
            //Trim the string so it's just the file name
            string[] splitString = pathArray[i].Split("\\"[0]);
            string trimmed_name = splitString[splitString.Length - 1];

            //If it is a unity file AND it isn't a meta file.
            if (trimmed_name.Contains("unity") && !trimmed_name.Contains("meta"))
            {
                trimmed_name = trimmed_name.Split("."[0])[0];
                sceneList.Add(trimmed_name);
                
            } //Only throw an error if it isn't a meta file.
            else if(!trimmed_name.Contains("meta"))
            {
                throw new System.Exception("Error, a file in the scenes folder is not a scene file: " + 
                    trimmed_name + "\nNOTE: meta files are ignored so they can be here.");
            }            
        }

        //Load dialogue sheets for all files.
        foreach(string fileName in sceneList)
        {
            LoadDialogueSheet(fileName);
        }

        //Init flag file with the list
        InitFlagFile(sceneList);

        //Populate all scene managers.
        InitSceneBlockList(sceneList);

        Debug.Log("Completed loading everything.");
    }

    /// <summary>
    /// Function that loads in dialogue sheet of given FileName.
    /// FileName should be equivalent to scene name.
    /// </summary>
    static void LoadDialogueSheet(string FileName)
    {
        //Read in all text from the given file (csv) and split by line.
        string fileData = System.IO.File.ReadAllText(path + FileName + ".csv");
        string[] lines = fileData.Split("\n"[0]);

        string blockDestinationPath = "Assets/Resources/DialogueSystem/DialogueBlocks/" + FileName;

        //First step is create all the blocks we need. We need to do this first 
        //since we won't be able to point to other blocks if they don't exist yet.
        PopulateDictionaryAndFolders(lines, blockDestinationPath, FileName);

        //Go through the text again, but set block values this time.
        SetAllBlocks(lines);
    }

    /// <summary>
    /// Populates the flag dictinoary with all the flags found in our excel sheet.
    /// </summary>
    static void InitFlagFile(List<string> fileNameList)
    {
        //Get the flag dict
        List<string> flagList = new List<string>();

        foreach(string FileName in fileNameList)
        {
            //Read in all text from the given file (csv) and split by line.
            string fileData = System.IO.File.ReadAllText(path + FileName + ".csv");
            string[] lines = fileData.Split("\n"[0]);

            //This ignore bool is for the first line, which are just headers.
            bool ignore = true;
            foreach (string line in lines)
            {
                //Ignore first line.
                if (ignore)
                {
                    ignore = false;
                }
                else if (!ignore)
                {
                    //Separate by commas. (CSV)
                    string[] lineData = (line.Trim()).Split(","[0]);

                    //This is the type of block this is.
                    string key = lineData[0];
                    string type = lineData[1];

                    if (SceneBlockDictionary.ContainsKey(key))
                    {
                        //First process the flag set list.
                        List<FlagSet> flagSetList = SceneBlockDictionary[key].GetFlagSetList();

                        //Loop through all flags in flag set list.
                        foreach (FlagSet fs in flagSetList)
                        {
                            if (!flagList.Contains(fs.flag_key))
                            {
                                flagList.Add(fs.flag_key);
                            }
                        }


                        //Then check for further flags found in the specific block type.
                        if (type.Equals("DialogueBlock"))
                        {
                            DialogueBlock db = (DialogueBlock)SceneBlockDictionary[key];
                            foreach (string flag in db.GetDependentFlags())
                            {
                                //If we don't have this flag in the list yet, add it.
                                if (!flagList.Contains(flag))
                                {
                                    flagList.Add(flag);
                                }
                            }
                        }
                        else if (type.Equals("DecisionBlock"))
                        {
                            DecisionBlock db = (DecisionBlock)SceneBlockDictionary[key];
                        }
                    }
                    else
                    {
                        throw new System.Exception("Error, given key: " + key + " from file: + " + FileName+ " isn't in our dictionary. Reload the csv to populate it.");
                    }
                }
            }
        }

        //Path that we are going to write to.
        string file_path = "Assets/Data/FlagData.txt";

        //Write some text to the test.txt
        StreamWriter writer = new StreamWriter(file_path, false);

        foreach (string flag in flagList)
        {
            writer.WriteLine(flag + ", false");
        }
        writer.Close();

        Debug.Log("Finished. Flag file initialized at: " + file_path);
    }

    /// <summary>
    /// Inits the list of blocks for a all the given scenes.
    /// </summary>
    static void InitSceneBlockList(List<string> fileNameList)
    {
        //Clear the folder if it exists, or create it if it doesn't.
        string folderPath = "Assets/Resources/DialogueSystem/SceneLists";
        ClearFolder(folderPath);

        //For each file name in this list.
        foreach(string fileName in fileNameList)
        {
            //The path we want to write to.
            string path = folderPath + "/" + fileName + ".txt";

            //Write to a file in the given path.
            StreamWriter streamWriter = new StreamWriter(path, false);

            //If the all scene block dictionary contains that key
            if (AllSceneBlockDictionary.ContainsKey(fileName))
            {
                //For each pair in the dictionary.
                foreach (KeyValuePair<string, TextBlock> entry in AllSceneBlockDictionary[fileName])
                {
                    string key = entry.Key;
                    string pathToBlock = "DialogueSystem/DialogueBlocks/" + fileName + "/" + key;
                    streamWriter.WriteLine(key + "," + pathToBlock);
                }
            }
            streamWriter.Close();
        }
        Debug.Log("Completed writing scene block lists.");
    }

    /// <summary>
    /// Helper function that clears the folder so we can repopulate it.
    /// </summary>
    /// <param name="path"></param>
    static void ClearFolder(string path)
    {
        if (Directory.Exists(path)) { Directory.Delete(path, true); }
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Helper function that sets all block values given the text in lines.
    /// </summary>
    /// <param name="lines"></param>
    static void SetAllBlocks(string[] lines)
    {
        bool ignore = true;
        foreach (string line in lines)
        {
            //Ignore first line.
            if (ignore)
            {
                ignore = false;
            }
            else if (!ignore)
            {
                //Separate by commas. (CSV)
                string[] lineData = (line.Trim()).Split(","[0]);

                //This is the type of block this is.
                string type = lineData[1];

                if (type.Equals("DialogueBlock"))
                {
                    SetDialogueBlock(lineData);
                }
                else if (type.Equals("DecisionBlock"))
                {
                    SetDecisionBlock(lineData);
                }
            }
        }
    }

    /// <summary>
    /// Populates our dictionary with all the blocks we need and makes the objects in the folders.
    /// </summary>
    /// <param name="lines"></param>
    static void PopulateDictionaryAndFolders(string[] lines, string path, string sceneName)
    {
        //Clear the directory
        ClearFolder(path);

        //Refresh the dictionary.
        SceneBlockDictionary = new Dictionary<string, TextBlock>();

        //This ignore bool is for the first line, which are just headers.
        bool ignore = true;
        foreach (string line in lines)
        {
            //Ignore first line.
            if (ignore)
            {
                ignore = false;
            }
            else if (!ignore)
            {
                //Separate by commas. (CSV)
                string[] lineData = (line.Trim()).Split(","[0]);

                //This is the type of block this is.
                string key = lineData[0];
                string type = lineData[1];

                if (type.Equals("DialogueBlock"))
                {
                    DialogueBlock db = DialogueBlock.CreateDialogueBlock(key, path);
                    SceneBlockDictionary.Add(key, (TextBlock)db);
                }
                else if (type.Equals("DecisionBlock"))
                {
                    DecisionBlock db = DecisionBlock.CreateDecisionBlock(key, path);
                    SceneBlockDictionary.Add(key, (TextBlock)db);
                }
            }
        }

        //Store this scenes block dictionary into the scene.
        AllSceneBlockDictionary.Add(sceneName, SceneBlockDictionary);
    }

    /// <summary>
    /// Helper function that sets a dialogue block given some line data.
    /// </summary>
    /// <param name="lineData"></param>
    static void SetDialogueBlock(string[] lineData)
    {
        //Extract the data we need.
        string key = lineData[0];
        string text = lineData[2];
        string flagSetListText = lineData[3];
        string nextBlockListText = lineData[4];

        //Use helper function to parse flag set list.
        List<FlagSet> flagSetList = ParseFlagSetList(flagSetListText);

        //Use helper function to parse next block list.
        List<NextBlock> nextBlockList = ParseNextBlockList(nextBlockListText);

        DialogueBlock block = (DialogueBlock)SceneBlockDictionary[key];
        block.SetDialogueBlock(text, flagSetList, nextBlockList);
    }

    /// <summary>
    /// Helper function that sets a decision block given some line data.
    /// </summary>
    /// <param name="lineData"></param>
    static void SetDecisionBlock(string[] lineData)
    {
        //Extract the data we need.
        string key = lineData[0];
        string text = lineData[2];
        string flagSetListText = lineData[3];
        string decisionInfoListText = lineData[4];

        //Use helper function to parse flag set list.
        List<FlagSet> flagSetList = ParseFlagSetList(flagSetListText);

        //Use helper function to parse next block list.
        List<DecisionInfo> decisionInfoList = ParseDecisioninfoList(decisionInfoListText);

        DecisionBlock block = (DecisionBlock)SceneBlockDictionary[key];
        block.SetDecisionBlock(text, flagSetList, decisionInfoList);
    }

    /// <summary>
    /// Helper function that parses the block of text that is the nextBlockList text.
    /// </summary>
    /// <param name="nextBlockListText"></param>
    /// <returns></returns>
    static List<NextBlock> ParseNextBlockList(string nextBlockListText)
    {
        //Create the list. 
        List<NextBlock> nextBlockList = new List<NextBlock>();

        //Split text
        string[] nextBlockData = (nextBlockListText.Trim()).Split(":"[0]);

        //If the nextBlockData length is less than 1, then it is empty.
        if(nextBlockData.Length > 1)
        {
            for (int i = 0; i < nextBlockData.Length; i++)
            {
                //Pull the whole entry in the nextblock list. Starts with [ ends with ].
                List<string> thisBlockList = new List<string>();
                thisBlockList.Add(nextBlockData[i]);
                do
                {
                    i++;
                    if (i > nextBlockData.Length)
                    {
                        throw new System.Exception("Error, somehow the csv didn't finish with a ']'. The line was: " + nextBlockData);
                    }
                    thisBlockList.Add(nextBlockData[i]);

                } while (!nextBlockData[i].Contains("]"));

                //Remove punctuation from the text. while loading them
                string block_key = Regex.Replace(thisBlockList[0], "[^\\w\\._]", "");

                //Flag check list.
                List<FlagCheck> flagCheckList = new List<FlagCheck>();

                //Loop over all the flags that need to be saved. FIrst part is just the block_key.
                for(int j = 1; j < thisBlockList.Count; j++)
                {
                    FlagCheck fc = new FlagCheck();
                    fc.SetFlagKey(Regex.Replace(thisBlockList[j], "[^\\w\\._]", ""));
                    string flag_val = Regex.Replace(thisBlockList[j + 1], "[^\\w\\._]", "");

                    j++;
                    if(flag_val.Equals("True") || flag_val.Equals("true"))
                    {
                        fc.SetFlagVal(FlagCheck.flagvector_value.True);
                    }
                    else if(flag_val.Equals("False") || flag_val.Equals("false"))
                    {
                        fc.SetFlagVal(FlagCheck.flagvector_value.False);
                    }
                    flagCheckList.Add(fc);
                }
             
                //Put it into the data structure.
                NextBlock nb = new NextBlock();
                nb.next_block = SceneBlockDictionary[block_key];
                nb.flag_check_list = flagCheckList;
                nextBlockList.Add(nb);
            }
        }
        return nextBlockList;
    }

    /// <summary>
    /// Helper function that parses the block of text that is the decisionInfoList text.
    /// </summary>
    /// <param name="decisionInfoListText"></param>
    /// <returns></returns>
    static List<DecisionInfo> ParseDecisioninfoList(string decisionInfoListText)
    {
        List<DecisionInfo> decisionInfoList = new List<DecisionInfo>();

        //Split text
        string[] decisionInfoBlock = (decisionInfoListText.Trim()).Split(":"[0]);

        //Go through all of the text.
        for(int i = 0; i < decisionInfoBlock.Length; i++)
        {
            //Create a decision info.
            DecisionInfo info = new DecisionInfo();
            string key = Regex.Replace(decisionInfoBlock[i], "[^\\w\\._]", "");
            i++;
            string val = Regex.Replace(decisionInfoBlock[i], "[^\\w\\s]", "");

            //Set values.
            info.SetNextBlock(SceneBlockDictionary[key]);
            info.SetDecisionText(val);
            decisionInfoList.Add(info);
        }
        return decisionInfoList;
    }
    
    /// <summary>
    /// Helper function that parses the block of text that is the flag set list.
    /// </summary>
    /// <param name="flagSetListText"></param>
    /// <returns></returns>
    static List<FlagSet> ParseFlagSetList(string flagSetListText)
    {
        //Parse flagSetListText
        List<FlagSet> flagSetList = new List<FlagSet>();
        string[] flagSetData = (flagSetListText.Trim()).Split(":"[0]);

        //This bool is to know if its key or not.
        bool isKey = true;
        FlagSet flagSet = new FlagSet();

        //Loop through the flag set data.
        for (int i = 0; i < flagSetData.Length; i++)
        {
            //Remove punctuation from the text.
            string text = Regex.Replace(flagSetData[i], "[^\\w\\._]", "");

            //Alternate between adding the key, and adding the value.
            if (isKey)
            {
                flagSet.flag_key = text;
                isKey = false;
            }//After adding the value add it to the list then make a new instance of flagset.
            else
            {
                //Interpret the boolean value
                if (text.Equals("True") || text.Equals("true"))
                {
                    flagSet.flag_val = true;
                }
                else if (text.Equals("False") || text.Equals("false"))
                {
                    flagSet.flag_val = false;
                }
                else //Throw an error if we get something else so we know to fix the value.
                {
                    throw new System.Exception("Error, while parsing there was an incorrect flag value: " + flagSetData[i]);
                }

                //Add the flagSet we made to the list.
                flagSetList.Add(flagSet);

                //Re-init the flagSet.
                flagSet = new FlagSet();
            }
        }
        return flagSetList;
    }
}
#endif
