using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scales all child objects to create zooming.
/// NOTE: All children should be of scale 1 so that they can be scaled/moved around easily.
/// </summary>
public class MapZoom : MonoBehaviour
{
    //What the scale is for everything
    public float scaleRatio = 1.0f, minScale = 0.01f, maxScale = 5.0f;

    // Update is called once per frame
    void Update()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        scaleRatio = Mathf.Clamp(scaleRatio + mouseWheel, minScale, maxScale);

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
        }
    }
}
