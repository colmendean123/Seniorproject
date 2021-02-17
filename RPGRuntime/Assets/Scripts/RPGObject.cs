using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGExceptions;

public class RPGObject : MonoBehaviour
{
    SortedDictionary<string,int> intvars;
    SortedDictionary<string,string> stringvars;

    protected bool locked = false;

    protected void Start(){
        ChangeInt("HP", 10);
        ChangeInt("DEF", 3);
        ChangeInt("ATK", 3);
        ChangeString("name", gameObject.name);
    }
    public void Lock(bool tf){
        Debug.Log(tf);
        this.locked = tf;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<CommandRouter>().Nextstep();
    }

    public int GetInt(string key){
        if(intvars.ContainsKey(key))
            return intvars[key];
        else
            throw new ObjectVariableException("Error! Object does not contain int \""+key+"\"");
    }

    public string GetString(string key){
        if(intvars.ContainsKey(key))
            return stringvars[key];
        else
            throw new ObjectVariableException("Error! Object does not contain string \""+key+"\"");
    }

    public void ChangeString(string key, string var){
        if(!stringvars.ContainsKey(key))
            stringvars.Add(key, var);
        else
            stringvars[key] = var;
    }

    public void ChangeInt(string key, int var){
    if(!intvars.ContainsKey(key))
        intvars.Add(key, var);
    else
        intvars[key] = var;
    }
}
