using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Manages a bunch of tabs
/// </summary>
public class TabManager : MonoBehaviour
{
    /// <summary>
    /// List of the tabs we're managing.
    /// </summary>
    public List<TabWindow> tabList = new List<TabWindow>();

    /// <summary>
    /// List of buttons  we're managing.
    /// </summary>
    public List<Button> buttonList = new List<Button>();

    private void Start()
    {
        if(tabList.Count != buttonList.Count)
        {
            throw new System.Exception("Error: The number of tabs we want to display isn't equivalent to the number of buttons. Please check the public lists on the TabManager.");
        }

        for(int i = 0; i < buttonList.Count; i++)
        {
            //Note: Had to separate the targetTab and targetButton from the event since it takes the actual list reference and fails if I don't.
            TabWindow targetTab = tabList[i];
            Button targetButton = buttonList[i];
            buttonList[i].onClick.AddListener( () => ToggleTab(targetTab, targetButton));
        }
    }

    /// <summary>
    /// Toggles the given tab on after disabling all other tabs.
    /// </summary>
    /// <param name="tab_on"></param>
    void ToggleTab(TabWindow tab_on, Button button)
    {
        foreach(TabWindow tab in tabList)
        {
            if(tab != tab_on)
            {
                tab.DisableWindow();
            }
        }

        //Set interactable for all buttons to true
        foreach(Button but in buttonList)
        {
            but.interactable = true;
        }
        
        //Enabling the window after things are disabled just so we don't get any flickering of objects being moved by unity's layout system
        tab_on.EnableWindow();
        button.interactable = false;
    }
}
