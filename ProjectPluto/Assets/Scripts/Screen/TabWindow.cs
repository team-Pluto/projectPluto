using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggles the window being active or not.
/// </summary>
public class TabWindow : MonoBehaviour
{
   public void DisableWindow()
    {
        gameObject.SetActive(false);
    }

    public void EnableWindow()
    {
        gameObject.SetActive(true);
    }
}
