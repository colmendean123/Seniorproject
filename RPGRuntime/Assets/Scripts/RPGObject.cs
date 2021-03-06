using RPGExceptions;
using System.Collections.Generic;
using UnityEngine;

public class RPGObject : MonoBehaviour
{
    public SortedDictionary<string,int> intvars;
    public SortedDictionary<string,string> stringvars;
    public SortedDictionary<string, string[]> functions;
    public SortedDictionary<string, string[]> actions;
    public List<string[]> attacks;
    public List<string> attacknames;
    public List<Item> inventory;
    int logicdepth = 0;
    string[] inputs;
    int step;
    public bool turn = false;
    protected bool locked = false;
    new protected bool camera = false;
    public int posx;
    public int posy;
    private int ID = 1;
    FunctionParser function;
    public GameObject target;
    public bool[] equippeditem = new bool[0];

    public static GameObject FindWithID(string name, int id)
    {
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Object"))
        {
            if (i.GetComponent<RPGObject>().GetString("NAME")==name)
            {
                id--;
                if (id == 0)
                    return i;
            }
        }
        return null;
    }

    public void AddToInventory(string item)
    {
        inventory.Add(new Item(item));
        RemoveFromInventory(null);
    }

    public void RemoveFromInventory(string item)
    {
        foreach(Item i in inventory)
        {
            if (i.GetName() == item)
            {
                if(i.equipped == true)
                {
                    i.UnEquip();
                }
                inventory.Remove(i);
                return;
            }
        }
    }

    public static GameObject FindWithName(string name)
    {
        List<GameObject> possibilities = new List<GameObject>();
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Object"))
        {
            if (i.GetComponent<RPGObject>().GetString("NAME") == name || i.gameObject.name == name)
            {
                return i;
            }
        }
        return null;
    }

    //onstart, load default variables. Then load parameters.
    protected void Start(){
    }

    public void AddMove(string filename)
    {
       
        string[] loadedattack = GameManager.LoadFile("Moves", filename);
        if (!attacknames.Contains(loadedattack[0].Substring(2)))
        {
            attacknames.Add(loadedattack[0].Substring(2));
            attacks.Add(loadedattack);
        }
    }

    public void RemoveMove(string filename)
    {

        string[] loadedattack = GameManager.LoadFile("Moves", filename);
        if (attacknames.Contains(loadedattack[0].Substring(2)))
        {
            attacknames.Remove(loadedattack[0].Substring(2));
            attacks.Remove(loadedattack);
        }
    }

    public void BeginTurn()
    {
        DoFunction("TURNSTART");
        turn = true;
    }

    public void EndTurn()
    {
        DoFunction("TURNEND");
        turn = false;
        //GameObject.FindGameObjectWithTag("Console").GetComponent<HUDScript>().GetSaved();
        GameManager.NextTurn();
    }

    //sets position for the game manager
    public void SetPosition(int x, int y)
    {
        TilemapGenerator.walls[posx, posy] = false;
        posx = x;
        posy = y;
        TilemapGenerator.walls[posx, posy] = true;
    }

    public (int, int) GetPosition()
    {
        return (posx, posy);
    }

    //Set Instance ID
    public void SetID(int i)
    {
        ID = i;
    }

    public int GetID() {
        return ID;
    }

    //Move
    public void Move(int x, int y)
    {
        if (TilemapGenerator.CheckCollision(x, y) == false)
        {
            TilemapGenerator.walls[posx, posy] = false;
            posx = x;
            posy = y;
            TilemapGenerator.walls[posx, posy] = true;
        }
    }

    //Scans for adjacencies and returns the result
    public List<GameObject> ScanForAdjacency()
    {
        List<GameObject> collisions = new List<GameObject>();
        GameObject obj = GameManager.GetObjectByPosition(posx, posy - 1);
        if (obj != null && obj.name!="Wall")
            collisions.Add(obj);
        obj = GameManager.GetObjectByPosition(posx, posy + 1);
        if (obj != null && obj.name != "Wall")
            collisions.Add(obj);
        obj = GameManager.GetObjectByPosition(posx-1, posy);
        if (obj != null && obj.name != "Wall")
            collisions.Add(obj);
        obj = GameManager.GetObjectByPosition(posx + 1, posy);
        if (obj != null && obj.name != "Wall")
            collisions.Add(obj);
        return collisions;
    }

    public void Initialize(string name)
    {
        //start the dictionary for int and strings
        intvars = new SortedDictionary<string, int>();
        stringvars = new SortedDictionary<string, string>();
        functions = new SortedDictionary<string, string[]>();
        actions = new SortedDictionary<string, string[]>();
        inventory = new List<Item>();
        attacks = new List<string[]>();
        attacknames = new List<string>();
        ChangeInt("HP", 10);
        ChangeInt("defense", 3);
        ChangeInt("attack", 3);
        ChangeInt("speed", 1);
        ChangeInt("slots", 5);
        ChangeString("noitem", "There are no items in my inventory!");
        ChangeString("name", gameObject.name);
        ChangeString("sprite", "player.png");
        if(name!="")
            LoadParameters(name+".txt");
        LoadSprite(GetString("sprite"));
        for(int i = 0; i < equippeditem.Length; ++i)
        {
            equippeditem[i] = false;
        }
        
    }

