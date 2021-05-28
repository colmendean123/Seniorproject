using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;
using System;
using Random = System.Random;

public class CommandRouter : MonoBehaviour
{
    private string[] commands = {"SELECT", "GET", "SET", "RAND",
    "GOTO", "IF", "ELSE", "TRUE", "FALSE", "PRINT", "HEAL",
    "ACTION", "ATTACK", "AGGRO", "FOLLOW", "MOVE", "DESTROY", "GIVE",
    "SAY", "RESPONSE", "PRINT", "QUEST", "COMPLETEQUEST",
    "OBJECTIVECOMPLETE", "SHOPWINDOW", "SHOPEND", "SHOP", "SELL", "LOCK", "UNLOCK", "=", "-=", "+=", "IF", "GREATER",
    "THAN", "LESS", "IS", "GETRESPONSE", "NEWRESPONSE", "ADDRESPONSE", "RUN", "RAND", "SWITCHMAP", "AGGRO", "DEAGRRO"};
    private Tokenizer token;
    int logicdepth = 0;
    private string[] func;
    private bool waiting;
    private GameObject self;
    //target var temporarys for dialogue selection
    private int tempstep;
    private string temptarget;
    private float timer;
    private int step = 0;

    public void Assign(string[] function, GameObject s)
    {
        func = function;
        self = s;
    }

    public void ExecuteStep(int i){
        step = i;
        Execute(self, ref logicdepth);
    }

    public void Update()
    {
        if(waiting)
            Response();
    }

    public void FixedUpdate()
    {
        if (timer > 0)
            StepTimer();
    }

    public void StepTimer()
    {
        if (timer > 0)
            timer -= 1f;
        if (timer <= 0)
        {
            timer = 0f;
            Nextstep();
        }
    }

    public void WaitSeconds(float s)
    {
        timer = s * 60f;
    }

    //Executes a function from an RPG object
    public void Execute(GameObject self,  ref int logicdepth)
    {
        string inputs = func[step];
        int depth = 0;
        while (true)
        {
            if (inputs[depth] == '	')
                ++depth;
            else
                break;
        }

        if (logicdepth > depth)
            logicdepth = depth;
        if (logicdepth != depth)
        {
            Nextstep();
            return;
        }

        if (inputs.StartsWith("//"))
            Nextstep();
        token = new Tokenizer(inputs, self.name, this);
        ExecuteStep(token);
    }

    //Executes a function without the Gameobject self
    public void Execute(string inputs, ref int logicdepth)
    {
        int depth = 0;
        while (true)
        {
            if (inputs[depth] == '	')
                ++depth;
            else
                break;
        }

        if (logicdepth > depth)
            logicdepth = depth;
        if (logicdepth != depth)
        {
            Nextstep();
            return;
        }

        if (inputs.StartsWith("//"))
            Nextstep();
        
        token = new Tokenizer(inputs, self.name, this);
        ExecuteStep(token);
    }

