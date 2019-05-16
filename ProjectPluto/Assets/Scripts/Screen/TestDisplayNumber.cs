using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Just a simple test script to test some screen interactions
/// </summary>
public class TestDisplayNumber : MonoBehaviour
{
    Text textComponent;
    
    /// <summary>
    /// Set the text to the given text.
    /// </summary>
    /// <param name="in_text"></param>
    public void SetText(string in_text)
    {
        if(textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }

        if(textComponent != null)
        {
            textComponent.text = in_text;
        }
    }
}