    public void Attack(int index, GameObject target)
    {
        string[] attack = attacks[index];
        string[] attackReplace = new string[attack.Length];
        for(int i = 0; i < attack.Length; ++i)
        {
            attackReplace[i] = attack[i].Replace("$this", "$" + this.gameObject.name);
            attackReplace[i] = attackReplace[i].Replace("$target", "$" + target.name);
        }
        DoMove(attackReplace);
    }
    
    public int FindDistance(GameObject target)
    {
  
        PathingInfo targetpath = GeneratePath(target.gameObject.name);
        return targetpath.distance;
    }

    //Generatepath finds the smallest possible path between an object and another. Obviously useful for aggro path finding. Could be reformatted to target points instead.
    public PathingInfo GeneratePath(string target)
    {
        string[,] map = GameObject.FindGameObjectWithTag("Manager").GetComponent<TilemapGenerator>().GetMap();
        (int, int) dimensions = GameObject.FindGameObjectWithTag("Manager").GetComponent<TilemapGenerator>().GetDimensions();
        int lenx = dimensions.Item1;
        int leny = dimensions.Item2;
        //Generate path start will return a full list of all possible paths without re-traversing. Minimizes created paths.
        //if paths has 0 entries there are either no valid paths or
        return PathFind(map, target);
    }

    //iterates 4 times: Once for every direction.
    public PathingInfo PathFind(string[,] map, string target)
    {
        //I'm going to kill myself. An implementation of Djikstra's theory.
        GameObject targ = FindWithName(target);
        int thisx = posx;
        int thisy = posy;
        int x = targ.GetComponent<RPGObject>().posx;
        int y = targ.GetComponent<RPGObject>().posy;
        bool targetfound = false;
        List<(int, int)> searched = new List<(int, int)>();
        List<(int, int)> tosearch = new List<(int, int)>();
        tosearch.Add((x, y));
        Dictionary<(int, int), int> distance = new Dictionary<(int, int), int>(); ;
        Dictionary<int, (int, int)> completed = new Dictionary<int, (int, int)>();
        int dist = 0;
        while(targetfound == false)
        {
            List<(int, int)> newentries = new List<(int, int)>();
            foreach ((int, int) pos in tosearch)
            {
                //only searched if not searched before
                if (!searched.Contains(pos))
                {
                    distance[pos] = 255;
                    searched.Add(pos);
                    //Fail conditions
                    if (map[pos.Item1, pos.Item2] == "1")
                    {
                        continue;
                    }
                    //Calamity
                    if (map[pos.Item1 , pos.Item2] == this.name)
                    {
                        //make sure the keys exist
                        int finaldist = 255;
                        if (distance.ContainsKey((pos.Item1 - 1, pos.Item2)))
                        {
                            finaldist = distance[(pos.Item1 - 1, pos.Item2)];
                            completed[finaldist] = (pos.Item1 - 1, pos.Item2);
                        }
                        if (distance.ContainsKey((pos.Item1 + 1, pos.Item2)))
                        {
                            finaldist = distance[(pos.Item1 + 1, pos.Item2)];
                            completed[finaldist] = (pos.Item1 + 1, pos.Item2);
                        }
                        if (distance.ContainsKey((pos.Item1, pos.Item2 + 1)))
                        {
                            finaldist = distance[(pos.Item1, pos.Item2 + 1)];
                            completed[finaldist] = (pos.Item1, pos.Item2 + 1);
                        }
                        if (distance.ContainsKey((pos.Item1, pos.Item2 - 1)))
                        {
                            finaldist = distance[(pos.Item1, pos.Item2 - 1)];
                            completed[finaldist] = (pos.Item1, pos.Item2 - 1);
                        }
                        targetfound = true;
                        continue;
                    }
                    newentries.Add((pos.Item1 - 1, pos.Item2));
                    newentries.Add((pos.Item1 + 1, pos.Item2));
                    newentries.Add((pos.Item1, pos.Item2 - 1));
                    newentries.Add((pos.Item1, pos.Item2 + 1));
                    
                    distance[pos] = dist;
                }
            }
            foreach ((int, int) i in newentries)
            {
                tosearch.Add(i);
            }
            ++dist;
            if(dist > 7)
            {
                targetfound = true;
                continue;
            }
           
        }
        //get the completed paths
        if (completed.Count == 0)
            return new PathingInfo((thisx, thisy), 100);
        else
        {
            int check = 0;
            while (!completed.ContainsKey(check))
            {
                
                ++check;
            }
            return new PathingInfo(completed[check], check);
        }

    }

    public bool PathTest(string[,] map, int x, int y)
    {
        //All failure conditions:
        //out of index
        if (x < 0 || y < 0)
            return false;
        if (x > map.GetLength(0) - 1 || y > map.GetLength(1) - 1)
            return false;
        //Wall in the way
        if (map[x, y] == "1")
            return false;
        return true;
    }

