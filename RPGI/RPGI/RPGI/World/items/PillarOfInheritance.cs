using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class PillarOfInheritance : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		character.addItem(this.getKey());
		return character.getName() + " picked up a pillar of Inheritance.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " examines the pillar, the pillar is partially damaged."
				+ " Parts of it have been chiped away, revealing the structure of the pillar.";
	}

	public String getAbbreviation()
	{
		return "P";
	}

	public Items getKey()
	{
		return Items.PillarOfInheritance;
	}

}
}
