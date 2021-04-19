using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class HealPot : Item
	{

		public String trigger(Character character)
		{
			if (character == null)
			{
				throw new ArgumentException("Passed hero was null.");
			}
			character.addItem(this.getKey());
			return character.getName() + " picked up a healing potion.";
		}

		public String interact(Character character)
		{
			if (character == null)
			{
				throw new ArgumentException("Passed hero was null.");
			}
			var rng = new Random();
			int heal = rng.Next(11) + 5;
			character.addHitPoints(heal);
			return character.getName() + " drank a Healing Potion and healed for " + heal + " points";
		}

		public String getAbbreviation()
		{
			return "H";
		}

		public Items getKey()
		{
			return Items.HealingPotion;
		}
	}
}
