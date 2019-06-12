using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Orbital Ring script. Holds the nodes in this ring.
/// </summary>
public class OrbitalRing : MonoBehaviour
{
    //Whether or not the nodes in this orbital ring are act ive
    public bool nodes_active = true;

    //List of nodes in this orbital ring.
    public List<Node> nodes = new List<Node>();

    //List of positions in this orbit.
    List<Vector3> orbit_positions = new List<Vector3>();

    //List of indexes for connections for each position
    [SerializeField]
    List<List<int>> connections_list = new List<List<int>>();

    //List of positions for lineRenderer
    Vector3[] positions;

    //Node Manager
    NodeManager nodeManager;

    //Line renderer
    LineRenderer lineRenderer;

    //Orbit colors
    public Color orbitColor, inactiveOrbitColor;

    //Orbital depth. Used to calculate adding new nodes and for if its valid to go to another ring.
    public int orbitDepth;

    //Manager
    NodeManager manager;

    //Vertex count for drawing the orbit.
    int vertexCount = 12;
    int orbitPush = 10;

    private void Start()
    {
        //Node manager
        nodeManager = transform.parent.GetComponent<NodeManager>();

        //Line renderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        //Get the nodeManager.
        manager = transform.parent.GetComponent<NodeManager>();

        //Save list of positions and connections
        foreach (Node node in nodes)
        {
            orbit_positions.Add(node.transform.localPosition);
            connections_list.Add(nodeManager.GetConnectionsOf(node));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Remove this when orbitting is implemented.");
            OrbitCounterClockwise();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Remove this when orbitting is implemented.");
            OrbitClockwise();
        }

        UpdateOrbitLine();
    }

    public void UpdateOrbitLine()
    {
        positions = new Vector3[nodes.Count];
        //Save list of positions and connections
        for(int i = 0; i < positions.Length; i++)
        {
            //Add some spacing so it renders over the camera?
            positions[i] = nodes[i].transform.position + new Vector3(0, 0, -1);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    /// <summary>
    /// Set the interactabliity of all nodes in this ring
    /// </summary>
    /// <param name="state"></param>
    public void ToggleNodesActive()
    {
        foreach(Node node in nodes)
        {
            node.GetComponent<Button>().interactable = !nodes_active;
        }

        //If we are currently inactive and are becoming act ive
        if (!nodes_active)
        {
            //Reset colors
            lineRenderer.startColor = orbitColor;
            lineRenderer.endColor = orbitColor;

            //Reset text on all nodes
            foreach(Node node in nodes)
            {
                node.SetText("");
            }
        }
        else
        {
            lineRenderer.startColor = inactiveOrbitColor;
            lineRenderer.endColor = inactiveOrbitColor;
        }
        nodes_active = !nodes_active;
    }

    /// <summary>
    /// Orbit clockwise. Moves the index of every node in the list by plus one.
    /// Also updates connections and positions so they match.
    /// </summary>
    public void OrbitClockwise()
    {
        //Get the first node, remove it  from list then add it again so it's on the end.
        Node firstNode = nodes[0];
        nodes.Remove(firstNode);
        nodes.Add(firstNode);

        for (int i = 0; i < nodes.Count; i++)
        {
            //Update positions
            nodes[i].transform.localPosition = orbit_positions[i];

            Node oldNode;
            //THe last index if we just moved clockwise
            if( i > 0)
            {
                oldNode = nodes[i - 1];
            }
            else //If it is zero then it is the last index
            {
                oldNode = nodes[nodes.Count - 1];
            }
            foreach (int index in connections_list[i])
            {
                nodeManager.connectionList[index].ReplaceNode(oldNode, nodes[i]);
            }
        }
    }

    /// <summary>
    /// Orbit counter clockwise. Moves the index of every node in the list by minus one.
    /// </summary>
    public void OrbitCounterClockwise()
    {
        //Get the last node, remove it and add it at index 0.
        Node lastNode = nodes[nodes.Count - 1];
        nodes.Remove(lastNode);
        nodes.Insert(0, lastNode);

        for (int i = 0; i < nodes.Count; i++)
        {
            //Update positions
            nodes[i].transform.localPosition = orbit_positions[i];

            Node oldNode;
            //THe last index if we just moved clockwise
            if (i < nodes.Count - 1)
            {
                oldNode = nodes[i + 1];
            }
            else //If it is the max index then it is the first index
            {
                oldNode = nodes[0];
            }
            foreach (int index in connections_list[i])
            {
                nodeManager.connectionList[index].ReplaceNode(oldNode, nodes[i]);
            }
        }
    }
}
