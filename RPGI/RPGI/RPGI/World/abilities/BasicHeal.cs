using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class BasicHeal
	{
		public void heal(Monster source)
		{
			var rng = new Random();

			if (source == null)
			{
				throw new ArgumentException("Source was null");
			}

			Boolean canHeal = (rng.Next() <= source.getChanceToHeal()) && (source.getHitPoints() > 0);
			int healPoints = 0;

			if (canHeal)
			{
				healPoints = (int)(rng.Next() * (source.getMaxHeal() - source.getMinHeal() + 1)) + source.getMinHeal();
				source.addHitPoints(healPoints);
				Console.WriteLine(source.getName() + " healed itself for " + healPoints + " points.\n"
						+ "Total hit points remaining are: " + source.getHitPoints());
				Console.WriteLine();
			}
		}
	}
}
