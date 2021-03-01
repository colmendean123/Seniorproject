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

		//seperate commandtext into an arraylist, removing any blanks, just like a real parser.
		public Tokenizer(String commandText)
		{
			this.next = 0;
			tokens = new List<String>();
			unconverted = new List<string>();
			String[] stringgetter = commandText.Split('"');
			for(int i = 0; i < stringgetter.Length; i++)
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
					foreach(String token in tokengetter)
					{
						//add things that register as commands, targets, or variables. we'll sort those later.
						tokens.Add(token.Trim());
					}
				}
				
			}

		}

		//parses variables for use in the final
		public String ParseVars(string line){
			string parsed = "";
			//split on spaces
			string[] parseinfo = line.Split(' ');
			for(int i = 0; i < parseinfo.Length; ++i){
				//if the parsed var starts with $
				if(parseinfo[i].Contains("$")){
					if(!parseinfo[i].Substring(0,1).Equals("\\")){
						//var length is 2 to compensate for the $ and the . that won't be counted
						int varlen = 2;
						//makes the varlength not break on the first non-character
						bool first = false;
						foreach(char c in parseinfo[i].Substring(1)){
							if(char.IsLetter(c)){
								varlen++;
							}else{
								if(first == false)
									first = true;
								else
									break;
							}
						}
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
			return tokens[currentIndex];
		}

		public int GetStep(){
			return next;
		}

		public String Get(int index)
		{
			if (index >= this.tokens.Count)
				throw new SystemException("Error! Invalid token!");
			return tokens[index];
		}

		//get size of tokenizer. Length is size-1
		public int Size()
		{
			return tokens.Count;
		}
	}
}