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



    //onstart, load default variables. Then load parameters.
    protected void Start(){
        //start the dictionary for int and strings
        intvars = new SortedDictionary<string, int>();
        stringvars = new SortedDictionary<string, string>();
        ChangeInt("HP", 10);
        ChangeInt("DEF", 3);
        ChangeInt("ATK", 3);
        ChangeString("image", "player.png");
        ChangeString("name", gameObject.name);
        ChangeString("sprite", "player.png");
        LoadSprite(GetString("image"));
        LoadParameters("player.txt");
        DoScript("testdialogue.txt");

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
        }

    }
    //scripting on a local level
    public void DoScript(string script){
        inputs = GameManager.LoadFile("Scripts", script);
        StartScript(0);
    }

    public void StartScript(int step){
        string input = inputs[step];
        input = input.Replace("$this", "$"+this.gameObject.name);
        Debug.Log(input);
        GameObject.FindGameObjectWithTag("Manager").GetComponent<CommandRouter>().Execute(input, gameObject, step, ref logicdepth);
    }

    public void Nextstep(int step){
        if(step < inputs.Length){
            StartScript(step);
        }
    }

    //lock the player into place
    public void Lock(bool tf, GameObject target, int step){
        this.locked = tf;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<CommandRouter>().Nextstep(target, step);
    }

    //Load the sprite from an external sprite path
    public void LoadSprite(string spr)
    {
        Texture2D SpriteTexture = GameManager.LoadTexture(GameManager.GetPath()+"\\Sprites\\"+spr);
        SpriteTexture.filterMode = FilterMode.Point;
        UnityEngine.Sprite newsprite = UnityEngine.Sprite.Create(SpriteTexture, new Rect(0,0, SpriteTexture.width, SpriteTexture.height),  new Vector2(0.5f, 0.5f), 100f, 0 , SpriteMeshType.Tight);
        this.GetComponent<SpriteRenderer>().sprite = newsprite;
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
}
