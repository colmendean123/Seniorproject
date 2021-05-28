using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Scripting{
    public class Tokenizer
	{
		//parsed tokens
		private List<String> tokens;
		//unconverted tokens, without any kind of parsing
		private List<String> unconverted;
		private int next;
		public string name;

		//seperate commandtext into an arraylist, removing any blanks, just like a real parser.
		public Tokenizer(String commandText, string name, CommandRouter thisrouter)
		{
			this.name = name;
			this.next = 0;
			//seperates the : from the rest of the statement if there is one.
			if (commandText[commandText.Length-1] == ':') {
				if (commandText[commandText.LastIndexOf(':') - 1] != ' ')
                {
					commandText = commandText.Replace(':', ' ');
					commandText += ':';
                }
			}
			tokens = new List<String>();
			unconverted = new List<string>();
			//the code for if statements. A real, metaphysical nightmare. Goes through the list of strings, replacing as necessary. concats the remaining together.
			List<string> strings = new List<string>();
			if(commandText.ToLower().StartsWith("if")){
				
				int outers = 0;
				int start = 0;
				int end = 0;
				for(int i = 0; i < commandText.Length; ++i){
					//get the outermost parenthesis, and evaluate only inside that. Doesn't allow for nested ANDS or ORS, but could potentially be updated to.
					if(commandText[i] == '('){
						if(outers==0){
							
							start = i+1;
							strings.Add(commandText.Substring(end, start-end));
						}
						outers += 1;
					}
					if(commandText[i] == ')'){
						outers -= 1;
						if(outers == 0){
							end = i;
							strings.Add(Parentheses(ParseVars(commandText.Substring(start, end-start))));
						}
					}
					
				}
				strings.Add(commandText.Substring(end));
			}
			if(strings.Count() > 0){
				commandText = "";
				foreach(string i in strings){
					commandText += i;
				}
			}
			commandText = commandText.Replace('(', ' ');
			commandText = commandText.Replace(')', ' ');
			String[] stringgetter = commandText.Split('"');
			bool ifswitch = false;
			for (int i = 0; i < stringgetter.Length; i++)
			{

				//for every other split "" we take a string. Otherwise we take a token.
				if(i % 2 == 1){
					string parsed = stringgetter[i].Trim();
					parsed = (ParseVars(parsed));
					tokens.Add(parsed);
				}
				else{
					//trim this so no blank spaces get added to tokens.
					String[] tokengetter = stringgetter[i].Trim().Split(' ');
					if (thisrouter != null)
					{
						foreach (String token in tokengetter)
						{
							string tok = token;
							if (thisrouter.CheckCommand(token.ToUpper()))
								ifswitch = true;
							//add things that register as commands, targets, or variables. we'll sort those later.
							if (thisrouter.CheckCommand(token.ToUpper()))
							{
								tok = token.ToUpper();
							}
							if (tok.Trim() != "")
							{
								if (ifswitch == true && tok.StartsWith("$"))
								{
									tok = ParseVars(tok);
								}
								tokens.Add(tok.Trim());
							}
						}
                    }
                    else
                    {
						foreach(String token in tokengetter)
                        {
							string tok = token.Trim();

							if (ifswitch == true && tok.StartsWith("$"))
							{
								tok = ParseVars(tok);
							}

							if (tok != "")
								tokens.Add(tok);
                        }
                    }
				}
				ifswitch = false;
			}
		}

		public string Parentheses(string substring){
			return VariableMath.Eval(substring).ToString();
		}


		//parses variables for use in the final
		public String ParseVars(string line){
			string parsed = "";
			//split on spaces
			string[] parseinfo = line.Split(' ');
			for(int i = 0; i < parseinfo.Length; ++i){
				//if the parsed var starts with $
				
				if(parseinfo[i].Contains("$") && !parseinfo[i].Equals("$this")){
					if(!parseinfo[i].Substring(0,1).Equals("\\")){
						//var length is 2 to compensate for the $ and the . that won't be counted
						int varlen = 2;
						//makes the varlength not break on the first non-character
						bool first = false;
						foreach(char c in parseinfo[i].Substring(1)){
							if(char.IsLetter(c) || char.IsNumber(c)){
								varlen++;
							}else{
								if(first == false)
									first = true;
								else
									break;
							}
						}
						if (first == false)
							parseinfo[i] = GameManager.ParseVar(parseinfo[i].Substring(1));
						else
							//replace the $var.varname with the actual parsed var
							parseinfo[i] = GameManager.ParseVar(parseinfo[i].Substring(0, varlen)) + parseinfo[i].Substring(varlen);
					}
					else
						parseinfo[i] = parseinfo[i].Substring(1);
				}
				parsed += parseinfo[i] + " ";
			}
			return parsed;
		}
		

		// Returns the String contained in tokens that the index next is currently pointing to, then advances next 1
		public String GetNext()
		{
			if (tokens.Count == 0)
				return null;

			int currentIndex = this.next;
			if (currentIndex >= this.tokens.Count)
				return null;
			this.next++;
			if (tokens[currentIndex] == "$this")
			{
				return name;
			}
			return tokens[currentIndex].Trim();
		}

		public int GetStep(){
			return next;
		}

		public String Get(int index)
		{
			if (index >= this.tokens.Count)
				throw new SystemException("Error! Invalid token!");
			if (tokens[index] == "$this")
				return name;
			return tokens[index];
		}

		//get size of tokenizer. Length is size-1
		public int Size()
		{
			return tokens.Count;
		}
	}
}