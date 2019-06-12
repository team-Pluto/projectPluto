using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Events;

public class CreateMapNode
{
    /// <summary>
    /// Creates a generic map node with absolutely no connections.
    /// Returns the node
    /// </summary>
    [MenuItem("MapNode/Create/No Connections")]
    public static Node CreateNode()
    {
        //Create an empty game object to be our new map node
        GameObject map_node = new GameObject();

        //Parent transform
        GameObject nodes_object = GameObject.Find("Nodes");

        if(nodes_object == null)
        {
            throw new System.Exception("No \"Nodes\" object.");
        }

        Transform nodes_parent = nodes_object.transform;

        //Set the parent of map_node
        map_node.transform.parent = nodes_parent;

        //Attach the important components
        Node node = map_node.AddComponent<Node>();
        Image image = map_node.AddComponent<Image>();
        Button button = map_node.AddComponent<Button>();

        //Add a second object as the node's child to hold text
        GameObject text_object = new GameObject();
        text_object.transform.parent = map_node.transform;

        //Set position of the new text object
        text_object.transform.localPosition = Vector3.zero;
        text_object.transform.localRotation = Quaternion.identity;
        text_object.transform.localScale = Vector3.one;

        //Setup text component
        Text text = text_object.AddComponent<Text>();
        text.text = "0";
        text.fontSize = 72;
        text.alignment = TextAnchor.MiddleCenter;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.color = Color.black;

        //Adjust button component.
        UnityEventTools.AddPersistentListener(button.onClick, node.ClickNode);
        //UnityEventTools.AddPersistentListener(button.onClick, node.ToggleOrbit);

        //Adjust image component, add sprite and scale. Yet to be done.
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        //Get number of children with orbit in name
        int nodeCount = 0;
        for (int i = 0; i < nodes_parent.childCount; i++)
        {
            if (nodes_parent.GetChild(i).GetComponent<Node>() != null)
            {
                nodeCount++;
            }
            if(nodes_parent.GetChild(i).GetComponent<OrbitalRing>() != null)
            {
                nodeCount += nodes_parent.GetChild(i).GetComponent<OrbitalRing>().nodes.Count;
            }
        }
        //Set name based off which orbit this is
        map_node.name = "MapNode" + (nodeCount - 1);

        //Set map_node position to the middle of the screen
        map_node.transform.localPosition = Vector3.zero;

        //Set rotation to be local zero
        map_node.transform.localRotation = Quaternion.identity;

        //Set localScale to be local zero
        map_node.transform.localScale = Vector3.one;

        return node;
    }
}
