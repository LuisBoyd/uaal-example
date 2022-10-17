using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NativeBridge : MonoBehaviour
{
    public Text text;    
    void appendToText(string line) { text.text += line + "\n"; }

    void AppendLocation(string cmd)
    {
        appendToText($"Changing Location to {cmd}");
    }
}
