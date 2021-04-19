using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class Smite : SpecialAbility
	{
		public void special(WorldCharacter source, WorldCharacter target)
		{

			if (source == null)
			{
				throw new ArgumentException("Atacker passed was null.");
			}
			else if (target == null)
			{
				throw new ArgumentException("Defender passed was null.");
			}

			var rng = new Random();
			double smite = rng.Next();
			if (smite <= .4)
			{
				int damage = (int)(rng.Next() * 88);
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
				Console.WriteLine(
						source.getName() + " SMITES the " + target.getName() + " for " + damage + " extra damage!");
				target.subtractHitPoints(damage);
			}
			else if (smite >= .9)
			{
				Console.WriteLine(source.getName() + " failed to land the attack!");
			}
			else
			{
				Console.WriteLine(source.getName() + " attacks but fails to smite");
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
			}
		}

		public String specialDesc()
		{
			return "SMITE";
		}

		public Abilities getKey()
		{
			return Abilities.Smite;
		}

	}
}
