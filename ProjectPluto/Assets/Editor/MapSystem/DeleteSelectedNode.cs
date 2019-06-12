using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Deletes selected node and all relevant connections.
/// </summary>
public class DeleteSelectedNode : MonoBehaviour
{
    [MenuItem("MapNode/Delete/Selected")]
    public static void DeleteSelectedNodes()
    {
        //Parent transform
        GameObject nodes_object = GameObject.Find("Nodes");

        if (nodes_object == null)
        {
            throw new System.Exception("No \"Nodes\" object.");
        }

        Transform nodes_parent = nodes_object.transform;

        NodeManager manager = nodes_parent.GetComponent<NodeManager>();

        GameObject[] objects = Selection.gameObjects;

        //If there is more than one selected node
        for(int i = objects.Length - 1; i >= 0; i--)
        {
            //If it has a component node on it
            if(objects[i].GetComponent<Node>() != null)
            {
                //Remove from manager
                manager.RemoveNodeAndConnections(objects[i].GetComponent<Node>());

                //Remove from orbit if relevant
                if(objects[i].transform.parent.GetComponent<OrbitalRing>() != null)
                {
                    objects[i].transform.parent.GetComponent<OrbitalRing>().nodes.Remove(objects[i].GetComponent<Node>());
                }
                DestroyImmediate(objects[i]);
            }
            else if(objects[i].GetComponent<OrbitalRing>() != null) //If it is an orbital ring, remove the whole orbit
            {
                OrbitalRing orbit = objects[i].GetComponent<OrbitalRing>();
                for(int j = orbit.nodes.Count - 1; j >= 0; j--)
                {
                    //Remove from manager
                    manager.RemoveNodeAndConnections(orbit.nodes[j]);
                    manager.orbitList.Remove(orbit);
                    DestroyImmediate(orbit.nodes[j]);
                }
                //Destroy orbit as well.
                DestroyImmediate(objects[i]);
            }
        }      
    }
}
