using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting{
    public static class DialogueCommands
    {
        public static void Say(GameObject target, string input, GameObject self, int step){
            Say(target, input, 0f, self, step);
        }


        public static void Say(GameObject target, string input, float speed, GameObject self, int step){
            var say = Resources.Load("Prefabs/Say") as GameObject;
            say.GetComponent<DialogueScript>().text = input;
            say.GetComponent<DialogueScript>().speaker = target;
            say.GetComponent<DialogueScript>().speed = speed;
            say.GetComponent<DialogueScript>().target = self;
            say.GetComponent<DialogueScript>().step = step;
            GameObject final = GameObject.Instantiate(say);
            final.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }
}
