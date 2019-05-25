using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Similar to PanStationary, but instead we rotate the along a pivot.
/// </summary>
public class PanCarousel : MonoBehaviour
{
    /// <summary>
    /// The pivot from which we rotate around.
    /// </summary>
    public Transform camera_pivot;

    /// <summary>
    /// Saves distance to pivot.
    /// </summary>
    public float dist_to_pivot;

    /// <summary>
    /// Rotational speed multipliers
    /// 
    /// </summary>
    public float horizontal_rot_speed, vertical_rot_speed;

    // Update is called once per frame
    void Update()
    {
        //Get cur rot output
        Vector2 rotDelta = GetComponent<PlayerPanCamController>().cur_rot;

        transform.RotateAround(camera_pivot.position, Vector3.up, rotDelta.y);

        float rotX = transform.eulerAngles.x + rotDelta.x;

        //Rotation can be over 180, in which case we want to flip it.
        rotX = Mathf.Clamp((rotX <= 180) ? rotX : -(360 - rotX), -90, 90);

        //Create target rot
        Vector3 targetRot = new Vector3(
            rotX,
            transform.eulerAngles.y,
            transform.eulerAngles.z);

        //Set rotation
        transform.eulerAngles = targetRot;
    }
}
