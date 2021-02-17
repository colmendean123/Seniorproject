using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting{
    public static class DialogueCommands
    {
        public static void Say(GameObject target, string input){
            Say(target, input, 0f);
        }

        public static void Say(GameObject target, string input, float speed){
            var say = Resources.Load("Prefabs/Say") as GameObject;
            say.GetComponent<DialogueScript>().text = input;
            say.GetComponent<DialogueScript>().speaker = target;
            say.GetComponent<DialogueScript>().speed = speed;
            GameObject final = GameObject.Instantiate(say);
            final.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }
}
