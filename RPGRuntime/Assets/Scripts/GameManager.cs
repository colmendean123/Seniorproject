using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public static class GameManager
{
    private static string moduleName = "TestModule";
    private static string mapName = "default";
    private static string path;
    // Start is called before the first frame update
    public static void Init()
    {
        path = Application.dataPath + "\\..\\" + moduleName;
        
        string[] directories = Directory.GetDirectories(path);
        GameObject dirtest = GameObject.Find("Text");
        for(int i = 0; i < directories.Length; ++i){
            Debug.Log(directories[i]);
            dirtest.GetComponent<TextMeshProUGUI>().text += "\n"+directories[i];
        }
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        for(int i = 0; i < objects.Length; ++i){
            objects[i].GetComponent<SpriteScript>().LoadSprite();
        }
        DialogueCommands.Say(GameObject.Find("man"), "Testing");
    }

    public static string GetPath(){
        return path;
    }

    public static string GetMapName(){
        return mapName;
    }

    public static string GetMap(string mapName){
        return System.IO.File.ReadAllText(path + "\\Maps\\" + mapName);
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


}
