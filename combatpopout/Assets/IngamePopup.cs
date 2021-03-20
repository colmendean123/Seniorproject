using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool showPopUp = true;

    void Update()
    {

    }

    void OnGUI()
    {
        
            GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
                   , 300, 250), ShowGUI, "Invalid word");

        
    }

    void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        GUI.Label(new Rect(65, 40, 200, 30), "PUT YOUR MESSAGE HERE");

        // You may put a button to close the pop up too

        if (GUI.Button(new Rect(50, 150, 75, 30), "OK"))
        {
            showPopUp = false;
            // you may put other code to run according to your game too
        }

    }


}
