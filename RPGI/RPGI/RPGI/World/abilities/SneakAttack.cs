using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class SneakAttack : SpecialAbility
	{
		public void special(WorldCharacter source, WorldCharacter target)
		{
			var rand = new Random();

			if (source == null)
			{
				throw new ArgumentException("Atacker passed was null.");
			}
			else if (target == null)
			{
				throw new ArgumentException("Defender passed was null.");
			}

			double surprise = rand.NextDouble();
			if (surprise <= .4)
			{
				Console.WriteLine("Surprise attack was successful!\n" + source.getName() + " gets an additional turn.");
				source.setTurns(source.getTurns() + 1);
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
			}
			else if (surprise >= .9)
			{
				Console.WriteLine("Uh oh! " + target.getName() + " saw you and" + " blocked your attack!");
			}
			else
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
		}

		public String specialDesc()
		{
			return "Sneak Attack";
		}

		public Abilities getKey()
		{
			return Abilities.SneakAttack;
		}
	}
}
