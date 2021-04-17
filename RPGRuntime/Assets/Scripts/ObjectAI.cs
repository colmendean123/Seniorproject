using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAI : RPGObject
{
    public new void Update()
    {
        base.Update();
        if (turn == true)
        {
            EndTurn();
        }
    }
}
