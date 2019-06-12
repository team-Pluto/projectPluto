using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows for player manipulation of nodes
/// </summary>
public class NodeManipulator : MonoBehaviour
{
    //State for the manipulator.
    public enum ManipulatorState { Idle, Selecting, Finished };
    public ManipulatorState state = ManipulatorState.Idle;

    //List of selected nodes
    public List<Node> selectedNodes = new List<Node>();

    //Home node
    public Node homeNode;

    //Highlight color for all nodes
    public Color node_highlight_color;

    //Line renderer
    LineRenderer line_rend;

    //List of positions for line renderer.
    Vector3[] positions = new Vector3[0];

    //Camera
    Camera renderCam;

    //Maximum cost we can travel
    public float max_cost = 2;
    float cur_cost = 0;

    //Node Manager
    NodeManager manager;

    private void Start()
    {
        line_rend = GetComponent<LineRenderer>();
        renderCam = Camera.main;
        manager = GetComponent<NodeManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case ManipulatorState.Idle:
                if(line_rend.positionCount > 1)
                {
                    //Empty positions
                    positions = new Vector3[1];

                    positions[0] = Vector3.zero;

                    //Clear line renderer
                    line_rend.positionCount = 1;
                    line_rend.SetPositions(positions);
                }           
                break;
            case ManipulatorState.Selecting:
                //First we need to set the right size of positions
                positions = new Vector3[selectedNodes.Count + 1];

                //Convert mouse position to world space
                Ray mouseRay = renderCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(mouseRay, out hit);

                //Set the last index to the mouse position
                positions[positions.Length - 1] = hit.point;

                //Now set the position for every node
                for(int i = 0; i < selectedNodes.Count; i++)
                {
                    positions[i] = selectedNodes[i].transform.position;
                }

                //Set position count
                line_rend.positionCount = positions.Length;

                //Set the positions
                line_rend.SetPositions(positions);
                break;
            case ManipulatorState.Finished:
                break;
        }
    }

    /// <summary>
    /// Changes state
    /// </summary>
    /// <param name="new_state"></param>
    public void ChangeState(ManipulatorState new_state)
    {
        state = new_state;
    }

    /// <summary>
    /// Attempts to select a given node. Doesn't call SelectNode if invalid
    /// </summary>
    /// <param name="node"></param>
    public bool AttemptSelectNode(Node node)
    {
        if(state == ManipulatorState.Idle)
        {
            //Since this is the first one, add the home node if it isn't the one selected
            if(node != homeNode)
            {
                //Click the node in code
                homeNode.ClickNode();
            }

            //Change to Selecting
            ChangeState(ManipulatorState.Selecting);
        }

        //If we already have the node in list, deselect it
        if (selectedNodes.Contains(node))
        {
            RemoveNode(node);
            return true;
        }
        else//Select it if possible
        {
            //If there is a previous node, check 
            if(selectedNodes.Count > 0)
            {
                //Check cost between the new node and the previously selected node
                Connection con = manager.GetConnectionBetween(node, selectedNodes[selectedNodes.Count - 1]);

                //Only allow selection if the cost works
                if(con != null && con.cost + cur_cost <= max_cost)
                {
                    SelectNode(node);
                    return true;
                }
            }
            else
            {
                SelectNode(node);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Helper function that selects node, and updates our cur_cost
    /// </summary>
    /// <param name="node"></param>
    public void SelectNode(Node node)
    {
        //Only update current cost if this wasn't the first node added.
        if(selectedNodes.Count >= 1)
        {
            //Check cost between the new node and the previously selected node
            Connection con = manager.GetConnectionBetween(node, selectedNodes[selectedNodes.Count - 1]);

            //Update cur_cost
            cur_cost += con.cost;
        }   

        //Call VisualSelectNode (shows visually the node is selected)
        node.VisualSelectNode(node_highlight_color);

        //Show costs in relation to node
        DisplayCostsInRelationTo(node);

        //Add to list
        selectedNodes.Add(node);
    }

    /// <summary>
    /// Deselects a given node and removes from selected list. Also updates cur_cost
    /// </summary>
    /// <param name="node"></param>
    public void DeselectNode(Node node)
    {
        //If we are deselecting the homeNode, just set to zero. No more lookups needed.
        if(node == homeNode)
        {
            cur_cost = 0;
        }
        //Only update current cost if this isn't the last node to be removed.
        //NOTE: If there are only 2 nodes left, and it isn't home node, then this will be the second node selected an will function.
        // In any other case including that one there should be a previous node to reference
        else if (selectedNodes.Count >= 2)
        {
            //Check cost between the new node and the previously selected node
            Connection con = manager.GetConnectionBetween(node, selectedNodes[selectedNodes.IndexOf(node) - 1]);

            //Update cur_cost
            cur_cost -= con.cost;
        }

        node.VisualDeselectNode(node_highlight_color);
        selectedNodes.Remove(node);
    }

    /// <summary>
    /// Removes node from the list. Has to remove all nodes before to maintain a path.
    /// </summary>
    /// <param name="node"></param>
    public void RemoveNode(Node node)
    {
        int index = selectedNodes.IndexOf(node);
        for(int i = selectedNodes.Count - 1; i >= index; i--)
        {
            //Deselect the node. Also removes from list.
            DeselectNode(selectedNodes[i]);
        }
        
        //If there is only one node selected (home) or if there are none, change back to idle.
        if (selectedNodes.Count <= 1)
        {
            //If it was one, we need to deselect the home
            if(selectedNodes.Count == 1)
            {
                DeselectNode(homeNode);
            }

            //Change state
            ChangeState(ManipulatorState.Idle);
        }
    }

    /// <summary>
    /// Displays the cost  of a node in relation to the given node.
    /// </summary>
    /// <param name="node"></param>
    public void DisplayCostsInRelationTo(Node node)
    {
        List<int> index_list = manager.GetConnectionsOf(node);
        foreach(int index in index_list)
        {
            Node other = manager.connectionList[index].GetOther(node);
            //If we are the homeNode so we don't need to check orbital ring depth, OR the the pair of nodes in question are in an orbit and the other is greater by depth.
            //NOTE: Other cannot be homeNode or the checks won't work./
            if(node != homeNode)
            {
                //Check orbital depth as long as both nodes aren't the home node, which does not belong in any orbit.
                if (other != homeNode)
                {
                    if (other.transform.parent.GetComponent<OrbitalRing>() != null &&
                        node.transform.parent.GetComponent<OrbitalRing>() != null &&
                        other.transform.parent.GetComponent<OrbitalRing>().orbitDepth > node.transform.parent.GetComponent<OrbitalRing>().orbitDepth)
                    {
                        other.SetText("" + manager.connectionList[index].cost);
                    }
                }
            }
            else//For home node case, just display the costs.
            {
                other.SetText("" + manager.connectionList[index].cost);
            }
           
           
        }
    }
}
