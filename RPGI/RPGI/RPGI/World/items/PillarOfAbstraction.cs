using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class PillarOfAbstraction : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
			character.addItem(this.getKey());
		return character.getName() + " picked up a pillar of Abstraction.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " examines the pillar, as they rotate the pillar parts of it become invisible... "
				+ "alsmot like part of the pillar is hidden from plain sight.";
	}

	public String getAbbreviation()
	{
		return "P";
	}

	public Items getKey()
	{
		return Items.PillarOfAbstraction;
	}

}
}
