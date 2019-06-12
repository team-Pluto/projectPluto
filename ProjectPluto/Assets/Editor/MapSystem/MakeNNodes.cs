using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Makes N connected nodes at once.
/// </summary>
public class MakeNNodes : EditorWindow
{
    //Rects
    private Rect mainPanel;
    private Rect sliderPanel;
    private Rect buttonPanel;

    //Int variables
    int number = 1;
    int min_number = 1;
    int max_number = 100;

    //Bool variables
    bool orbital = false;
    bool orbitalConnect = false;
    bool connected = true;
    bool interConnected = true;

    [MenuItem("MapNode/Create/Multiple")]
    private static void OpenWindow()
    {
        MakeNNodes window = GetWindow<MakeNNodes>();
        window.titleContent = new GUIContent("Make MapNodes");
    }

    private void OnGUI()
    {
        DrawMainPanel();

        //The vertical organizes everything else we draw into a nice vertical stack.
        GUILayout.BeginVertical();

        DrawNumberSlider();

        DrawOrbitalToggle();

        DrawOrbitalConnectToggle();

        DrawConnectedToggle();

        DrawInterconnectedToggle();

        DrawMakeNodeButton();

        GUILayout.EndVertical();
    }

    private void DrawMainPanel()
    {
        mainPanel = new Rect(0, 0, position.width, position.height);

        GUILayout.BeginArea(mainPanel);

        GUILayout.EndArea();
    }

    private void DrawNumberSlider()
    {
        EditorGUILayout.LabelField(new GUIContent("Number of nodes","Number of nodes you wish to create"));
        number = EditorGUILayout.IntSlider(number, min_number, max_number);
    }

    private void DrawOrbitalToggle()
    {
        orbital = GUILayout.Toggle(orbital, new GUIContent("Orbital", "Whether or not all the created nodes should be in one orbital ring."));
    }

    private void DrawOrbitalConnectToggle()
    {
        if (orbital)
        {
            orbitalConnect = GUILayout.Toggle(orbitalConnect, new GUIContent("Orbital Connect", "Whether or not we should connect the new orbit to the last one, and nothing else."));
        }
    }

    private void DrawConnectedToggle()
    {
        if (!orbitalConnect)
        {
            connected = GUILayout.Toggle(connected, new GUIContent("Connected", "Whether or not all the created nodes should be connected to all nodes in the scene."));
        }     
    }

    private void DrawInterconnectedToggle()
    {
        if (!connected && !orbitalConnect)
        {
            interConnected = GUILayout.Toggle(interConnected, new GUIContent("Interconnected", "Whether or not the created nodes are connected to one another, but not connected to the nodes currently in the scene."));
        }
    }

    private void DrawMakeNodeButton()
    {
        if(GUILayout.Button(new GUIContent("Create", "Creates the nodes with the current settings.")))
        {
            //List of nodes for interconnected.
            List<Node> node_list = new List<Node>();

            //Parent transform
            GameObject nodes_object = GameObject.Find("Nodes");

            if (nodes_object == null)
            {
                throw new System.Exception("No \"Nodes\" object.");
            }

            Transform nodes_parent = nodes_object.transform;

            //Node manager
            NodeManager manager = nodes_parent.GetComponent<NodeManager>();

            //Create an orbit if we want one
            OrbitalRing orbit = null;
            OrbitalRing prevOrbit = null;
            int cur_depth = 0;
            if (orbital)
            {
                //If we want to connect orbits, we need the previous one. Search for it first before making one.
                if (orbitalConnect)
                {
                    //Start from the bottom and count up
                    foreach(OrbitalRing tempOrbit in manager.orbitList)
                    {
                        if(cur_depth < tempOrbit.orbitDepth)
                        {
                            prevOrbit = tempOrbit;
                            cur_depth = tempOrbit.orbitDepth;
                        }
                    }
                }
                //Now make new orbit
                orbit = CreateOrbitalRing.CreateOrbital();
                manager.orbitList.Add(orbit);
            }
            for(int i = 0; i < number; i++)
            {
                Node node = null;
                if(orbitalConnect)
                {
                    node = CreateMapNode.CreateNode();
                    //If we found a previous orbit, connect to that one
                    if (prevOrbit != null)
                    {
                        foreach (Node map_node in prevOrbit.nodes)
                        {
                            Connection con = new Connection(node, map_node, 1);
                            manager.AddConnection(con);
                        }
                    }
                    else //Otherwise, just connect to home node since this is the first orbit
                    {
                        Connection con = new Connection(node, manager.homeNode, 1);
                        manager.AddConnection(con);
                    }               
                }
                else if (connected)
                {
                    node = CreateMapNodeConnectAll.CreateConnectedAllNode();               
                }
                else if (interConnected)//Not connected but interconnected
                {
                    node = CreateMapNode.CreateNode();
                  
                    foreach (Node map_node in node_list)
                    {
                        Connection con = new Connection(node, map_node, 1);
                        manager.AddConnection(con);
                    }
                }
                else //Not connected nor interconnected nor orbitalConnect.
                {
                    node = CreateMapNode.CreateNode();
                }

                //If we are a making an orbital, put the node as a child of the orbit transform. Also add it to the list of orbits.
                if (orbital)
                {
                    node.transform.parent = orbit.transform;
                    orbit.nodes.Add(node);
                }
            }
        }
    }
}
