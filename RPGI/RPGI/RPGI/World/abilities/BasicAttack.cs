using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class BasicAttack
	{
		public void attack(WorldCharacter source, WorldCharacter target)
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
			Boolean canAttack = rng.Next() <= source.getChanceToHit();
			int damage = 0;

			if (canAttack)
			{
				damage = (int)(rng.Next() * (source.getMaxDamage() - source.getMinDamage() + 1))
						+ source.getMinDamage();
				Console.WriteLine(
						source.getName() + " " + AttackPool.getInstanceOf().getWeapon(source.getWeapon()).attackDesc() + " "
								+ target.getName() + "!");
				target.subtractHitPoints(damage);

				Console.WriteLine();
			}
			else
			{

				Console.WriteLine(source.getName() + "'s attack on " + target.getName() + " failed!");
				Console.WriteLine();
			}

		}
	}
}
