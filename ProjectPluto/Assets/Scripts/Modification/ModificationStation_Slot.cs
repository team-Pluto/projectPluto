using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modification station slot script. Attached to buttons that show the slots.
/// </summary>
public class ModificationStation_Slot : MonoBehaviour
{
    //Which slot this is
    public SlotType slotType = SlotType.ArmType;

    //Number of slots.
    public int slots = 1;

    //List of transforms that are the slots.
    public List<Transform> slotTransforms = new List<Transform>();

    //List of parts placed in this slot.
    public List<RobotPart> equippedParts = new List<RobotPart>();

    /// <summary>
    /// Called when changes are made to display the parts
    /// </summary>
    public void DisplayParts(List<RobotPart> roboList)
    {

    }
}
