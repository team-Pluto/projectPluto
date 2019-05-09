using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Literally just makes this obj face the player.
/// </summary>
public class FacePlayer : MonoBehaviour
{
    //The player game object
    GameObject player;

    //Cur velocity
    Vector3 cur_velocity;

    //How snappy the damp is.
    float dampSpeed = 0.1f;

    //How much of a difference in the angle before we stop rotating (prevents jittering)
    float diff_threshold = 10f;

    private void Start()
    {
        player = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Makes this look at the player. Does so on all axis.
        //transform.LookAt(player.transform);

        //Calculate the rot required to face the player. Only rotating on y axis.
        //This is the vector line from us to the player
        Vector3 toPlayer = transform.position - player.transform.position;

        //Calculate the angle between our forward and that line
        float angle = Vector3.SignedAngle(transform.forward, toPlayer, Vector3.up);

        //Only rotate if we are above the threshold.
        if(Mathf.Abs(angle) > diff_threshold)
        {
            //Get the target rotation
            Vector3 targetRot = Vector3.SmoothDamp(transform.eulerAngles,
                transform.eulerAngles + new Vector3(0, Mathf.Floor(angle), 0), ref cur_velocity, dampSpeed);

            //Lerp to targetRotation.
            transform.rotation = Quaternion.Euler(targetRot);
        }
    }
}
