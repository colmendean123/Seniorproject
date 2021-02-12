using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.Tokenizer
{
	public class Tokenizer
	{
		private List<String> tokens;
		int arg;

		private int next;

		//seperate commandtext into an arraylist, removing any blanks, just like a real parser.
		public Tokenizer(String commandText)
		{
			arg = 0;
			this.next = 0;
			tokens = new List<String>();

			String[] tokengetter = commandText.Split("\\s+");
			foreach(String token in tokengetter)
			{
				tokens.Add(token.Trim());
			}
		}

		// Returns the String contained in tokens that the index next is currently pointing to, then advances next 1
		public String getNext()
		{
			if (tokens.Count == 0)
				return null;

			int currentIndex = this.next;
			if (currentIndex >= this.tokens.Count)
				return null;
			this.next++;
			return tokens[currentIndex];
		}

		public String get(int index)
		{
			if (index >= this.tokens.Count)
				throw new SystemException("Error! Invalid token!");
			return tokens[index];
		}

		//get size of tokenizer. Length is size-1
		public int size()
		{
			return tokens.Count;
		}

		/*make it easy to call exception throws like invalid token. Return this at the bottom of every parse function.
		public A_Command invalidToken() throws RuntimeException
		{
			throw new RuntimeException("Error! invalid token! ");

		}

		public A_Command invalidToken(String message) throws RuntimeException
		{
			throw new RuntimeException(message);
		}
		*/
	}
}
