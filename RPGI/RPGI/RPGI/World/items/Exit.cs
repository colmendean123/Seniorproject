using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class Exit : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed character was null.");
		}
		return character.getName() + " walks towards the exit.";
	}

	public String interact(Character charater)
	{
		if (charater == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		if (charater.getNumPillars() < 4)
		{
			return "The sealed exit! " + charater.getName() + " may only leave once they possess "
					+ "all four pillars of OO.";
		}
		else
		{
			return charater.getName() + " has brought all four pillars to the exit!";
		}
	}

	public String getAbbreviation()
	{
		return "O";
	}

	public Items getKey()
	{
		return Items.Exit;
	}

}
}
