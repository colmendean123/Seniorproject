using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{

    private float defsize = 0.32f;

    public static bool[,] walls = new bool[0,0];
    private float size;
    private static int lenx = 0, leny = 0;
    Dictionary<string, int> assignid;
    
    // Start is called before the first frame update
    void Start()
    {
        
        LoadCurrentMap();
        GameManager.Init();
    }

    
    //Loads the current map files. Does not replace the map currently loaded
    public void LoadCurrentMap()
    {
        GameManager.WipeObjects();
        string tiles = GameManager.GetMap(GameManager.GetMapName() + "tile.txt");
        string objects = GameManager.GetMap(GameManager.GetMapName() + "object.txt");
        string arts = GameManager.GetMap(GameManager.GetMapName() + "background.txt");
        size = defsize;

        ParseCollision(tiles);
        ParseArt(arts);
        ParseObjects(objects);
    }

    //parse the tileset into line-by-line chunks
    //Format: 1, 0, 1, 1, 0, ect
    private void ParseCollision(string str){
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


    private void ParseObjects(string str)
    {
        //GameObject scr = GameObject.Find("ScriptRunner");
        //scr.GetComponent<RPGObject>().Initialize("");
        assignid = new Dictionary<string, int>();
        string[] bin = str.Split('\n');
        foreach(string i in bin)
        {
            ParseObjectPlacement(i);
        }
    }


    //place objects for placement on the map
    //Format: "Objectname" X: xpos Y: ypos
    private void ParseObjectPlacement(string line)
    {
        Scripting.Tokenizer token = new Scripting.Tokenizer(line, null, null);
        //obj1
        string obj = token.GetNext();
        if (assignid.ContainsKey(obj))
        {
            assignid[obj]++;
        }
        else
        {
            assignid[obj] = 1;
        }
        int assign = assignid[obj];
        //set up x and y pos
        int xpos, ypos;
        //X:
        token.GetNext();
        //xpos
        int.TryParse(token.GetNext(), out xpos);
        //Y:
        token.GetNext();
        //ypos
        int.TryParse(token.GetNext(), out ypos);

        var prefab = Resources.Load("Prefabs/Object") as GameObject;
        var inst = GameObject.Instantiate(prefab, this.transform.position, this.transform.rotation);
        inst.name = obj;
        
        string next = token.GetNext();

        //Camera defaults to player
        if (next != null)
        {
            if (next == "PLAYER")
            {
                GameManager.SetPlayer(inst);
                inst.AddComponent(typeof(PlayerMovement));
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().AssignTarget(inst);
            }
            else
                inst.AddComponent(typeof(ObjectAI));

        }
        else
        {
            inst.AddComponent(typeof(ObjectAI));
        }
        //set correct position
        inst.GetComponent<RPGObject>().SetPosition(xpos, ypos);
        inst.transform.position += new Vector3(xpos*0.32f, ypos*0.32f, 0f);
        inst.GetComponent<SpriteRenderer>().enabled = true;
        inst.GetComponent<RPGObject>().SetID(assign);
        inst.GetComponent<RPGObject>().Initialize(obj);
    }

    private void ParseArt(string str)
    {
        string[] parse = str.Split('\n');
        string[] bin = new string[parse.Length];
        for (int i = 0; i < bin.Length; ++i)
        {
            bin[i] = "";
            string[] con = parse[i].Split(',');
            for (int j = 0; j < con.Length; ++j)
            {
                bin[i] += con[j].Trim() + ",";
            }
        }
        LoadArt(bin);
    }

    public string[,] GetMap()
    {
        //Get basic string map conversions
        string[,] map = new string[lenx, leny];
        for(int x = 0;  x < walls.GetLength(0); ++x)
        {
            for (int y = 0; y < walls.GetLength(1); ++y)
            {
                if (walls[x, y] == false)
                    map[x, y] = "0";
                else
                    map[x, y] = "1";
            }
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Object");
        foreach(GameObject i in objs)
        {
            i.GetComponent<RPGObject>().GetLocation(out int x, out int y);
            map[x, y] = i.name;
            
        }
        return map;
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
                    wallinst.GetComponent<SpriteRenderer>().enabled = false;
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

    private void LoadArt(string[] map)
    {
        //map out the objects that place the black collision tiles into the game
        Dictionary<string, Texture2D> spritedict = new Dictionary<string, Texture2D>();
        float posx = 0f;
        float posy = 0f;
        int posxint = 0;
        int posyint = 0;
        leny = map.Length;
        lenx = map[0].Length;
        foreach (string i in map)
        {
            foreach (string j in i.Split(','))
            {
                string spr = j.Trim();
                if (spr == "")
                    continue;
                if (!spritedict.ContainsKey(spr))
                {
                    spritedict.Add(spr, GameManager.LoadTexture(GameManager.GetPath() + "\\Tiles\\" + spr + ".png"));
                }
                Texture2D SpriteTexture = spritedict[spr];
                SpriteTexture.filterMode = FilterMode.Point;
                UnityEngine.Sprite newsprite = UnityEngine.Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.Tight);
                spr = spr.Trim();
                //creates the visible tile and loads in the new texture
                var wall = Resources.Load("Prefabs/Wall") as GameObject;
                var wallinst = GameObject.Instantiate(wall, this.transform.position, this.transform.rotation);
                wallinst.GetComponent<SpriteRenderer>().sprite = newsprite;
                wallinst.transform.position += new Vector3(posx, posy, 0f);
                posxint += 1;
                posx += size;
            }
            posx = 0f;
            posxint = 0;
            posyint += 1;
            posy -= size;
        }
        return;
    }

    public static bool CheckCollision(int x, int y){
        if(x < 0 || y < 0 || x > lenx-1 || y > leny-1)
            return true;
        return walls[x, y];
    }

    public (int, int) GetDimensions()
    {
        return (lenx, leny);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
