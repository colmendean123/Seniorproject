using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;

public class PlayerMovement : RPGObject
{

    bool movementreset = true;
    bool talking = false;
    bool attacking = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }



    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(!locked && turn)
            Control();
        
    }

    private void Control(){
        //reset input
        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0 )
            movementreset = true;
        if (talking == true || attacking == true)
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
        if (Input.GetButtonDown("Action") && talking == false)
        {
            talking = true;
            DialogueCommands.NewResponse();
            List<GameObject> Adjacent = ScanForAdjacency();
            if (Adjacent.Count == 0)
            {
                talking = false;
                return;
            }
            
            foreach(GameObject i in Adjacent)
            {
                DialogueCommands.AddResponse(i.GetComponent<RPGObject>().GetString("NAME"));
            }
        }
        if (attacking == true)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int choice = DialogueCommands.GetResponse();
                attacking = false;
                List<string> actions = new List<string>();
                foreach (string i in target.GetComponent<RPGObject>().actions.Keys)
                {
                    actions.Add(i);
                }
                DialogueCommands.Clear();
                if (choice < attacknames.Count)
                {
                    Attack(choice, target);
                }
                else
                {
                    target.GetComponent<RPGObject>().DoAction(actions[choice - attacknames.Count]);
                }
            }
        }
        if (talking == true)
        {
            DialogueCommands.ResponseMenu();
            DialogueCommands.Control();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                List<GameObject> Adjacent = ScanForAdjacency();
                int choice = DialogueCommands.GetResponse();
                talking = false;
                attacking = true;
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

    }
}
