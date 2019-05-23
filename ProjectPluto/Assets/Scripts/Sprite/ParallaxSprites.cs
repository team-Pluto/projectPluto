using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parallaxes sprites depending on player rotation relative to sprite location.
/// </summary>
public class ParallaxSprites : MonoBehaviour
{
    //Which camera to be relativeTo
    public Camera relativeTo;

    //How large of a window of angles does this effect change in? When does it reach maximum displacement
    public float hor_window = 45f, ver_window = 45f;

    //What's the maximum drift of a sprite
    public float hor_max_drift = 0.03f, ver_max_drift = 0.03f;

    /// <summary>
    /// How drifty it is when moving the sprites.
    /// </summary>
    public float smoothTime = 0.05f;

    /// <summary>
    /// Separation between layers
    /// </summary>
    public float separation = 0.2f;

    //How much we need to move for 1 degree difference. Calculated at run time.
    //Based off of max_drift / window. 
    float hor_angleStep = 0, ver_angleStep = 0;

    //Target position
    Vector3 targetPos = Vector3.zero, refVelocity;

    private void Start()
    {
        //Inefficient to use Camera.main. Write a menuitem that will auto fill this for us in the future.
        if(relativeTo == null)
        {
            relativeTo = Camera.main;
        }

        //Map the window we want to apply drift with the max drift.
        hor_angleStep = hor_max_drift / hor_window;
        ver_angleStep = ver_max_drift / ver_window;
    }

    private void Update()
    {
        //Get line from player camera to us
        Vector3 toUs = transform.position - relativeTo.transform.position;
        Vector3 toUsProjVert = Vector3.ProjectOnPlane(toUs, transform.up);
        Vector3 toUsProjHor = Vector3.ProjectOnPlane(toUs, transform.right);

        //Camera vectors excluding an axis
        Vector3 camProjVert = Vector3.ProjectOnPlane(relativeTo.transform.forward, transform.up);
        Vector3 camProjHor = Vector3.ProjectOnPlane(relativeTo.transform.forward, transform.right);

        //Calculate angle at which player is looking at us, as in between the toUs line and the camera forward line.
        //Only in the 2D top down way. No angles in other dimensions.
        float horizontal_angle = -Vector3.SignedAngle(toUsProjVert, camProjVert, transform.up);
        float vertical_angle = -Vector3.SignedAngle(toUsProjHor, camProjHor, transform.right);

        //Translate the layers based on their numbers as well as the angle
        for(int i = 0; i < transform.childCount; i++)
        {
           targetPos = new Vector3(
                Mathf.Clamp(i * horizontal_angle * hor_angleStep, -hor_max_drift * i, hor_max_drift * i),
                Mathf.Clamp(i * vertical_angle * ver_angleStep, -ver_max_drift * i, ver_max_drift * i),
                i * separation);

           Transform child = transform.GetChild(i);

           child.localPosition = Vector3.SmoothDamp(child.localPosition, targetPos, ref refVelocity, smoothTime);
        }

        

    }
}
