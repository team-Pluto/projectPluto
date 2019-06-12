using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all connections
/// </summary>
public class NodeManager : MonoBehaviour
{

    /// <summary>
    /// List of all connections in game
    /// </summary>
    public List<Connection> connectionList = new List<Connection>();

    /// <summary>
    /// List of all orbital rings in game
    /// </summary>
    public List<OrbitalRing> orbitList = new List<OrbitalRing>();

    /// <summary>
    /// Central node
    /// </summary>
    public Node homeNode;

    /// <summary>
    /// Add a connection to the list
    /// </summary>
    /// <param name="con"></param>
    public void AddConnection(Connection con)
    {
        if (!connectionList.Contains(con))
        {
            connectionList.Add(con);
        }    
    }

    /// <summary>
    /// Removes all connections in relation to that node.
    /// </summary>
    /// <param name="node"></param>
    public void RemoveNodeAndConnections(Node node)
    {
        //Have to do two lists
        List<Connection> remove_list = new List<Connection>();
        foreach(Connection con in connectionList)
        {
            if(con.node1 == node || con.node2 == node)
            {
                remove_list.Add(con);
            }
        }

        foreach(Connection con in remove_list)
        {
            RemoveConnection(con);
        }
    }

    /// <summary>
    /// Remove a connection from list
    /// </summary>
    /// <param name="con"></param>
    public void RemoveConnection(Connection con)
    {
        connectionList.Remove(con);
    }

    /// <summary>
    /// Get a list of indexes for the connectionsfor this node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<int> GetConnectionsOf(Node node)
    {
        List<int> conList = new List<int>();
        for(int i = 0; i < connectionList.Count; i++)
        {
            //If either node is the one we want
            if(connectionList[i].node1 == node || connectionList[i].node2 == node)
            {
                //Add it to the list
                conList.Add(i);
            }
        }
        return conList;
    }

    /// <summary>
    /// Helper function that returns the  connection between two given nodes.
    /// Returns null if no valid connection.
    /// </summary>
    /// <param name="node1"></param>
    /// <param name="node2"></param>
    /// <returns></returns>
    public Connection GetConnectionBetween(Node node1, Node node2)
    {
        foreach(Connection con in connectionList)
        {
            //If the nodes are equivalent (order/assignment doesn't matter) return that connection
            if((con.node1 == node1 && con.node2 == node2) || (con.node1 == node2 && con.node2 == node1))
            {
                return con;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Connection
{
    /// <summary>
    /// Node that this connection is to
    /// </summary>
    public Node node1;

    /// <summary>
    /// The other node this node is connected to
    /// </summary>
    public Node node2;

    /// <summary>
    /// Cost to get from this node to that node.
    /// </summary>
    public float cost;

    /// <summary>
    /// Connection constructor
    /// </summary>
    /// <param name="in_node"></param>
    /// <param name="in_cost"></param>
    public Connection(Node in_node1, Node in_node2, float in_cost)
    {
        node1 = in_node1;
        node2 = in_node2;
        cost = in_cost;
    }

    /// <summary>
    /// Helper function that returns the other node given a node.
    /// Returns null if the given node isn't even in the connection, or the connection isn't valid.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Node GetOther(Node node)
    {
        if (node == node1)
        {
            return node2;
        }
        else if (node == node2)
        {
            return node1;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Replace a given node with a new node.
    /// Returns true if it works, false if not.
    /// NOTE: If the newNode we are placing into the old node's slot is the other node in the connection, 
    /// we don't have to do anything. The connection is the same.
    /// </summary>
    /// <param name="oldNode"></param>
    /// <param name="newNode"></param>
    public bool ReplaceNode(Node oldNode, Node newNode)
    {
        //Base case, this connection is already correct.
        if(node1 == newNode || node2 == newNode)
        {
            return true;
        }
        if(node1 == oldNode)
        {
            node1 = newNode;
            return true;
        }
        else if(node2 == oldNode)
        {
            node2 = newNode;
            return true;
        }
        return false;

    }
}