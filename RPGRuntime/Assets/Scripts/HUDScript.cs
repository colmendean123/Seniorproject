using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripting;

public class HUDScript : MonoBehaviour
{
    private string[] lines = {"", "", "", "", ""};

    //add a new line to the array of prints
    public void Print(string ln){
        for(int i = lines.Length-1; i > 0; --i){
            lines[i] = lines[i-1];
        }
        lines[0] = ln;
        UpdateHUD();
    }


    // Update is called once per frame
    void UpdateHUD()
    {
        //put all the console lines together
        string conc = "";
        for(int i = 0; i < lines.Length; ++i){
            conc += lines[i] + "\n";
        }
        this.gameObject.GetComponent<TextMeshProUGUI>().text = conc;
    }

    private void Update()
    {
       
    }

}
