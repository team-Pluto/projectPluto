using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Robot class
/// </summary>
public class RobotClass
{
    //Name of the robot
    public string name = "Triangle";

    //Status of the robot
    public RobotStatus status = RobotStatus.Idle;

    //How charged the robot is
    public int charge_level = 3, max_Charge = 3;

    //How much progress has been made on the mission
    public int missionProgress = 0;

    //The mission the robot is on
    public MissionObject cur_mission = null;

    //Lists to hold types. All lists in case we have weird robots with more than one head etc.
    public List<HeadType> headList = new List<HeadType>();
    public List<TorsoType> torsoList = new List<TorsoType>();
    public List<ArmType> armList = new List<ArmType>();
    public List<LegType> legList = new List<LegType>();
    public List<MiscType> miscList = new List<MiscType>();

    //Constructor for a robot
    public RobotClass(string in_name, RobotStatus in_status)
    {
        name = in_name;
        status = in_status;
    }

    /// <summary>
    /// Set what mission this robot will attempt when you hit embark
    /// </summary>
    /// <param name="mission"></param>
    /// <returns></returns>
    public bool SetMission(MissionObject mission)
    {
        if(status == RobotStatus.Idle || status == RobotStatus.Charging)
        {
            if(charge_level >= mission.charge_requirement)
            {
                cur_mission = mission;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Progresses one cycle for this robot.
    /// Evaluates all things it needs to do.
    /// </summary>
    public void Progress()
    {
        //If we are meant to be charging, attempt to charge
        if(status == RobotStatus.Charging)
        {
            if(charge_level < max_Charge)
            {
                charge_level++;
                status = RobotStatus.Idle;
            }
        }
        else if(status == RobotStatus.OnMission)
        {
            ApplyMissionProgress(CalculateMissionProgress());
        }
        else if(status == RobotStatus.Idle) //This is for the case where the player has set a robot to a mission and then ends the day. We should embark the robot at the end of day.
        {
            if(cur_mission != null)
            {
                status = RobotStatus.OnMission;
                ApplyMissionProgress(CalculateMissionProgress());
            }
        }
    }

    /// <summary>
    /// Calculate how much progress was made based off of our mission and parts
    /// </summary>
    /// <returns></returns>
    public void ApplyMissionProgress(int session_value)
    {
        missionProgress += Mathf.Clamp(missionProgress + session_value, 0, 100);
        if (missionProgress == 100)
        {
            charge_level -= cur_mission.charge_requirement;
            status = RobotStatus.Idle;
            cur_mission.Complete();
            cur_mission = null;
        }
    }

    /// <summary>
    /// Calculate how much progress should be made this session
    /// </summary>
    /// <returns></returns>
    public int CalculateMissionProgress()
    {
        throw new System.Exception("Not yet implemented. Need to figure out how parts interact with mission elements (and what both of these things are).");
    }
}

//Enums for robot parts
public enum HeadType {Round, Square};
public enum TorsoType { Large, Medium, Small };
public enum ArmType { Grasper, Climber, Swimmer };
public enum LegType { Treads, Stilts, Booties};
public enum MiscType { Camera, VisualParser, AudioParser };

//Enums for robot status
public enum RobotStatus { Idle, OnMission, Charging, Damaged, Dead };