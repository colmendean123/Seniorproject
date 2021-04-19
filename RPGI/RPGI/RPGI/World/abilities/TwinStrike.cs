using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class TwinStrike : SpecialAbility
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
			double two = rng.NextDouble();
			if (two <= .4)
			{
				Console.WriteLine(source.getName() + " hits twice!");
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);

			}
			else if (two >= .8)
			{
				Console.WriteLine("Oh no! " + source.getName() + " missed both attacks!");
			}
			else
			{
				Console.WriteLine(source.getName() + " hits once!");
				AttackPool.getInstanceOf().getBasicAttack().attack(source, target);
			}
		}

		public String specialDesc()
		{
			return "Twin Strike";
		}

		public Abilities getKey()
		{
			return Abilities.TwinStrike;
		}
	}
}
