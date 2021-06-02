using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripting;

public class HUDScript : MonoBehaviour
{
    private string[] lines = {"", "", "", "", ""};
    private string[] saved = { "", "", "", "", ""};

    //add a new line to the array of prints
    public void Print(string ln){
        for(int i = lines.Length-1; i > 0; --i){
            lines[i] = lines[i-1];
        }
        lines[0] = ln;
        UpdateHUD(lines) ;
    }

    public void PrintPerm(string ln)
    {
        for (int i = 0; i < saved.Length-1; ++i)
        {
            saved[i] = saved[i + 1];
        }
        saved[4] = ln;
        UpdateHUD(saved);
    }


    // Update is called once per frame
    void UpdateHUD(string[] ln)
    {
        
        //put all the console lines together
        string conc = "";
        for(int i = 0; i < ln.Length; ++i){
            conc += ln[i] + "\n";
        }
        this.gameObject.GetComponent<TextMeshProUGUI>().text = conc;
    }




    public void GetSaved()
    {
        Debug.Log("LOADED");
        UpdateHUD(saved);
    }

    private void Update()
    {
       
    }

}
