using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting{
public static class ParsingCommands {
        public static void ChangeVar(string target, string set){
            target = target.Substring(1);
            set = VariableMath.Eval(set).ToString();
            string[] spl = target.Split('.');
            string obj = spl[0];
            string var = spl[1];
            RPGObject rpgobj = GameObject.Find(obj).GetComponent<RPGObject>();
            int parsed;
            if(int.TryParse(set, out parsed))
                rpgobj.ChangeInt(var, parsed);
            else
                rpgobj.ChangeString(var, set);
        }

        public static string GetVar(string target){
        target = target.Substring(1);
        string[] spl = target.Split('.');
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
