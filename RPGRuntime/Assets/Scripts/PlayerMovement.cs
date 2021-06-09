using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;

public class PlayerMovement : RPGObject
{

    bool movementreset = true;
    int selectionprocess = 0;
    Item chosen;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if(!functions.ContainsKey("ACTIONS:Inventory"))
            AddFunction(("ACTION:Inventory", new string[0]));
    }



    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (!locked && turn)
            Control();
        
    }

    private void Control(){
        //reset input
        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0 )
            movementreset = true;
        if (selectionprocess != 0)
            movementreset = false;
        //down
        if (Input.GetAxis("Vertical") < 0 && movementreset == true){
            Move(posx, posy+1);
            movementreset = false;
            EndTurn();
        }
        //up
        if(Input.GetAxis("Vertical") > 0 && movementreset == true){
            Move(posx, posy-1);
            movementreset = false;
            EndTurn();
        }
        //right
        if(Input.GetAxis("Horizontal") > 0 && movementreset == true){
            Move(posx+1, posy);
            movementreset = false;
            EndTurn();
        }
        //left
        if(Input.GetAxis("Horizontal") < 0 && movementreset == true){
            Move(posx-1, posy);
            movementreset = false;
            EndTurn();
        }
        if (Input.GetButtonDown("Action") && selectionprocess == 0)
        {

            selectionprocess = 1;
            DialogueCommands.NewResponse();
            List<GameObject> Adjacent = ScanForAdjacency();
            Adjacent.Add(this.gameObject);
            if (Adjacent.Count == 0)
            {
                selectionprocess = 0;
                return;
            }
            
            foreach(GameObject i in Adjacent)
            {
                DialogueCommands.AddResponse(i.GetComponent<RPGObject>().GetString("NAME"));
            }

        }
        if (selectionprocess == 4)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int choice = DialogueCommands.GetResponse();
                if (chosen.slot != 0)
                {
                    if (choice == 0)
                    {
                        if (!chosen.equipped)
                            chosen.Equip();
                        else
                            chosen.UnEquip();
                        selectionprocess = 0;
                        DialogueCommands.Clear();
                        return;
                    }
                    choice -= 1;
                }
                this.gameObject.GetComponent<RPGObject>().DoExtFunction(chosen.GetActionResults(choice));
                selectionprocess = 0;
                DialogueCommands.Clear();
                return;
            }


        }
        if (selectionprocess == 3)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int choice = DialogueCommands.GetResponse();
                chosen = inventory[choice];
                DialogueCommands.NewResponse();
                List<string> actions = new List<string>();
                if (chosen.slot != 0)
                {
                    if(chosen.equipped == false)
                        DialogueCommands.AddResponse("Equip");
                    else
                        DialogueCommands.AddResponse("UnEquip");
                }
                foreach (string i in chosen.GetActions())
                {
                    DialogueCommands.AddResponse(i);
                }
                ++selectionprocess;
            }
        }
        if (selectionprocess == 2)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int choice = DialogueCommands.GetResponse();
                selectionprocess = 0;
                List<string> actions = new List<string>();
                foreach (string i in target.GetComponent<RPGObject>().actions.Keys)
                {
                    actions.Add(i);
                }
                int respcount = DialogueCommands.ResponseCount();
                DialogueCommands.Clear();
                if (choice < attacknames.Count)
                {
                    Attack(choice, target);
                    EndTurn();
                }
                else
                {
                    if(choice == respcount-1 && target == this.gameObject)
                    {
                        if (inventory.Count == 0)
                        {
                            DialogueCommands.Say(this.gameObject, GetString("NOITEM"));
                            return;
                        }
                        selectionprocess = 3;
                        DialogueCommands.NewResponse();
                        foreach (Item i in inventory)
                        {
                            DialogueCommands.AddResponse(i.GetName());
                        }
                        return;
                    }
                    target.GetComponent<RPGObject>().DoAction(actions[choice - attacknames.Count]);
                    
                }
            }
        }
        if (selectionprocess == 1)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                List<GameObject> Adjacent = ScanForAdjacency();
                Adjacent.Add(this.gameObject);
                int choice = DialogueCommands.GetResponse();
                selectionprocess = 2;
                DialogueCommands.NewResponse();
                foreach (string i in attacknames)
                {
                    DialogueCommands.AddResponse(i);
                }
                foreach (string i in Adjacent[choice].GetComponent<RPGObject>().actions.Keys)
                {
                    
                    DialogueCommands.AddResponse(i);
                }
                target = Adjacent[choice];
                DialogueCommands.Clear();
                
            }
        }
        if(selectionprocess > 0 && Input.GetKeyDown(KeyCode.Backspace))
        {
            DialogueCommands.Clear();
            selectionprocess = 0;
        }
    }

}
