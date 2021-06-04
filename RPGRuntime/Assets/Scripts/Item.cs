using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    string name;
    SortedDictionary<string, string[]> actions = new SortedDictionary<string, string[]>();
    RPGObject player = GameManager.GetPlayer().GetComponent<RPGObject>();
    bool equipped = false;

    public string GetName()
    {
        return name;
    }


    public Item(string item)
    {
        if (item == null)
            return;
        Debug.Log(item);
        name = item.Substring(0, item.Length-4);
        Debug.Log(name);
        FunctionParser funcparser = new FunctionParser();
        int functionstart = 0;
        string[] parameters = GameManager.LoadFile("Items", item);
        foreach (string str in parameters)
        {
            if (str.StartsWith("["))
            {
                break;
            }
            ++functionstart;
        }
        for (int i = functionstart; i < parameters.Length; ++i)
        {
            if (parameters[i].StartsWith("["))
            {
                AddFunction(funcparser.Export());
                string n = parameters[i].Replace("[", "");
                n = n.Replace("]", "");

                funcparser = new FunctionParser(n);
                continue;
            }
            funcparser.AddLine(parameters[i]);
        }
        AddFunction(funcparser.Export());
    }

    public void AddFunction((string, string[]) func)
    {
        if (func.Item1 == null)
            return;
        string name = func.Item1;

        string[] function = func.Item2;
        if (name.StartsWith("ACTION:"))
        {

            name = name.Substring(7);
            actions.Add(name, function);
        }
        else
        {
            name = name.ToUpper();
            actions.Add(name, function);
        }
    }

    public List<string> GetActions()
    {
        List<string> ret = new List<string>();
        foreach(string i in actions.Keys)
        {
            ret.Add(i);
        }
        return ret;
    }


    public void Equip()
    {
        int tryslot = 0;
        string[] parameters = GameManager.LoadFile("Items", name);
        foreach (string str in parameters)
        {
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            if (key.ToUpper() == "SLOT")
            {
                int.TryParse(value, out tryslot);
                if (player.equippeditem[tryslot] == false)
                {
                    GameManager.Print("There is");
                    return;
                }
                else
                {
                    equipped = true;
                    player.equippeditem[tryslot] = true;
                }
            }
        }

        foreach (string j in parameters)
        {
            string str = j.Replace("$this", "$" + name);
            if (str.StartsWith("["))
            {
                break;
            }
            //split into key value pair
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            //check if int or string
            if (key.ToUpper() == "NAME")
            {
                continue;
            }

            if (int.TryParse(value, out int i))
            {
                if (player.GetInt(key) == 0)
                    player.ChangeInt(key, i);
                else
                    player.ChangeInt(key, player. GetInt(key) + i);
            }
            else
            {
                player.ChangeString(key, value);
            }
        }
    }

    public void UnEquip()
    {
        int tryslot = 0;
        string[] parameters = GameManager.LoadFile("Items", name);
        foreach (string str  in parameters)
        {
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            if (key.ToUpper() == "SLOT")
            {
                int.TryParse(value, out tryslot);
                if (player.equippeditem[tryslot] == true)
                {
                    GameManager.Print("There is already something in that slot!");
                    return;
                }
                else
                {
                    player.equippeditem[tryslot] = true;
                }
            }
        }

        foreach (string j in parameters)
        {
            string str = j.Replace("$this", "$" + name);
            if (str.StartsWith("["))
            {
                break;
            }
            //split into key value pair
            string key = str.Split('=')[0].Trim().ToUpper();
            string value = str.Split('=')[1].Trim();
            //check if int or string
            if (key.ToUpper() == "NAME")
            {
                continue;
            }

            if (int.TryParse(value, out int i))
            {
                if (player.GetInt(key) == 0)
                    player.ChangeInt(key, i);
                else
                    player.ChangeInt(key, player.GetInt(key) + i);
            }
            else
            {
                player.ChangeString(key, value);
            }
        }
    }
}
