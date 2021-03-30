using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGExceptions;

public class RPGObject : MonoBehaviour
{
    SortedDictionary<string,int> intvars;
    SortedDictionary<string,string> stringvars;
    SortedDictionary<string, string[]> functions;
    int logicdepth = 0;
    string[] inputs;
    int step;
    protected bool locked = false;
    new protected bool camera = false;
    protected int posx = 2;
    protected int posy = 2;
    private int ID = 1;

    public static GameObject FindWithID(string name, int id)
    {
        List<GameObject> possibilities = new List<GameObject>();
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Object"))
        {
            if (i.name == name)
            {
                id--;
                if (id == 0)
                    return i;
            }
        }
        return null;
    }

    //onstart, load default variables. Then load parameters.
    protected void Start(){
        DoScript("testdialogue.txt");
    }

    public void SetID(int i)
    {
        ID = i;
    }

    public int GetID() {
        return ID;
    }

    public void Initialize(string name)
    {
        //start the dictionary for int and strings
        intvars = new SortedDictionary<string, int>();
        stringvars = new SortedDictionary<string, string>();
        functions = new SortedDictionary<string, string[]>();
        Debug.Log("GO");
        ChangeInt("HP", 10);
        ChangeInt("DEF", 3);
        ChangeInt("ATTACK", 3);
        ChangeString("name", gameObject.name);
        ChangeString("sprite", "player.png");
        LoadParameters(name+".txt");
        LoadSprite(GetString("sprite"));
    }
    
    //Generatepath finds the smallest possible path between an object and another. Obviously useful for aggro path finding. Could be reformatted to target points instead.
    public PathingInfo GeneratePath(string target, bool throughobjects)
    {
        string[,] map = GameObject.FindGameObjectWithTag("Manager").GetComponent<TilemapGenerator>().GetMap();
        (int, int) dimensions = GameObject.FindGameObjectWithTag("Manager").GetComponent<TilemapGenerator>().GetDimensions();
        int lenx = dimensions.Item1;
        int leny = dimensions.Item2;
        bool[,] visited = new bool[lenx, leny];
        for(int i = 0; i < lenx; ++i)
        {
            for(int j = 0; j < leny; ++j)
            {
                visited[i, j] = false;
            }
        }
        //Generate path start will return a full list of all possible paths without re-traversing. Minimizes created paths.
        //if paths has 0 entries there are either no valid paths or
        List<PathingInfo> paths = GeneratePathStart(map, ref visited, target);
        PathingInfo finalpath = paths[0];
        foreach(PathingInfo path in paths)
        {
            if (finalpath.length > path.length)
                if (path.throughobjects == true)
                {
                    //throughobjects checks if the user wants the path to go through things or not.
                    if (throughobjects == true)
                        finalpath = path;
                }
                else
                {
                    finalpath = path;
                }
        }
        return finalpath;
    }

    //starts the ref list for pathiterate
    public List<PathingInfo> GeneratePathStart(string[,] map, ref bool[,] visited, string target)
    {
        List<PathingInfo> successfulpaths = new List<PathingInfo>();
        PathingInfo path = new PathingInfo(posx, posy, new PathingInfo());
        PathIterate(map, ref visited, target, ref successfulpaths, path);
        return successfulpaths;
    }

    //iterates 4 times: Once for every direction.
    public void PathIterate(string[,] map, ref bool[,] visited, string target, ref List<PathingInfo> success, PathingInfo prev)
    {

        int x, y;
        x = prev.GetX(prev.length);
        y = prev.GetY(prev.length);
        PathingInfo path1 = CreatePath(prev, map, ref visited, target, x + 1, y);
        PathingInfo path2 = CreatePath(prev, map, ref visited, target, x - 1, y);
        PathingInfo path3 = CreatePath(prev, map, ref visited, target, x, y + 1);
        PathingInfo path4 = CreatePath(prev, map, ref visited, target, x, y - 1);
        if(!(PathCheck(path1, ref success)))
        {
            PathIterate(map, ref visited, target, ref success, path1);
        }
        if (!(PathCheck(path2, ref success)))
        {
            PathIterate(map, ref visited, target, ref success, path2);
        }
        if (!(PathCheck(path3, ref success)))
        {
            PathIterate(map, ref visited, target, ref success, path3);
        }
        if (!(PathCheck(path4, ref success)))
        {
            PathIterate(map, ref visited, target, ref success, path4);
        }

    }

