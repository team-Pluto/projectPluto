using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player camera controller
/// </summary>
public class PlayerFPSCamController : MonoBehaviour
{
    //Camera state variables.
    enum CameraState { Locked, Unlocked, ZoomingIn, ZoomingOut };
    [SerializeField]
    CameraState state;

    /// <summary>
    /// Whether or not to invert mouse y
    /// </summary>
    public bool invert_y = false;

    /// <summary>
    /// Whether or not the camera is frozen. 
    /// </summary>
    public bool camera_frozen = false;

    /// <summary> Maximum angle the camera can rotate </summary> 
    public Vector2 clampInDegrees = new Vector2(360, 180);
    /// <summary> Whether cursor should be locked </summary> 
    public bool lockCursor;
    /// <summary> mouse sensitivity</summary> 
    public Vector2 sensitivity = new Vector2(2, 2);
    /// <summary> smoothing applied when mouse moves </summary> 
    public Vector2 smoothing = new Vector2(3, 3);
    /// <summary> starting rotation of object </summary> 
    public Vector2 targetDirection;
    /// <summary> starting rotation of object </summary> 
    public Vector2 targetCharacterDirection;

    private Vector2 originaleStartingPosition;
    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.
    public GameObject characterBody;

    void Start()
    {
        //Lock the cursor to start
        
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        state = CameraState.Locked;
        originaleStartingPosition = transform.position;
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
        if (Input.GetButtonDown("Jump"))
        {
            UnlockCamera();
        }

        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //Invert the x value if players don't want it inverted. (Conventionally flipped is not inverted.)
        if (invert_y)
        {
            mouseDelta.y = -mouseDelta.y;
        }

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, Time.deltaTime * sensitivity.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, Time.deltaTime * sensitivity.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Then clamp and apply the global y value.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }

    /// <summary>
    /// Handles the camera when unlocked.
    /// </summary>
    void HandleUnlocked()
    {
        //If we get the mouse button one key lock the mouse back to camera
        if (Input.GetButtonDown("Jump"))
        {
            LockCamera();
        }
    }

    /// <summary>
    /// Zooms in the camera towards a specific object
    /// </summary>
    void HandleZoomingIn()
    {

    }

    /// <summary>
    /// Zooms out the camera towards a specific object
    /// </summary>
    void HandleZoomingOut()
    {

    }

    /// <summary>
    /// Helper function taht unlocks the camera and changes the state
    /// </summary>
    public void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        state = CameraState.Unlocked;
    }

    /// <summary>
    /// Helper function that locks the camera and changes the state
    /// </summary>
    public void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        state = CameraState.Locked;
    }
}
