using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Nodes on the map.
/// </summary>
public class Node : MonoBehaviour
{
    /// <summary>
    /// Node types, used for scout bot scanning. Types still up for debate.
    /// </summary>
    public enum MissionType { Natural, Human, Resource , Special};
    public MissionType type = MissionType.Natural;

    //When scouted, show the real colors for the node
    public bool isScouted = false;

    //Needs some other info for missions and the like
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Remove this when scouting is implemented.");
            ScoutNode();
        }
    }


    /// <summary>
    /// Function call when this node is scouted.
    /// </summary>
    public void ScoutNode()
    {
        //For now just change the color based on enum
        switch (type)
        {
            case MissionType.Natural:
                GetComponent<Image>().color = Color.green;
                break;
            case MissionType.Human:
                GetComponent<Image>().color = Color.red;
                break;
            case MissionType.Resource:
                GetComponent<Image>().color = Color.blue;
                break;
            case MissionType.Special:
                Debug.Log("Do nothing for special nodes. Leave as is. Used for homeNode");
                break;
        }
    }

    /// <summary>
    /// On click, send a message to nodeManager
    /// </summary>
    public void ClickNode()
    {
        bool success = false;
        //If our parent has node manipulator then call that
        if (transform.parent.GetComponent<NodeManipulator>() != null)
        {
            success = transform.parent.GetComponent<NodeManipulator>().AttemptSelectNode(this);
        }
        else if (transform.parent.parent.GetComponent<NodeManipulator>() != null) //If not, then we are an orbit node and we have to move one more parent up.
        {
            success = transform.parent.parent.GetComponent<NodeManipulator>().AttemptSelectNode(this);
        }
        else
        {
            throw new System.Exception("Error: Couldn't find NodeManipulator. Can only be of depth 1 from Nodes object.");
        }

        if (success)
        {
            //ToggleOrbit();
        }
        
    }

    /// <summary>
    /// On click for orbit nodes, toggle orbit status.
    /// </summary>
    public void ToggleOrbit()
    {
        //If our parent has orbital ring then we are an orbit node.
        if(transform.parent.GetComponent<OrbitalRing>() != null)
        {
            transform.parent.GetComponent<OrbitalRing>().ToggleNodesActive();
        }

        //We should still be interactable to cancel.
        GetComponent<Button>().interactable = true;
    }

    /// <summary>
    /// Selects this node. Applies highlights/other visual cues
    /// </summary>
    public void VisualSelectNode(Color color)
    {
        GetComponent<Image>().color = (GetComponent<Image>().color + color)/2;
        ToggleOrbit();
    }

    /// <summary>
    /// Deselects this node. Removes all highlights/visual cues.
    /// </summary>
    public void VisualDeselectNode(Color color)
    {
        GetComponent<Image>().color = (GetComponent<Image>().color * 2) - color;
        ToggleOrbit();
    }

    /// <summary>
    /// Function that sets text and turns it on
    /// </summary>
    /// <param name="in_text"></param>
    public void SetText(string in_text)
    {
        transform.GetComponentInChildren<Text>().text = in_text;
        transform.GetComponentInChildren<Text>().enabled = true;
    }
}
