using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code found: http://wiki.unity3d.com/index.php/Toolbox
/// </summary>
public class Toolbox : Singleton<Toolbox>
{
    // Used to track any global components added at runtime.
    private Dictionary<string, Component> m_Components = new Dictionary<string, Component>();

    // Prevent constructor use.
    protected Toolbox() { }

    private void Awake()
    {
        // Put initialization code here.
        m_FlagDict.LoadFlagFile();
    }


    // Define all required global components here. These are hard-coded components
    // that will always be added. Unlike the optional components added at runtime.
    //Below add any singleton like monobehaviours we want as well as getters.

    //The flagDict is where we store all the flags the player could trigger.
    private FlagDict m_FlagDict = new FlagDict();

    public FlagDict GetFlagDict()
    {
        return m_FlagDict;
    }

    //The dialogueDict is where we store all the starting dialogue blocks that we need to know in this scene.
    private DialogueDict m_DialogueDict = new DialogueDict();

    public DialogueDict GetDialogueDict()
    {
        return m_DialogueDict;
    }

    //The dialogueManager is what actually transfers the dialogue blocks to the screen.
    private DialogueManager m_DialogueManager = new DialogueManager();

    public DialogueManager GetDialogueManager()
    {
        return m_DialogueManager;
    }

    //The roboBay is what stores all the robots
    private RobotBay m_RobotBay = new RobotBay();

    public RobotBay GetRobotBay()
    {
        return m_RobotBay;
    }

    // The methods below allow us to add global components at runtime.
    // TODO: Convert from string IDs to component types.
    public Component AddGlobalComponent(string componentID, Type component)
    {
        if (m_Components.ContainsKey(componentID))
        {
            Debug.LogWarning("[Toolbox] Global component ID \""
                + componentID + "\" already exist! Returning that.");
            return GetGlobalComponent(componentID);
        }

        var newComponent = gameObject.AddComponent(component);
        m_Components.Add(componentID, newComponent);
        return newComponent;
    }


    public void RemoveGlobalComponent(string componentID)
    {
        Component component;

        if (m_Components.TryGetValue(componentID, out component))
        {
            Destroy(component);
            m_Components.Remove(componentID);
        }
        else
        {
            Debug.LogWarning("[Toolbox] Trying to remove nonexistent component ID \""
                + componentID + "\"! Typo?");
        }
    }


    public Component GetGlobalComponent(string componentID)
    {
        Component component;

        if (m_Components.TryGetValue(componentID, out component))
        {
            return component;
        }

        Debug.LogWarning("[Toolbox] Global component ID \""
    + componentID + "\" doesn't exist! Typo?");
        return null;
    }
}