    //Execute steps based on the tokenizer input
    public void ExecuteStep(Tokenizer token) {
        
        string next = token.GetNext();
        if(CheckCommand(next)){
            if (next.Equals("GOTO"))
            {
                int.TryParse(token.GetNext(), out step);
            }
            if (next.Equals("SAY"))
            {
                Say(token, this.gameObject.name, self);
            }
            if (next.Equals("AGGRO"))
            {
                this.gameObject.GetComponent<RPGObject>().target = RPGObject.FindWithName(token.GetNext());
            }
            if (next.Equals("DEAGGRO"))
            {
                this.gameObject.GetComponent<RPGObject>().target = null;
            }
            if (next.Equals("SWITCHMAP"))
            {
                string map = token.GetNext();
                GameManager.LoadMap(map);
            }
            if (next.Equals("RUN"))
            {
                string script = token.GetNext();
                string target = token.GetNext();
                GameObject obj = this.gameObject;
                if(target != null)
                {
                    obj = RPGObject.FindWithName(target);
                }
                CommandRouter cmd = obj.AddComponent<CommandRouter>();
                string[] inputs = GameManager.LoadFile("Scripts", script);
                cmd.Assign(inputs, this.gameObject);
                cmd.ExecuteStep(0);

            }
            if (next.Equals("GETRESPONSE"))
            {
                string target = token.GetNext();
                
                tempstep = step;
                temptarget = target;
                waiting = true;
            }
            if (next.Equals("PRINT"))
            {
                string p = token.GetNext();
                GameManager.Print(p);
                Nextstep();
            }
            if (next.Equals("NEWRESPONSE"))
            {
                DialogueCommands.NewResponse();
                Nextstep();
            }
            if (next.Equals("ADDRESPONSE"))
            {
                string response = token.GetNext();
                DialogueCommands.AddResponse(response);
                Nextstep();
            }
            //insert command definitions 
            if (next.Equals("LOCK"))
            {
                next = token.GetNext();
                GameObject.Find(next).GetComponent<RPGObject>().Lock(true);
                Nextstep();
            }
            if (next.Equals("UNLOCK"))
            {
                GameObject.Find(token.GetNext()).GetComponent<RPGObject>().Lock(false);
                Nextstep();
            }
            if (next.Equals("RAND"))
            {
                int number = 0;
                int.TryParse(token.GetNext(), out number);
                Random r = new Random(DateTime.Now.Millisecond);
                int rint = r.Next(0, 100);
                Debug.Log("Rint - " + rint.ToString() + " number - " + number.ToString());
                if(number > rint)
                {
                    ++logicdepth;
                    Nextstep();
                    return;
                }
            }
            if(next.Equals("IF")){
                
                //find the comparator through concat
                string comparator = "";
                string var1 = token.GetNext();
                
                //skip over already determined operators
                while (var1.Equals("TRUE") || var1.Equals("FALSE") || var1.Equals("AND") || var1.Equals("OR"))
                {
                    var1 = token.GetNext();
                }
                
                //Final step. finding the :
                if (var1 == ":")
                {
                    List<bool> andops = new List<bool>();
                    List<bool> orops = new List<bool>();
                    for(int i = 1; i < token.Size(); i+=2)
                    {
                        if(!(i > token.Size())){
                            bool truth = (token.Get(i) == "TRUE");
                            //check for or statements on either side. Logic is a bit janky but will work in most scenarios.
                            if(token.Get(i-1) == "OR" || token.Get(i+1) == "OR")
                            {
                                orops.Add(truth);
                            }
                            else
                            {
                                andops.Add(truth);
                            }
                        }
                    }
                    //process ands. if all are true, it is true.
                    bool andstatementtruth = true;
                    //process ors. if one is true, it is true.
                    bool orstatementtruth = false;
                    foreach (bool j in orops)
                    {
                        if (j == true)
                            orstatementtruth = true;
                    }
                    foreach (bool j in andops)
                    {
                        if (j == false)
                            andstatementtruth = false;
                    }
                    if (andops.Count == 0)
                        andstatementtruth = true;
                    if (orops.Count == 0)
                        orstatementtruth = true;
                    if(andstatementtruth && orstatementtruth)
                    {
                        ++logicdepth;
                    }
                    Debug.Log("GO");
                    Nextstep();
                    return;
                }

                //Get the step of the current token. used for making the new command at the ned
                int stepstart = token.GetStep();
                string add = token.GetNext();
                while(CheckCommand(add)){
                    comparator+=add;
                    add = token.GetNext();
                }
                string var2 = add;
                //get last step.
                int stepend = token.GetStep();
                string newcommand = "";
                string torf;
                Debug.Log("var3 " + var1);
                if (Equals(var1, var2, comparator))
                    torf = "TRUE";
                else
                    torf = "FALSE";
                for(int i = 0; i < token.Size(); ++i)
                {
                    if (i == stepstart)
                        newcommand += " " + torf;
                    if(!(i >= stepstart-1 && stepend-1 >= i))
                    {
                        newcommand += " " + token.Get(i);
                    }
                }
                Execute(newcommand, ref logicdepth);
                return;
            }
        }
        //move on to simple commands, using the first string as a target
        else{
            string target = next;
            string command = token.GetNext().ToUpper();
            if(CheckCommand(command)){
                if (next.Equals("AGGRO"))
                {
                    RPGObject.FindWithName(target).GetComponent<RPGObject>().target = RPGObject.FindWithName(token.GetNext());
                }
                if (next.Equals("DEAGGRO"))
                {
                    RPGObject.FindWithName(target).GetComponent<RPGObject>().target = null;
                }
                //pass along the SAY command with the target for simple targeting.
                if (command.Equals("SAY")){

                    Say(token, target, self);
                }
                if (command.Equals("MOVE"))
                {
                    int x, y;
                    int.TryParse(token.GetNext(), out x);
                    int.TryParse(token.GetNext(), out y);
                    GameObject i = RPGObject.FindWithName(target);
                    i.GetComponent<RPGObject>().SetPosition(x, y);
                }
                if(command.Equals("=")){
                    string var = token.GetNext();
                    string[] spl = target.Split('.');
                    Set(target, var);
                    
                    Nextstep();
                }
                if(command.Equals("-=")){
                    string var = token.GetNext();
                    string[] spl = target.Split('.');
                    Set(target, (int.Parse(Get(target)) - int.Parse(var)).ToString());
                    Nextstep();
                }
                if (command.Equals("+="))
                {
                    string var = token.GetNext();
                    string[] spl = target.Split('.');
                    Set(target, (int.Parse(Get(target)) + int.Parse(var)).ToString());

                    Nextstep();
                }
            }
        }
    }
    

