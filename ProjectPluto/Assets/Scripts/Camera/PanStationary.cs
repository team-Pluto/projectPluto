using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes the outputted -1-to-1 values and uses it to rotate in place.
/// </summary>
public class PanStationary : MonoBehaviour
{
    //Multipliers for rotational speeds
    public float horizontal_rot_speed = 0.5f, vertical_rot_speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector2 rotDelta = GetComponent<PlayerPanCamController>().cur_rot;

        //Apply rotational speeds
        rotDelta.x *= vertical_rot_speed;
        rotDelta.y *= horizontal_rot_speed;

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
}
