using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting{
    public static class ParsingCommands {
    
            //Take the target, take off the $, find that object and evaluate the value of set. Determine int or string from parsing.
            public static void ChangeVar(string target, string set){
                target = target.Substring(1);
                bool f = false;
                if (set.StartsWith("-"))
                {
                    f = true;
                    set = set.Substring(1);
                }
                set = VariableMath.Eval(set).ToString();
                string[] spl = target.Split('.');
                int parsed;
                if (spl.Length == 1)
                {
                    if (int.TryParse(set, out parsed))
                    {
                        if (f == true)
                            parsed = -parsed;
                        GameManager.ChangeInt(spl[0], parsed);
                    }
                    else
                        GameManager.ChangeString(spl[0], set);
                    return;
                }
                string obj = spl[0];
                string var = spl[1];
            RPGObject rpgobj = GameObject.Find(obj).GetComponent<RPGObject>();
           
                if (int.TryParse(set, out parsed))
                {
                    if (f == true)
                        parsed = -parsed;
                    rpgobj.ChangeInt(var, parsed);
                }
                else
                    rpgobj.ChangeString(var, set);
            }
            //get the variable by finding the target, taking off the $, and parsing.
            public static string GetVar(string target){
                target = target.Substring(1);
                string[] spl = target.Split('.');
                if(spl.Length == 1)
                {
                    if (GameManager.IsInt(spl[0]))
                        return GameManager.GetInt(spl[0]).ToString();
                    if (GameManager.IsString(spl[0]))
                        return GameManager.GetString(spl[0]).ToString();
                }
                string obj = spl[0];
                string var = spl[1];
                RPGObject rpgobj = GameObject.Find(obj).GetComponent<RPGObject>();
                if(rpgobj.IsInt(var))
                    return rpgobj.GetInt(var).ToString();
                if(rpgobj.IsString(var))
                    return rpgobj.GetString(var).ToString();
                return "0";
            }
        }
}
