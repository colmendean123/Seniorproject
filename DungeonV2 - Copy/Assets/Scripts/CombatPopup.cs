using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePopup : MonoBehaviour
{
    public bool doWindow0 = true;

    // Make the contents of the window.
    void DoWindow0(int windowID)
    {
        GUI.Button(new Rect(10, 30, 80, 20), "Click Me!");
    }

    void OnGUI()
    {
        // Make a toggle button for hiding and showing the window
        doWindow0 = GUI.Toggle(new Rect(10, 10, 100, 20), doWindow0, "Window 0");

        // Make sure we only call GUI.Window if doWindow0 is true.
        if (doWindow0)
        {
            GUI.Window(0, new Rect(110, 10, 200, 60), DoWindow0, "Basic Window");
        }
    }

}

