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
    "OBJECTIVECOMPLETE", "SHOPWINDOW", "SHOPEND", "SHOP", "SELL", "LOCK", "UNLOCK", "="};
    private Tokenizer token;
    int logicdepth = 0;
    private int step;
    private string[] inputs;

    public void GetScript(string script){
        step = 0;
        inputs = GameManager.LoadFile("Scripts", script);
    }

    public void ExecuteStep(int i){
        Execute(inputs[i], gameObject, step, ref logicdepth);
    }

    public void Execute(string inputs, GameObject self, int step, ref int logicdepth){
        int depth = 0;
        while(true){
            if(inputs[depth] == ' ')
                ++depth;
            else
                break;
        }
        depth = depth/4;
        if(logicdepth > depth)
            logicdepth = depth;
        if(logicdepth != depth)
            Nextstep(self, step);
        token = new Tokenizer(inputs);
        

        string next = token.GetNext();
        if(CheckCommand(next)){
            //insert command definitions 
            if(next.Equals("LOCK"))
                GameObject.Find(token.GetNext()).GetComponent<RPGObject>().Lock(true, self, step);
            if(next.Equals("UNLOCK"))
                GameObject.Find(token.GetNext()).GetComponent<RPGObject>().Lock(false, self, step);
        }
        //move on to simple commands, using the first string as a target
        else{
            string target = next;
            string command = token.GetNext();
            if(CheckCommand(command)){
                //pass along the SAY command with the target for simple targeting.
                if(command.Equals("SAY")){

                    Say(token, target, self, step);
                }
                if(command.Equals("=")){
                    string var = token.GetNext();
                    string[] spl = target.Split('.');    
                    Debug.Log(spl[1]);
                    Set(target, var);
                    
                    Nextstep(self, step);
                }
                if(command.Equals("-=")){
                    string var = token.GetNext();
                    string[] spl = target.Split('.');    
                    Debug.Log(spl[1]);
                    Set(target, Get(target));
                    
                    Nextstep(self, step);
                }
            }
        }
    }

    private void Set(string target, string set){
        ParsingCommands.ChangeVar(target, set);
    }

    private string Get(string target){
        return ParsingCommands.GetVar(target);
    }

    //Run the dialogue commands by parsing and determining which one to do 
    private void Say(Tokenizer token, string target, GameObject self, int step){
        string dialogue = token.GetNext();
        if(token.Size()-1 > 3){
            
            string sp = token.GetNext();
            float speed = float.Parse(sp);
            DialogueCommands.Say(GameObject.Find(target), dialogue, speed, self, step);
            }
        else{
            DialogueCommands.Say(GameObject.Find(target), dialogue, self, step);
        }
    }

    private bool CheckCommand(string com){
        foreach(string i in commands){
            if(i.Equals(com))
                return true;
        }
        return false;
    }

    public void Nextstep(int step){
        if(step < inputs.Length){
            ExecuteStep(step);
        }
    }

    //go through potential script users and call nextstep
    public void Nextstep(GameObject target, int step){
        ++step;
        if(target.tag == "Manager")
        target.GetComponent<CommandRouter>().Nextstep(step);
        if(target.tag == "Object")
        target.GetComponent<RPGObject>().Nextstep(step);
    }

    public void Begin(){
        //GetScript("testdialogue.txt");
        //StartScript();
    }

    public void StartScript(){
        step = 0;
        ExecuteStep(step);
    }
}