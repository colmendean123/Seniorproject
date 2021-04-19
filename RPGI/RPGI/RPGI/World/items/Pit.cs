using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class Pit : Item
	{

	public String trigger(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		return character.getName() + " fell into a pit.";
	}

	public String interact(Character character)
	{
		if (character == null)
		{
			throw new ArgumentException("Passed hero was null.");
		}
		var rng = new Random();
		int damage = rng.Next(20) + 1;
		character.takeDamage(damage);
		return "";
		//return hero.getName() + " lost " + damage + " hit points due to a pit.";
	}

	public String getAbbreviation()
	{
		return "T";
	}

	public Items getKey()
	{
		return Items.Pit;
	}

}
}
