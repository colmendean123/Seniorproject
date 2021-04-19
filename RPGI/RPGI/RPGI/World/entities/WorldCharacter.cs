using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public abstract class WorldCharacter
    {
		private static long serialVersionUID = 3792593125795987889L;
		private String name;
		private int hitPoints;
		private int attackSpeed;
		private double chanceToHit;
		private int damageMin, damageMax;
		private int numTurns;
		private Weapons weapon;

		public WorldCharacter(String name, int hitPoints, int attackSpeed, double chanceToHit, int damageMin,
				int damageMax, Weapons weapon)
		{
			this.name = name;
			this.hitPoints = hitPoints;
			this.attackSpeed = attackSpeed;
			this.chanceToHit = chanceToHit;
			this.damageMin = damageMin;
			this.damageMax = damageMax;
			this.weapon = weapon;

		}

		public String getName()
		{
			return name;
		}

		public void setName(String name)
		{
			this.name = name;
		}

		public int getHitPoints()
		{
			return hitPoints;
		}

		public int getAttackSpeed()
		{
			return attackSpeed;
		}

		public double getChanceToHit()
		{
			return this.chanceToHit;
		}

		public int getMinDamage()
		{
			return this.damageMin;
		}

		public int getMaxDamage()
		{
			return this.damageMax;
		}

		public int getTurns()
		{
			return this.numTurns;
		}

		public Weapons getWeapon()
		{
			return this.weapon;
		}

		public void addHitPoints(int hitPoints)
		{
			if (hitPoints <= 0)
				Console.WriteLine("Hitpoint amount must be positive.");
		else
			{
				this.hitPoints += hitPoints;
				Console.WriteLine("Remaining Hit Points: " + hitPoints);

			}
		}

		public void subtractHitPoints(int hitPoints)
		{
			if (hitPoints < 0)
				Console.WriteLine("Hitpoint amount must be positive.");
		else if (hitPoints > 0)
			{
				this.hitPoints -= hitPoints;
				if (this.hitPoints < 0)
					this.hitPoints = 0;
				Console.WriteLine(getName() + " takes <" + hitPoints + "> points of damage.");
				Console.WriteLine(getName() + " now has " + getHitPoints() + " hit points remaining.");
				Console.WriteLine();
			}

			if (this.hitPoints == 0)
				Console.WriteLine(name + " has been killed :-(");

		}

		public void setCharacter(WorldCharacter character)
		{
			this.name = character.name;
			this.hitPoints = character.hitPoints;
			this.attackSpeed = character.attackSpeed;
			this.chanceToHit = character.chanceToHit;
			this.damageMin = character.damageMin;
			this.damageMax = character.damageMax;
			this.weapon = character.weapon;
		}

		public Boolean isAlive()
		{
			return (hitPoints > 0);
		}

		public void setTurns(int turns)
		{
			this.numTurns = turns;
		}

	}
}
