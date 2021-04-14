using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Monster : WorldCharacter
{
	private double chanceToHeal;
	private int minHeal, maxHeal;

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

	//public new void subtractHitPoints(int hitPoints)
	//{
	//	base.subtractHitPoints(hitPoints);
	//	//AttackPool.getInstanceOf().getBasicHeal().heal(this);
	//}

}