using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAI : RPGObject
{

    public new void Start()
    {
        base.Start();
        target = RPGObject.FindWithName("ness");
    }

    public new void Update()
    {
        base.Update();
        if (turn == true)
        {
            if (target != null)
            {
                Debug.Log("GO!");

                (int, int) path = GeneratePath(target.gameObject.name);
                Move(path.Item1, path.Item2);
                EndTurn();
                
            }
        }
    }
}
