using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FPS cursor implementation.
/// </summary>
public class FPSAdaptiveCursor : MonoBehaviour
{
    RaycastHit rayHit = new RaycastHit();

    public Texture2D screenCursor, normalCursor;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if(Physics.Raycast(transform.position, transform.forward, out rayHit, 10000000f))
            {
                if (rayHit.collider.CompareTag("Screen"))
                {
                    GetComponent<PlayerFPSCamController>().UnlockCamera();
                }
                else
                {
                    Debug.Log(rayHit.collider.name);
                }
            }
        }
    }
}
