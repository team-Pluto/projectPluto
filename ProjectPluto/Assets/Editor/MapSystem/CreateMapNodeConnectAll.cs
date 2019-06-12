using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Creates a map node and connects to all existing nodes.
/// </summary>
public class CreateMapNodeConnectAll
{
    /// <summary>
    /// Creates a map node connected to all nodes.
    /// </summary>
    /// <returns></returns>
    [MenuItem("MapNode/Create/All Connections")]
    public static Node CreateConnectedAllNode()
    {
        //Make an empty node
        Node map_node = CreateMapNode.CreateNode();

        //Parent transform
        GameObject nodes_object = GameObject.Find("Nodes");

        if (nodes_object == null)
        {
            throw new System.Exception("No \"Nodes\" object.");
        }

        Transform nodes_parent = nodes_object.transform;

        //Node manager
        NodeManager manager = nodes_parent.GetComponent<NodeManager>();

        //Get all the node objects from the parent
        Node[] node_list = nodes_parent.GetComponentsInChildren<Node>();

        //Add all nodes to list
        foreach(Node node in node_list)
        {
            //As long as its not ourselves
            if(node != map_node)
            {
                //Have both nodes connect
                Connection con = new Connection(node, map_node, 1);
                manager.AddConnection(con);
            }
        }

        return map_node;
    }
}
