using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class CrushingBlow : SpecialAbility
	{
		public void special(WorldCharacter source, WorldCharacter target)
		{
			var rng = new Random();

			if (source == null)
			{
				throw new ArgumentException("Atacker passed was null.");
			}
			else if (target == null)
			{
				throw new ArgumentException("Defender passed was null.");
			}

			if (rng.Next() <= .4)
			{
				int damage = (int)(rng.Next() * 76) + 100;
				Console.WriteLine(source.getName() + " lands a CRUSHING BLOW for " + damage + " damage!");
				target.subtractHitPoints(damage);
			}
			else
			{
				Console.WriteLine(source.getName() + " failed to land a crushing blow");
				Console.WriteLine("\n");
			}
		}

		public String specialDesc()
		{
			return "Crushing Blow";
		}

		public Abilities getKey()
		{
			return Abilities.CrushingBlow;
		}
	}
}
