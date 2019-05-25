using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object that saves all the details in a given mission
[CreateAssetMenu(fileName = "Mission", menuName ="Mission", order = 1)]
public class MissionObject : ScriptableObject
{
    /// <summary>
    /// Name of the mission
    /// </summary>
    public string mission_name = "AppleHunt";

    /// <summary>
    /// How much of a charge is needed to complete the mission
    /// </summary>
    public int charge_requirement = 1;

    /// <summary>
    /// Description of the mission
    /// </summary>
    public string mission_description;

    /// <summary>
    /// What you get for finishing this mission
    /// </summary>
    public List<GameObject> mission_rewards;

    /// <summary>
    /// Function that completes this given mission given a robot
    /// </summary>
    /// <param name="robot"></param>
    public void Complete()
    {
        GiveRewards();
        MarkAsComplete();
    }

    /// <summary>
    /// Give rewards to the player based off of our rewards list.
    /// </summary>
    public void GiveRewards()
    {
        throw new System.Exception("Error, not yet implemented. Need to write rewards code.");
    }

    /// <summary>
    /// Marks this mission as complete in our save file. This will alter the map state as well
    /// </summary>
    public void MarkAsComplete()
    {
        throw new System.Exception("Error, not yet implemented. Need to write code that marks a mission as complete.");
    }
}
