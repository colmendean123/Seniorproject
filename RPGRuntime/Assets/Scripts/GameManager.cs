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

    // Start is called before the first frame update
    public static void Init()
    {
        path = Application.dataPath + "\\..\\" + moduleName;
        GameObject.Find("MapManager").GetComponent<CommandRouter>().Begin();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        CameraTarget(GameObject.Find("man"));
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

//Variable parsing and checking
    public static string ParseVar(string command){
        string[] targandvar = command.Split('.');
        targandvar[0] = targandvar[0].Substring(1);
        targandvar[1] = targandvar[1].ToUpper();
        Debug.Log(targandvar[0] + ", " + targandvar[1]);
        if(IsString(targandvar[0], targandvar[1])){
            Debug.Log(GameObject.Find(targandvar[0]).GetComponent<RPGObject>().GetString(targandvar[1]));
            return GameObject.Find(targandvar[0]).GetComponent<RPGObject>().GetString(targandvar[1]);
        }else if(IsInt(targandvar[0], targandvar[1])){
            
            return GameObject.Find(targandvar[0]).GetComponent<RPGObject>().GetInt(targandvar[1]).ToString();
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
}
