using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that allows a window to hide. 
/// </summary>
public class HideableWindow : MonoBehaviour
{
    //The button that we're using
    public Button toggleButton;

    //The transform points the window should move to when its hidden or showing.
    public Transform showTransform, hideTransform;

    //States for whether the window is showing or hidden
    enum WindowState { Hidden, Transitioning, Showing };
    WindowState state = WindowState.Hidden;

    //Target position
    Vector3 targetPos = Vector3.zero, refVelocity = Vector3.zero;

    //Smooth time
    public float smooth = 0.5f;

    //Toggles window visibility.
    public void ToggleWindow()
    {
        //Check targetPos so we can toggle if its moving in a direction already
        if(state == WindowState.Hidden || targetPos == hideTransform.position)
        {
            targetPos = showTransform.position;
            state = WindowState.Transitioning;
        }
        else if(state == WindowState.Showing || targetPos == showTransform.position)
        {
            targetPos = hideTransform.position;
            state = WindowState.Transitioning;
        }
        //If we toggled, make the button uninteractable.
        //toggleButton.interactable = false;
    }

    private void Update()
    {
        //Only do stuff if the window needs to move
        if (state == WindowState.Transitioning)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refVelocity, smooth);
            if (transform.position == targetPos)
            {
                //Set the state based on which position we've arrived at.
                if (targetPos == showTransform.position)
                {
                    state = WindowState.Showing;
                }
                else if (targetPos == hideTransform.position)
                {
                    state = WindowState.Hidden;
                }

                //Reset interactability
                //toggleButton.interactable = true;
            }
        }
    }
}