    public bool PathCheck(PathingInfo path, ref List<PathingInfo> success)
    {
        //checks if the path is done. If not, returns true to tell the iterator to continue iterating.
        if (path == null)
            return true;
        if (path.success == true)
            success.Add(path);
        else
            return false;
        return true;
    }

    PathingInfo CreatePath(PathingInfo prev, string[,] map, ref bool[,] visited, string target, int x, int y)
    {
        //All failure conditions:
        //out of index
        if (x < 0 || y < 0)
            return null;
        if (x > map.GetLength(0) || y > map.GetLength(1))
            return null;
        //Wall in the way
        if (map[x, y] == "1")
            return null;
        if(visited[x, y] == true)
            return null;
        PathingInfo ret = new PathingInfo(x, y, prev);
        if (map[x, y] == target)
            ret.success = true;
        //check through objects
        else if (map[x, y] != "0")
            ret.throughobjects = true;
        visited[x, y] = true;
        return ret;
    }

    protected void Update()
    {
        if (camera)
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(posx * 0.32f, -posy * 0.32f, -10f);
        transform.position = new Vector2(posx * 0.32f, -posy * 0.32f);
    }

    //Load parameters from the object file
    public void LoadParameters(string name){
        string[] paramaters = GameManager.LoadFile("Objects", name);
        foreach(string str in paramaters){
            //split into key value pair
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            //check if int or string
            if(int.TryParse(value, out int i)){
                ChangeInt(key, i);
            }
            else{
                ChangeString(key, value);
            }
            this.gameObject.name = GetString("NAME");
        }

    }
    //scripting on a local level
    public void DoScript(string script){
        inputs = GameManager.LoadFile("Scripts", script);
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(inputs, this.gameObject);
        cmd.ExecuteStep(0);
    }

    public void StartScript(int step, string[] inputs){
        string input = inputs[step];
        GameObject.FindGameObjectWithTag("Manager").GetComponent<CommandRouter>().Execute(gameObject, ref logicdepth);
    }

    public void DoFunction(string func)
    {
        
        string[] inputs = functions[func];
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(inputs, this.gameObject);
        cmd.ExecuteStep(0);
    }

    public void Nextstep(int step, string[] inputs){
        if(step < inputs.Length){
            StartScript(step, inputs);
        }
    }

    //lock the player into place
    public void Lock(bool tf){
        this.locked = tf;
        this.gameObject.GetComponent<CommandRouter>().Nextstep();
    }

    //Load the sprite from an external sprite path
    public void LoadSprite(string spr)
    {
        Texture2D SpriteTexture = GameManager.LoadTexture(GameManager.GetPath()+"\\Sprites\\"+spr);
        SpriteTexture.filterMode = FilterMode.Point;
        UnityEngine.Sprite newsprite = UnityEngine.Sprite.Create(SpriteTexture, new Rect(0,0, SpriteTexture.width, SpriteTexture.height),  new Vector2(0.5f, 0.5f), 100f, 0 , SpriteMeshType.Tight);
        this.GetComponent<SpriteRenderer>().sprite = newsprite;
    }

    public void GetLocation(out int x, out int y)
    {
        x = posx;
        y = posy;
    }

    //check if the keys are in the dict.
    public bool IsVar(string key){
        key = key.Trim().ToUpper();
        return intvars.ContainsKey(key) || stringvars.ContainsKey(key);
    }

    public bool IsString(string key){
        key = key.Trim().ToUpper();
        return stringvars.ContainsKey(key);
    }

    public bool IsInt (string key){
        key = key.Trim().ToUpper();
        return intvars.ContainsKey(key);
    }

    //Get the keys from the dict by key
    public int GetInt(string key){
        key = key.ToUpper();
        if(intvars.ContainsKey(key))
            return intvars[key];
        else
            throw new ObjectVariableException("Error! Object does not contain int \""+key+"\"");
    }

    public string GetString(string key){
        key = key.ToUpper();
        if(stringvars.ContainsKey(key))
            return stringvars[key];
        else
            throw new ObjectVariableException("Error! Object does not contain string \""+key+"\"");
    }

    //change or initialize new vars
    public void ChangeString(string key, string var){
        key = key.ToUpper();
        if(!stringvars.ContainsKey(key))
            stringvars.Add(key, var);
        else
            stringvars[key] = var;
    }

    public void ChangeInt(string key, int var){
        key = key.ToUpper();
        if(!intvars.ContainsKey(key))
            intvars.Add(key, var);
        else
            intvars[key] = var;
    }

    private void ScanFunctions(string fname)
    {
        string[] inp = GameManager.LoadFile("Scripts", fname);
    }
}
