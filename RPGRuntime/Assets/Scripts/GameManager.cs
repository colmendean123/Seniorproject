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
        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        for(int i = 0; i < objects.Length; ++i){
            objects[i].GetComponent<SpriteScript>().LoadSprite();
        }
        CameraTarget(GameObject.Find("man"));
        Print("The player is standing!");
        Print("This is the second line!");
        Print("This is the third line!");
        Print("4!");
        Print("5!");
    }

    public static string GetPath(){
        if(path == null)
            path = Application.dataPath + "\\..\\" + moduleName;
        return path;
    }

    public static string GetMapName(){
        return mapName;
    }

    public static string[] getFile(string folder, string filename){
        return System.IO.File.ReadAllLines(GetPath() + "\\" + folder + "\\" + filename);
    }

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

    public static void Print(string ln){
        GameObject.FindGameObjectWithTag("Console").GetComponent<HUDScript>().Print(ln);
    }
}
