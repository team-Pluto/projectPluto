using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Creates an orbital ring
/// </summary>
public class CreateOrbitalRing : MonoBehaviour
{
    [MenuItem("MapNode/Create/Orbital Ring")]
    public static OrbitalRing CreateOrbital()
    {
        //Create an empty game object to be our new map node
        GameObject orbital = new GameObject();

        //Parent transform
        GameObject nodes_object = GameObject.Find("Nodes");

        if (nodes_object == null)
        {
            throw new System.Exception("No \"Nodes\" object.");
        }

        Transform nodes_parent = nodes_object.transform;

        //Get number of children with orbit in name
        int orbitCount = 0;
        for(int i = 0; i < nodes_parent.childCount; i++)
        {
            if(nodes_parent.GetChild(i).GetComponent<OrbitalRing>() != null)
            {
                orbitCount++;
            }
        }

        //Set name based off which orbit this is
        orbital.name = "Orbit" + (orbitCount + 1);

        //Set parent for orbital
        orbital.transform.parent = nodes_parent;

        //Add the components
        orbital.AddComponent<OrbitalRing>();

        //Set orbital depth
        orbital.GetComponent<OrbitalRing>().orbitDepth = orbitCount + 1;

        //Set orbital position to the middle of the screen
        orbital.transform.localPosition = Vector3.zero;

        //Set rotation to be local zero
        orbital.transform.localRotation = Quaternion.identity;

        //Set localScale to be local zero
        orbital.transform.localScale = Vector3.one;

        return orbital.GetComponent<OrbitalRing>();
    }
}
