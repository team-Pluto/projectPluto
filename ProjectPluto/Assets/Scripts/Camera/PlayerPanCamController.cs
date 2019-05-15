using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player camera controller
/// </summary>
public class PlayerPanCamController : MonoBehaviour
{
    //Camera state variables.
    enum CameraState { Locked, Unlocked };
    [SerializeField]
    CameraState state;

    /// <summary>
    /// Multiplier for how fast you rotate horizontally.
    /// </summary>
    public float horizontal_rot_speed = 0.5f;

    /// <summary>
    /// Multiplier for how fast you rotate up.
    /// </summary>
    public float vertical_rot_speed = 0.1f;

    /// <summary>
    /// Whether or not yo uwant inverted y.
    /// </summary>
    public bool invert_y = false;

    /// <summary>
    /// How much of the screen is used for horizontal panning. 5 means one fifth is used for left pan, one fifth used for right pan.
    /// So only 3/5s are left for gameplay.
    /// </summary>
    public float width_section_size = 5;

    /// <summary>
    /// How much of the screen is used for vertical panning. 5 means one fifth slice is used for bottom panning, and another one fifth slice is used for up panning.
    /// So only 3/5s of the screen is left without panning.
    /// </summary>
    public float height_section_size = 5;

    void Start()
    {
        //Lock the cursor to start
        Cursor.lockState = CursorLockMode.Confined;
        state = CameraState.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //If it's locked do normal gameplay stuff
        if (state == CameraState.Locked)
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

        //Get Mouse positions
        Vector2 mousePos = Input.mousePosition;

        //Init rot delta
        Vector2 rotDelta = Vector2.zero;

        float width_section = Screen.width / width_section_size;
        float height_section = Screen.height / height_section_size;

        //Bound the screen so delta is modified accordingly.
        //Rot delta y is y axis rotation, which uses x mouse position.
        if(mousePos.x <= width_section)
        {
            rotDelta.y = -(1 - mousePos.x / width_section);
        }
        else if(mousePos.x >= width_section * (width_section_size - 1))
        {
            rotDelta.y = -(1 - ((mousePos.x - (width_section * (width_section_size - 2))) / width_section));
        }

        if(mousePos.y <= height_section)
        {
            rotDelta.x = -(1 - mousePos.y / height_section);
        }
        else if(mousePos.y >= height_section * (height_section_size - 1))
        {
            rotDelta.x = -(1 - ((mousePos.y - (height_section * (height_section_size - 2))) / height_section));
        }

        //If we don't want inverted, then invert it. (Conventionally inverted controls means noninverted in the numbers).
        if (!invert_y)
        {
            rotDelta.x = -rotDelta.x;
        }

        //Apply rotational multipliers.
        rotDelta.y *= horizontal_rot_speed;
        rotDelta.x *= vertical_rot_speed;

        float rotX = transform.eulerAngles.x + rotDelta.x;

        //Rotation can be over 180, in which case we want to flip it.
        rotX = Mathf.Clamp((rotX <= 180) ? rotX : -(360 - rotX), -90, 90);

        //Create target rot
        Vector3 targetRot = new Vector3(
            rotX,
            transform.eulerAngles.y + rotDelta.y,
            transform.eulerAngles.z);

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
        Cursor.lockState = CursorLockMode.Confined;
        state = CameraState.Locked;
    }
}
