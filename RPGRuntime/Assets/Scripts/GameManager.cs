using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Scripting;

public static class GameManager
{
    private static string moduleName = "TestModule";
    private static string mapName = "default";
    private static string path;
    private static List<(int, int)> coordinateArray;
    private static List<GameObject> objects;
    private static int turn = 0;

    private static SortedDictionary<string, int> intvars = new SortedDictionary<string, int>();
    private static SortedDictionary<string, string> stringvars = new SortedDictionary<string, string>();
    // Start is called before the first frame update
    public static void Init()
    {
        turn = 0;
        path = Application.dataPath + "\\..\\" + moduleName;
        objects = new List<GameObject>();
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Object"))
        {
            objects.Add(i);
        }
        Debug(objects.Count.ToString());
        CalculateTurnOrder();
        NextTurn();
    }

    //calculates turn order by finding the highest speed and sorting them into a sorted object list. Done once after initialization and once any time speed is changed. (Implement this)
    public static void CalculateTurnOrder()
    {
        List<GameObject> dummy = objects;
        List<GameObject> sorted = new List<GameObject>();
        while (dummy.Count != 0) {
            
            GameObject maxspeed;
            maxspeed = dummy[0];
            foreach (GameObject i in dummy)
            {
                if (i.GetComponent<RPGObject>().GetInt("SPEED") > maxspeed.GetComponent<RPGObject>().GetInt("SPEED"))
                    maxspeed = i;
            }
            sorted.Add(maxspeed);
            dummy.Remove(maxspeed);
        }
        objects = sorted;
    }

    public static void Remove(GameObject target)
    {
        objects.Remove(target);
    }

    public static void NextTurn()
    {
        if (turn > objects.Count-1)
            turn = 0;
        objects[turn].GetComponent<RPGObject>().BeginTurn();
        ++turn;
    }

    public static void NewObjectList()
    {
        objects = new List<GameObject>();
    }

    public static void NewObject(GameObject obj)
    {
        objects.Add(obj);
        (int, int) pos = obj.GetComponent<RPGObject>().GetPosition();
        coordinateArray.Add(pos);
    }

    //Get the gameobject by ID
    public static GameObject GetObjectById(string name, int id)
    {
        return GameObject.Find(name + id.ToString());
    }

    //Get object from the position
    public static GameObject GetObjectByPosition(int x, int y)
    {
        foreach(GameObject i in objects)
        {
            if (i.GetComponent<RPGObject>().GetInt("X") == x && i.GetComponent<RPGObject>().GetInt("Y") == y)
                return i;
        }
        return null;
    }

    public static void ChangeCoordinates(int id, (int, int) pos)
    {
        coordinateArray[id] = pos;
    }

    //get the system module path
    public static string GetPath(){
        if(path == null)
            path = Application.dataPath + "\\..\\" + moduleName;
        return path;
    }

    //get the name of the current map
    public static string GetMapName(){
        return mapName;
    }

    //loads an external text file as a batch of lines
    public static string[] LoadFile(string folder, string filename){
        return System.IO.File.ReadAllLines(GetPath() + "\\" + folder + "\\" + filename);
    }

    //get a file from the module's map folder
    public static string GetMap(string mapName){
        return System.IO.File.ReadAllText(GetPath() + "\\Maps\\" + mapName);
    }

    //Loading for sprites
    public static Texture2D LoadTexture(string FilePath) {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
    
        Texture2D Tex2D;
        byte[] FileData;
    
        if (File.Exists(FilePath)){
        FileData = File.ReadAllBytes(FilePath);
        Tex2D = new Texture2D(2, 2);
        if (Tex2D.LoadImage(FileData))
            return Tex2D;
        }  
        return null;          
     }

    //changing camera target
    public static void CameraTarget(GameObject target){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().target = target;
    }

    //Print to the console from the gamemanager
    public static void Print(string ln){
        GameObject.FindGameObjectWithTag("Console").GetComponent<HUDScript>().Print(ln);
    }

    public static void Clear()
    {
        for(int i = 0; i < 5; ++i)
            GameObject.FindGameObjectWithTag("Console").GetComponent<HUDScript>().Print("");
    }

//Variable parsing and checking
    public static string ParseVar(string command){
        string[] spl = command.Split('.');
        if (spl.Length == 1)
        {
            if (IsInt(spl[0]))
                return GetInt(spl[0]).ToString();
            if (IsString(spl[0]))
                return GetString(spl[0]).ToString();
            GameManager.Print("Error! Variable not found!");
            return null;
        }
        spl[0] = spl[0].Substring(1);
        spl[1] = spl[1].ToUpper();
        if(IsString(spl[0], spl[1])){
            return GameObject.Find(spl[0]).GetComponent<RPGObject>().GetString(spl[1]);
        }else if(IsInt(spl[0], spl[1])){
            
            return GameObject.Find(spl[0]).GetComponent<RPGObject>().GetInt(spl[1]).ToString();
        }
        else{
            GameManager.Print("Error! Variable not found!");
            return null;
        }
    }


        //check if the target/key combo exists
    public static bool IsString(string target, string key){
        if(GameObject.Find(target)!= null){
            return GameObject.Find(target).GetComponent<RPGObject>().IsString(key);
        }
        return false;
    }

        public static bool IsInt(string target, string key){
        if(GameObject.Find(target)!= null){
            return GameObject.Find(target).GetComponent<RPGObject>().IsInt(key);
        }
        return false;
    }

    //dictionary system copied wholesale for universal variables from the rpgobjs to the gamemanager.

    public static bool IsString(string key)
    {
        key = key.ToUpper();
        if (stringvars.ContainsKey(key))
        {
            return true;
        }
        return false;
    }

    public static bool IsInt(string key)
    {
        key = key.ToUpper();
        if (intvars.ContainsKey(key))
        {
            return true;
        }
        return false;
    }

    public static void ChangeString(string key, string var)
    {
        key = key.ToUpper();
        if (!stringvars.ContainsKey(key))
            stringvars.Add(key, var);
        else
            stringvars[key] = var;
    }

    public static void ChangeInt(string key, int var)
    {
        key = key.ToUpper();
        if (!intvars.ContainsKey(key))
            intvars.Add(key, var);
        else
            intvars[key] = var;
    }

    //Get the keys from the dict by key
    public static int GetInt(string key)
    {
        key = key.ToUpper();
        if (intvars.ContainsKey(key))
            return intvars[key];
        else
            throw new System.Exception("Error! GameManager does not contain string \"" + key + "\"");
    }

    public static string GetString(string key)
    {
        key = key.ToUpper();
        if (stringvars.ContainsKey(key))
            return stringvars[key];
        else
            throw new System.Exception("Error! GameManager does not contain string \"" + key + "\"");
    }

    public static void Debug(string deb)
    {
        UnityEngine.Debug.Log(deb);
    }
}
