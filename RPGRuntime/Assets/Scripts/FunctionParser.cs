using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionParser
{
    List<string> function;
    string name;
    public FunctionParser(string name)
    {
        this.name = name;
        function = new List<string>();
    }

    public FunctionParser()
    {
        this.name = "Default";
        function = new List<string>();
    }

    public void AddLine(string line)
    {
        function.Add(line);
    }

    public (string, string[]) Export()
    {
        if(function.Count == 0)
        {
            return (null,null);
        }
        string[] exp = new string[function.Count];
        for(int i = 0; i < function.Count; ++i) 
        {
            exp[i] = function[i];
        }
        return (name, exp);
    }
}
