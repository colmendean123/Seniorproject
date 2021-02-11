using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class VisionPot : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		character.addItem(this.getKey());
		return character.getName() + " picked up a vision potion";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " drank a Vison Potion and can now see the rooms around them.";
	}

	public String getAbbreviation()
	{
		return "V";
	}

	public Items getKey()
	{
		return Items.VisionPotion;
	}

}
}
