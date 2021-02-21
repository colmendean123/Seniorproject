using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Scripting{
    public class Tokenizer
	{
		private List<String> tokens;
		private List<String> strings;
		//list of arguments
		private List<String> args;
		private int next;

		//seperate commandtext into an arraylist, removing any blanks, just like a real parser.
		public Tokenizer(String commandText)
		{
			this.next = 0;
			tokens = new List<String>();
			args = new List<string>();
			String[] stringgetter = commandText.Split('"');
			for(int i = 0; i < stringgetter.Length; i++)
			{
				//for every other split "" we take a string. Otherwise we take a token.
				if(i % 2 == 1)
					tokens.Add(stringgetter[i].Trim());
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

		public string GetArg(int index){
			return args[index];
		}

		public void AddArg(string arg){
			args.Add(arg);
		}
    }
}