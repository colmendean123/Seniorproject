using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{

    private string tiles = "0, 0, 0,\n0, 1, 0,\n1, 1, 1,";
    private float defsize = 0.32f;

    private static bool[,] walls = new bool[0,0];
    private float size;
    private static int lenx = 0, leny = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Init();
        tiles = GameManager.GetMap(GameManager.GetMapName()+"tile.txt");
        size = defsize;
        ParseString(tiles);
    }

    //parse the tileset into line-by-line chunks
    private void ParseString(string str){
        string[] parse = str.Split('\n');
        string[] bin = new string[parse.Length];
        for(int i = 0; i < bin.Length; ++i){
            bin[i] = "";
            string[] con = parse[i].Split(',');
            for(int j = 0; j < con.Length; ++j){
                bin[i] += con[j].Trim();
            }
        }
        LoadTiles(bin);
    }


    private void LoadTiles(string[] map){
        //map out the objects that place the black collision tiles into the game
        float posx = 0f;
        float posy = 0f;
        int posxint = 0;
        int posyint = 0;
        leny = map.Length;
        lenx = map[0].Length;
        walls = new bool[lenx, leny];
        foreach(string i in map){
            foreach(char j in i){
                if(j=='1'){
                    //creates the visible tile
                    var wall = Resources.Load("Prefabs/Wall") as GameObject;
                    var wallinst = GameObject.Instantiate(wall, this.transform.position, this.transform.rotation);
                    wallinst.transform.position += new Vector3(posx, posy, 0f);
                    //inserts into collision map
                    walls[posxint, posyint] = true;
                }
                else{
                    walls[posxint, posyint] = false;
                }
                posxint+=1;
                posx+=size;
             }
            posx=0f;
            posxint = 0;
            posyint += 1;
            posy-=size;
        }
        return;
    }

    public static bool CheckCollision(int x, int y){
        if(x < 0 || y < 0 || x > lenx-1 || y > leny-1)
            return true;
        return walls[x, y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
