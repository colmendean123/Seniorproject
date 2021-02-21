using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;

public class CommandRouter : MonoBehaviour
{
    private string[] commands = {"SELECT", "GET", "SET", "RAND",
    "GOTO", "IF", "ELSE", "TRUE", "FALSE", "PRINT", "HEAL",
    "ACTION", "ATTACK", "AGGRO", "FOLLOW", "MOVE", "DESTROY", "GIVE",
    "DEAGGRO", "SAY", "RESPONSE", "PRINT", "QUEST", "COMPLETEQUEST",
    "OBJECTIVECOMPLETE", "SHOPWINDOW", "SHOPEND", "SHOP", "SELL", "LOCK", "UNLOCK"};
    private Tokenizer token;
    private int step;
    private string[] inputs;

    public void GetScript(string script){
        step = 0;
        inputs = GameManager.LoadFile("Scripts", script);
    }

    public void ExecuteStep(int i){
        token = new Tokenizer(inputs[i]);
        string next = token.GetNext();
        if(CheckCommand(next)){
            //insert command definitions 
            if(next.Equals("LOCK"))
                GameObject.Find(token.GetNext()).GetComponent<RPGObject>().Lock(true);
            if(next.Equals("UNLOCK"))
                GameObject.Find(token.GetNext()).GetComponent<RPGObject>().Lock(false);
        }
        if(next.Contains(".")){
            //add the variable to the tokenizer argument list. Can be parsed later.
            token.AddArg(ParseVar(next)); 
        }
        //move on to simple commands, using the first string as a target
        else{
            string target = next;
            string command = token.GetNext();
            if(CheckCommand(command)){
                //pass along the SAY command with the target for simple targeting.
                if(command.Equals("SAY")){
                    Say(token, target);
                }
            }
        }
    }


    private string ParseVar(string command){
            string[] targandvar = command.Split('.');
            if(IsString(targandvar[0], targandvar[1])){
                return GameObject.Find(targandvar[0]).GetComponent<RPGObject>().GetString(targandvar[1]);
            }else if(IsInt(targandvar[0], targandvar[1])){
                return GameObject.Find(targandvar[0]).GetComponent<RPGObject>().GetInt(targandvar[1]).ToString();
            }
            else{
                GameManager.Print("Error! Variable not found!");
                return null;
            }
    }

    public bool IsString(string target, string key){
        if(GameObject.Find(target)!= null){
            return GameObject.Find(target).GetComponent<RPGObject>().IsString(key);
        }
        return false;
    }

        public bool IsInt(string target, string key){
        if(GameObject.Find(target)!= null){
            return GameObject.Find(target).GetComponent<RPGObject>().IsInt(key);
        }
        return false;
    }

    private void Say(Tokenizer token, string target){
        string dialogue = token.GetNext();
        if(token.Size()-1 > 3){
            
            string sp = token.GetNext();
            float speed = float.Parse(sp);
            DialogueCommands.Say(GameObject.Find(target), dialogue, speed);
            }
        else{
            DialogueCommands.Say(GameObject.Find(target), dialogue);
        }
    }

    private bool CheckCommand(string com){
        foreach(string i in commands){
            if(i.Equals(com))
                return true;
        }
        return false;
    }

    public void Nextstep(){
        ++step;
        if(step < inputs.Length){
            ExecuteStep(step);
        }
    }

    public void Begin(){
        GetScript("testdialogue.txt");
        StartScript();
    }

    public void StartScript(){
        step = 0;
        ExecuteStep(step);
    }
}