using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Data structure that contains all of the flags in our game.
/// </summary>
public class FlagDict : MonoBehaviour
{
    /// <summary>
    /// This is the flag dictionary. 
    /// Key is a string, value is a bool.
    /// </summary>
    [SerializeField]
    private Dictionary<string, bool> flagDictionary = new Dictionary<string, bool>();

    /// <summary>
    /// Load flag file into our dictionary.
    /// </summary>
    public void LoadFlagFile()
    {
        string path = "Assets/Data/FlagData.txt";

        try
        {
            StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();
            while(line != null)
            {
                string[] splitLine = (line.Trim()).Split(","[0]);

                string flag_name = splitLine[0].Trim();
                string flag_val_string = splitLine[1].Trim();
                bool flag_val = true;

                //Turn the string into a bool.
                if(flag_val_string.Equals("True") || flag_val_string.Equals("true"))
                {
                    flag_val = true;
                }
                else if(flag_val_string.Equals("False") || flag_val_string.Equals("false"))
                {
                    flag_val = false;
                }
                
                //Add to dictionary.
                flagDictionary.Add(flag_name, flag_val);

                //Move on to next line.
                line = reader.ReadLine();
            }
        }
        catch
        { 
            throw new System.Exception("Flag file not found. Make sure it has been initialized.");
        }
        Debug.Log("Finished populating flag dictionary.");
    }

    /// <summary>
    /// Adds a given flag to our flagDictionary. Sets its value to the given val.
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="val"></param>
    public void AddFlag(string flag, bool val)
    {
        if (!flagDictionary.ContainsKey(flag))
        {
            flagDictionary.Add(flag, val);
        }
        else
        {
            throw new System.Exception("Error, attempted to add a flag that already exists in the flagDictionary.");
        }
    }

    /// <summary>
    /// Sets a given flag in our flagDictionary to the given val if possible.
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="val"></param>
    public void SetFlag(string flag, bool val)
    {
        if (flagDictionary.ContainsKey(flag))
        {
            flagDictionary[flag] = val;
        }
        else
        {
            throw new System.Exception("Error, attempted to set a flag that doesn't exist in the flagDictionary.");
        }
    }

    /// <summary>
    /// Helper function that retrieves the flag's value given a flag's descriptor/key.
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool GetFlagVal(string flag)
    {
        if (flagDictionary.ContainsKey(flag))
        {
            return flagDictionary[flag];
        }
        else
        {
            throw new System.Exception("Error, attempted to access a flag that doesn't exist in the flagDictionary.");
        }
        
    }

    /// <summary>
    /// Function that returns a flag vector to be used in comparisons.
    /// Input requires a list of flags that need to be looked up in the FlagDictionary.
    /// </summary>
    /// <param name="flag_list"></param>
    /// <returns></returns>
    public List<bool> GetFlagVector(List<string> flag_list)
    {
        //Create an empty flag vector
        List<bool> flag_vector = new List<bool>();

        //For every key in our flag list
        foreach(string key in flag_list)
        {
            //If the flagDictionary doesn't contain that key, throw an error.
            if(!flagDictionary.ContainsKey(key))
            {
                throw new System.Exception("Error, given key: " + key + " does not exist in flagDictionary.");
            }

            //Add the bool value found in the dictionary given that key.
            flag_vector.Add(flagDictionary[key]);
        }

        //Return the constructed flag vector
        return flag_vector;
    }

    /// <summary>
    /// Getter for the actual flag dictionary.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, bool> GetDict()
    {
        return flagDictionary;
    }
}
