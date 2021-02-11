using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    public class Monster : WorldCharacter
    {
		private static long serialVersionUID = -4784406317110797124L;
		private double chanceToHeal;
		private int minHeal, maxHeal;

		public Monster(String name, int hitPoints, int attackSpeed, double chanceToHit, double chanceToHeal, int damageMin,
				int damageMax, int minHeal, int maxHeal, Weapons w) : base(name, hitPoints, attackSpeed, chanceToHit, damageMin, damageMax, w)
		{
			this.chanceToHeal = chanceToHeal;
			this.maxHeal = maxHeal;
			this.minHeal = minHeal;

		}

		public double getChanceToHeal()
		{
			return this.chanceToHeal;
		}

		public int getMinHeal()
		{
			return this.minHeal;
		}

		public int getMaxHeal()
		{
			return this.maxHeal;
		}

		public void subtractHitPoints(int hitPoints)
		{
			base.subtractHitPoints(hitPoints);
			AttackPool.getInstanceOf().getBasicHeal().heal(this);
		}

	}
}
