using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAI : RPGObject
{
    float timer = 0.2f;
    public new void Start()
    {
        base.Start();
    }

    public new void Update()
    {
        base.Update();
        if (turn == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                if (target != null)
                {
                    PathingInfo targetpath = GeneratePath(target.gameObject.name);
                    if (targetpath.distance > 0)
                    {
                        (int, int) next = targetpath.position;
                        Move(next.Item1, next.Item2);
                    }
                    else
                    {
                        int maxattack = attacks.Count - 1;
                        System.Random rand = new System.Random();
                        int attack = rand.Next(0, maxattack);
                        Attack(attack, target.gameObject);
                    }
                    EndTurn();
                    timer = 0.2f;

                }
                else
                {
                    EndTurn();
                    timer = 0.2f;
                }
            }
        }
    }
}