    //returns bool for the applicable operator
    private bool Equals(string x, string y,string op)
    {
        if(int.TryParse(x, out int xint) && int.TryParse(y, out int yint))
        {
            switch (op)
            {
                case "==":
                case "IS":
                    return xint == yint;
                case "ISGREATERTHAN":
                case ">":
                    return xint > yint;
                case "<":
                case "ISLESSTHAN":
                    return xint < yint;
                case ">=":
                case "ISGREATERTHANOREQUALTO":
                    return xint >= yint;
                case "<=":
                case "ISLESSTHANOREQUALTO":
                    return xint <= yint;
            }
        }
        switch (op)
        {
            case "==":
            case "IS":
                return x == y;
            case "ISGREATERTHAN":
            case ">":
                return x.CompareTo(y) > 0;
            case "<":
            case "ISLESSTHAN":
                return x.CompareTo(y) < 0;
            case ">=":
            case "ISGREATERTHANOREQUALTO":
                return x.CompareTo(y) >= 0;
            case "<=":
            case "ISLESSTHANOREQUALTO":
                return x.CompareTo(y) <= 0;
        }
        return false;
    }

    //Set either a string or an int based on $object.var or $var
    private void Set(string target, string set){
        ParsingCommands.ChangeVar(target, set);
    }

    //Get a variable from the format $object.var or $var
    private string Get(string target){
        return ParsingCommands.GetVar(target);
    }



    //Run the dialogue commands by parsing and determining which one to do 
    private void Say(Tokenizer token, string target, GameObject self){
        string dialogue = token.GetNext();
        if(token.Size()-1 > 3){
            
            string sp = token.GetNext();
            float speed = float.Parse(sp);
            WaitSeconds(speed);
            DialogueCommands.Say(RPGObject.FindWithName(target), dialogue, speed);
            }
        else{
            DialogueCommands.Say(RPGObject.FindWithName(target), dialogue);
            WaitSeconds(3f);
        }
    }

    //goes through the command list and checks against it
    public bool CheckCommand(string com){
        com = com.Trim();
        foreach(string i in commands){
            if(i.Equals(com))
                return true;
        }
        return false;
    }

    //call next step
    public void Nextstep(){
        ++step;
        if (step < func.Length){
            this.ExecuteStep(step);
        }
        else
        {
            Destroy(this);
        }
    }


    public void Begin(){
        //GetScript("testdialogue.txt");
        //StartScript();
    }

    public void StartScript(){
        ExecuteStep(0);
    }

    //Controls for the dialogue menu.
    private void Response()
    {
        DialogueCommands.ResponseMenu();
        DialogueCommands.Control();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int choice = DialogueCommands.GetResponse();
            waiting = false;
            Debug.Log(temptarget);
            Set("$"+temptarget, choice.ToString());
            DialogueCommands.Clear();
            Nextstep();
        }
    }
}