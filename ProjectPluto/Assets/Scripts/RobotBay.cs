using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves all the robots. This is where they are stored
/// </summary>
public class RobotBay : MonoBehaviour
{
    //List of robots
    public List<RobotClass> robotList = new List<RobotClass>();
    
    /// <summary>
    /// Sends a given robot on a given mission
    /// </summary>
    /// <param name="mission"></param>
    /// <param name="robot"></param>
    /// <returns></returns>
    public bool SetMissionToRobot(MissionObject mission, RobotClass robot)
    {
        return robot.SetMission(mission);
    }

    /// <summary>
    /// Evaluates progress for all robots. Usually called at the end of the day/session.
    /// </summary>
    public void EvaluateProgress()
    {
        foreach(RobotClass robot in robotList)
        {
            robot.Progress();
        }
    }
}
