using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Scripting{
    public class VariableMath
    {
        private static int Add(int x, int y){
            return x + y;
        }

        private static int Subtract(int x, int y){
            return x - y;
        }

        private static int Multiply(int x, int y){
            return x * y;
        }

        private static int Division(int x, int y){
            return Mathf.RoundToInt((float)x / (float)y);
        }


        private static string[] _operators = { "-", "+", "/", "*", "=="};
        private static System.Func<int, int, int>[] _operations = {
            (a1, a2) => Subtract(a1, a2),
            (a1, a2) => Add(a1, a2),
            (a1, a2) => Division(a1, a2),
            (a1, a2) => Multiply(a1, a2),
            (a1, a2) => Equals(a1, a2),
        };

        public static int Equals(int x, int y){
            if(x == y)
                return 1;
            return 0;
        }

        public static int Eval(string expression)
        {
            List<string> tokens = GetTokens(expression);
            Stack<int> operandStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count) {
                string token = tokens[tokenIndex];
                if (token == "(") {
                    string subExpr = GetSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")") {
                    throw new System.Exception("Mis-matched parentheses in expression");
                }
                //If this is an operator  
                if (System.Array.IndexOf(_operators, token) >= 0) {
                    while (operatorStack.Count > 0 && System.Array.IndexOf(_operators, token) < System.Array.IndexOf(_operators, operatorStack.Peek())) {
                        string op = operatorStack.Pop();
                        int arg2 = operandStack.Pop();
                        int arg1 = operandStack.Pop();
                        operandStack.Push(_operations[System.Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorStack.Push(token);
                } else {

                    operandStack.Push(int.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0) {
                string op = operatorStack.Pop();
                int arg2 = operandStack.Pop();
                int arg1 = operandStack.Pop();
                operandStack.Push(_operations[System.Array.IndexOf(_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }
            
        private static string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0) {
                string token = tokens[index];
                if (tokens[index] == "(") {
                    parenlevels += 1;
                }

                if (tokens[index] == ")") {
                    parenlevels -= 1;
                }

                if (parenlevels > 0) {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0)) {
                throw new System.Exception("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private static List<string> GetTokens(string expression)
        {
        string operators = "()^*/+-";
        List<string> tokens = new List<string>();
        StringBuilder sb = new StringBuilder();

        foreach (char c in expression.Replace(" ", string.Empty)) {
            if (operators.IndexOf(c) >= 0) {
                if ((sb.Length > 0)) {
                    tokens.Add(sb.ToString());
                    sb.Length = 0;
                }
                    tokens.Add(c.ToString());
                } else {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0)) {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}
