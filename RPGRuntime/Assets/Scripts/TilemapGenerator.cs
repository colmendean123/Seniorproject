using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{

    private string deftiles = "1, 0, 1,\n1, 0, 1,\n1, 1, 1,";
    private float defsize = 0.32f;
    private float size;
    // Start is called before the first frame update
    void Start()
    {
        size = defsize;
        ParseString(deftiles);
    }

    private void ParseString(string str){
        string[] parse = str.Split('\n');
        string[] bin = new string[parse.Length];
        for(int i = 0; i < bin.Length; ++i){
            bin[i] = "";
            string[] con = parse[i].Split(',');
            for(int j = 0; j < con.Length; ++j){
                bin[i] += con[j].Trim();
            }
            Debug.Log(bin[i]);
        }
        LoadTiles(bin);
    }

    private void LoadTiles(string[] map){
        float posx = 0f;
        float posy = 0f;
        foreach(string i in map){
            foreach(char j in i){
                if(j=='1'){
                    var wall = Resources.Load("Prefabs/Wall") as GameObject;
                    var wallinst = GameObject.Instantiate(wall, this.transform.position, this.transform.rotation);
                    wallinst.transform.position += new Vector3(posx, posy, 0f);
                    
                }
                posx+=size;
             }
            posx=0f;
            posy-=size;
        }
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