    protected void Update()
    {
        if (camera)
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(posx * 0.32f, -posy * 0.32f, -10f);
        transform.position = new Vector2(posx * 0.32f, -posy * 0.32f);
        ChangeInt("X", posx);
        ChangeInt("Y", posy);
        if (intvars.ContainsKey("HP"))
        {
            if (GetInt("HP") <= 0)
            {

                DoFunction("ONDESTROY");
                Destroy(this.gameObject);
                GameManager.Remove(this.gameObject);
                TilemapGenerator.walls[posx, posy] = false;
                if (turn == true)
                    EndTurn();
            }
        }
        if (intvars.ContainsKey("MAXHP"))
        {
            if (GetInt("HP") > GetInt("MAXHP"))
            {
                ChangeInt("HP", GetInt("MAXHP"));
            }
        }
    }

    //Equips an item
    public void EquipItem(string item)
    {
        FunctionParser funcparser = new FunctionParser();
        int tryslot = 0;
        string[] parameters = GameManager.LoadFile("Items", item);
        foreach(string str in parameters)
        {
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            if (key.ToUpper() == "SLOT")
            {
                int.TryParse(value, out tryslot);
                if(equippeditem[tryslot] == true)
                {
                    GameManager.Print("There is already something in that slot!");
                    return;
                }
                else
                {
                    equippeditem[tryslot] = true;
                }
            }
        }
        
        foreach (string j in parameters)
        {
            string str = j.Replace("$this", "$" + name);
            if (str.StartsWith("["))
            {
                break;
            }
            //split into key value pair
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            //check if int or string
            if(key.ToUpper() == "NAME")
            {
                continue;
            }
            
            if (int.TryParse(value, out int i))
            {
                if (GetInt(key) == 0)
                    ChangeInt(key, i);
                else
                    ChangeInt(key, GetInt(key) + i);
            }
            else
            {
                ChangeString(key, value);
            }
        }
        this.gameObject.name = GetString("NAME") + ID.ToString();
    }

    //Load parameters from the object file
    public void LoadParameters(string name){
        int functionstart = 0;
        FunctionParser funcparser = new FunctionParser();
        string[] parameters = GameManager.LoadFile("Objects", name);
        foreach(string str in parameters){
            if (str.StartsWith("["))
            {
                break;
            }
            //split into key value pair
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            //check if int or string
            if(int.TryParse(value, out int i)){
                if(key.ToUpper() == "HP")
                    ChangeInt("MAXHP", i);
                ChangeInt(key, i);
            }
            else{
                ChangeString(key, value);
            }
            ++functionstart;
        }
        for(int i = functionstart; i < parameters.Length; ++i)
        {
            if (parameters[i].StartsWith("["))
            {
                AddFunction(funcparser.Export());
                string n = parameters[i].Replace("[", "");
                n = n.Replace("]", "");
                
                funcparser = new FunctionParser(n);
                continue;
            }
            funcparser.AddLine(parameters[i]);
        }
        AddFunction(funcparser.Export());
        this.gameObject.name = GetString("NAME") + ID.ToString();
        equippeditem = new bool[GetInt("SLOTS")];
        for (int i = 0; i < equippeditem.Length; ++i)
        {
            equippeditem[i] = false;
        }
        DoFunction("ONSTART");
    }

    //Add the function
    public void AddFunction((string, string[]) func)
    {
        if (func.Item1 == null)
            return;
        string name = func.Item1;
        
        string[] function = func.Item2;
        if (name.StartsWith("ACTION:"))
        {

            name = name.Substring(7);
            actions.Add(name, function);
        }
        else
        {
            name = name.ToUpper();
            functions.Add(name, function);
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

    public void DoExtFunction(string[] function)
    {
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(function, this.gameObject);
        cmd.ExecuteStep(0);
    }

    public string[] GetFunction(string func)
    {
        if (!functions.ContainsKey(func))
            return null;
        return functions[func];
    }

    public void DoFunction(string func)
    {
        func = func.ToUpper();
        if (!functions.ContainsKey(func))
            return;
        string[] inputs = functions[func];
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(inputs, this.gameObject);
        cmd.ExecuteStep(0);
    }

    public string[] GetAction(string func)
    {
        if (!actions.ContainsKey(func))
            return null;
        return actions[func];
    }

    public void DoAction(string func)
    {
        if (!actions.ContainsKey(func))
            return;
        string[] inputs = actions[func];
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(inputs, this.gameObject);
        cmd.ExecuteStep(0);
    }

    public void DoMove(string[] move)
    {
        CommandRouter cmd = gameObject.AddComponent<CommandRouter>();
        cmd.Assign(move, this.gameObject);
        cmd.ExecuteStep(1);
    }

    public void Nextstep(int step, string[] inputs){
        if(step < inputs.Length){
            StartScript(step, inputs);
        }
    }

    //lock the player into place
    public void Lock(bool tf){
        this.locked = tf;
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
        if (intvars.ContainsKey(key))
            return intvars[key];
        else
            return 0;
    }

    public string GetString(string key){
        key = key.ToUpper();
        if (stringvars.ContainsKey(key))
            return stringvars[key];
        else
            return "None";
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
}
