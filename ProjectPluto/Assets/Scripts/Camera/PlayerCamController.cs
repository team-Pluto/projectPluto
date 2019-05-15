using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player camera controller
/// </summary>
public class PlayerCamController : MonoBehaviour
{
    //Camera state variables.
    enum CameraState { Locked, Unlocked };
    [SerializeField]
    CameraState state;

    /// <summary>
    /// Mouse sensitivity variable
    /// </summary>
    public float mouse_sensitivity = 1;

    /// <summary>
    /// Whether or not to invert mouse y
    /// </summary>
    public bool invert_y = false;

    void Start()
    {
        //Lock the cursor to start
        Cursor.lockState = CursorLockMode.Locked;
        state = CameraState.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //If it's locked do normal gameplay stuff
        if(state == CameraState.Locked)
        {
            HandleLocked();
        }//If it's unlocked, do menu stuff.
        else if (state == CameraState.Unlocked)
        {
            HandleUnlocked();
        }
    }

    /// <summary>
    /// Handles the camera when locked.
    /// </summary>
    void HandleLocked()
    {
        //If we get the cancel key unlock the mouse from camera
        if (Input.GetButtonDown("Cancel"))
        {
            UnlockCamera();
        }

        //Rotate the camera based on axis
        Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * mouse_sensitivity;

        //Invert the x value if players don't want it inverted. (Conventionally flipped is not inverted.)
        if (!invert_y)
        {
            mouseDelta.x = -mouseDelta.x;
        }

        //Manipulate x angle so it doesnt go past -90 or 90.
        float x = transform.eulerAngles.x + mouseDelta.x;

        //Rotation can be over 180, in which case we want to flip it.
        float rotX = Mathf.Clamp((x <= 180) ? x : -(360 - x), -90, 90);

        //Create target rot
        Vector3 targetRot = new Vector3(
            rotX, 
            transform.eulerAngles.y + mouseDelta.y, 
            transform.eulerAngles.z + mouseDelta.z);

        //Set rotation
        transform.eulerAngles = targetRot;
    }

    /// <summary>
    /// Handles the camera when unlocked.
    /// </summary>
    void HandleUnlocked()
    {
        //If we get the mouse button one key lock the mouse back to camera
        if (Input.GetButtonDown("Fire1"))
        {
            LockCamera();
        }
    }

    /// <summary>
    /// Helper function taht unlocks the camera and changes the state
    /// </summary>
    void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        state = CameraState.Unlocked;
    }

    /// <summary>
    /// Helper function that locks the camera and changes the state
    /// </summary>
    void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        state = CameraState.Locked;
    }
}
