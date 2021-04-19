using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class PillarOfEncapsulation : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		character.addItem(this.getKey());
		return character.getName() + " picked up a pillar of Encapsulation.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName()
				+ " examines the pillar, it appears to be made out of an extremly hard material.";
	}

	public String getAbbreviation()
	{
		return "P";
	}

	public Items getKey()
	{
		return Items.PillarOfEncapsulation;
	}

}
}
