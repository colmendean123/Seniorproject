using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting{
    public static class DialogueCommands
    {
        //setting up response menu shenanigans
        public static List<string> responses;
        private static int camera;
        private static int selected;
        private static bool movementreset;
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

        public static void NewResponse()
        {
            responses = new List<string>();
            camera = 0;
            selected = 0;

        }

        public static void AddResponse(string response)
        {
            responses.Add(response);
        }

        public static void MoveSelection(int i)
        {
            if(selected + i > -1 && selected + i < responses.Count)
            selected += i;
        }

        public static void Clear()
        {
            for (int i = 0; i < 5; ++i)
            {
                GameManager.Print("");
            }
        }

        public static void ResponseMenu()
        {
            //print to the menu the options along with the selected option
            for(int i = camera + 5; i > camera-1; --i)
            {
                if (i > responses.Count-1) {
                    GameManager.Print("");
                    continue;
                }
                if (i == selected)
                    GameManager.Print("> " + responses[i]);
                else
                    GameManager.Print(responses[i]);
            }
        }

        public static int GetResponse()
        {
            return selected;
        }

        public static void Control()
        {
            //reset input
            if (Input.GetAxis("Vertical") == 0)
                movementreset = true;
            //down
            if (Input.GetAxis("Vertical") < 0 && movementreset == true)
            {
                MoveSelection(1);
                movementreset = false;

            }
            //up
            if (Input.GetAxis("Vertical") > 0 && movementreset == true)
            {
                MoveSelection(-1);
                movementreset = false;
            }

        }
    }
}
