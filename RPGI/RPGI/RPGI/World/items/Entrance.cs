using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class Entrance : Item
    {

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " walks towards what used to be the entrance.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return "The collapsed entrance " + character.getName() + " came in from. It seems turning back "
				+ "is impossible...";
	}

	public String getAbbreviation()
	{
		return "I";
	}

	public Items getKey()
	{
		return Items.Entrance;
	}

}
}
