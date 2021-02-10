using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{

    int posx = 1;
    int posy = 0;
    bool movementreset = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Move(int x, int y){
        if(TilemapGenerator.CheckCollision(x, y) == false){
            posx = x;
            posy = y;
            Debug.Log("Go");
        }
    }
    // Update is called once per frame
    void Update()
    {
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
        transform.position = new Vector2(posx*0.32f, -posy*0.32f);
    }
}
