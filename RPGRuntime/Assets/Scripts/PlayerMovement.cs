using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RPGObject
{

    bool movementreset = true;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }


    public void Move(int x, int y){
        if(TilemapGenerator.CheckCollision(x, y) == false){
            posx = x;
            posy = y;
            
        }
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(!locked)
            Control();
        
    }

    private void Control(){
        //reset input
        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            movementreset = true;
        //down
        if(Input.GetAxis("Vertical") < 0 && movementreset == true){
            Move(posx, posy+1);
            movementreset = false;
           
        }
        //up
        if(Input.GetAxis("Vertical") > 0 && movementreset == true){
            Move(posx, posy-1);
            movementreset = false;
        }
        //right
        if(Input.GetAxis("Horizontal") > 0 && movementreset == true){
            Move(posx+1, posy);
            movementreset = false;
        }
        //left
        if(Input.GetAxis("Horizontal") < 0 && movementreset == true){
            Move(posx-1, posy);
            movementreset = false;
        }
    }
}
