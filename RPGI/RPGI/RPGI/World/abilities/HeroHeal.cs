using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class HeroHeal : SpecialAbility
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

			int hPoints;
			var rng = new Random();

			hPoints = (int)(rng.Next() * (50 - 25 + 1)) + 25;
			source.addHitPoints(hPoints);
			Console.WriteLine(source.getName() + " added [" + hPoints + "] points.\n" + "Total hit points remaining are: "
					+ source.getHitPoints());
			Console.WriteLine(target.getName() + " disliked that!");
			Console.WriteLine();
		}

		public String specialDesc()
		{
			return "Heal Self";
		}

		public Abilities getKey()
		{
			return Abilities.HeroHeal;
		}
	}
}
