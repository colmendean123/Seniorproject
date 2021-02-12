using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDScript : MonoBehaviour
{
    private string[] lines = {"", "", "", "", ""};

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
        string conc = "";
        for(int i = 0; i < lines.Length; ++i){
            conc += lines[i] + "\n";
        }
        this.gameObject.GetComponent<TextMeshProUGUI>().text = conc;
    }
}
