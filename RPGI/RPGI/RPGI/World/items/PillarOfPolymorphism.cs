using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class PillarOfPolymorphism : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		character.addItem(this.getKey());
		return character.getName() + " picked up a pillar of Polymorphism.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " examines the pillar, which emits a dim glow."
				+ " The pillar's appearance seems to be shifting each time " + character.getName() + " looks away.";
	}

	public String getAbbreviation()
	{
		return "P";
	}

	public Items getKey()
	{
		return Items.PillarOfPolymorphism;
	}

}
}
