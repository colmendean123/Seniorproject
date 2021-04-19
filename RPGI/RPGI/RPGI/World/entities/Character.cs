using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    public class Character : WorldCharacter
    {
		private static long serialVersionUID = 7325368055168446121L;
		private double chanceToBlock;
		private ArrayList items;
		private Abilities specialAttack;
		private int posX;
		private int posY;

		public Character(String name, int hitPoints, int attackSpeed, double chanceToHit, int damageMin, int damageMax,
				double chanceToBlock, Abilities sp, Weapons w) : base(name, hitPoints, attackSpeed, chanceToHit, damageMin, damageMax, w)
		{
			this.specialAttack = sp;
			this.chanceToBlock = chanceToBlock;
			this.items = new ArrayList();
		}

		public double getChanceToBlock()
		{
			return this.chanceToBlock;
		}

		public int getNumPillars()
		{
			int count = 0;
			foreach(Items item in items)
			{
				if (item == Items.PillarOfAbstraction || item == Items.PillarOfEncapsulation
						|| item == Items.PillarOfInheritance || item == Items.PillarOfPolymorphism)
				{
					count++;
				}
			}
			return count;
		}

		private int getNumHeal()
		{
			int count = 0;
			foreach(Items item in items)
			{
				if (item == Items.HealingPotion)
				{
					count++;
				}
			}
			return count;
		}

		private int getNumVision()
		{
			int count = 0;
			foreach(Items item in items)
			{
				if (item == Items.VisionPotion)
				{
					count++;
				}
			}
			return count;
		}

		public void addItem(Items item)
		{
			if (item == null)
			{
				throw new ArgumentException("Item passed was null.");
			}
			items.Add(item);
		}

		public void consumeHeal()
		{
			foreach(Items item in items)
			{
				if (item == Items.HealingPotion)
				{
					Console.WriteLine(AttackPool.getInstanceOf().getItem(item).interact(this));
					this.items.Remove(item);
					return;
				}
			}
		}

		public void consumeVision()
		{
			foreach(Items item in items)
			{
				if (item == Items.VisionPotion)
				{
					Console.WriteLine(AttackPool.getInstanceOf().getItem(item).interact(this));
					this.items.Remove(item);
					return;
				}
			}
		}

		public void special(WorldCharacter enemy)
		{
			AttackPool.getInstanceOf().getSpecialAbility(specialAttack).special(this, enemy);
		}

		public String readSpecial()
		{
			return AttackPool.getInstanceOf().getSpecialAbility(specialAttack).specialDesc();
		}

		public void readName()
		{
			Console.WriteLine("Enter character name: ");
			this.setName(Console.ReadLine());
		}

		public Boolean defend()
		{
			var rng = new Random();
			return rng.Next() <= chanceToBlock;

		}

		public void setX(int x)
		{
			this.posX = x;
		}

		public int getX()
		{
			return this.posX;
		}

		public void setY(int y)
		{
			this.posY = y;
		}

		public int getY()
		{
			return this.posY;
		}

		public void setCharacter(WorldCharacter c)
		{
			if (c.GetType() == typeof(Character)) {
				base.setCharacter(c);
				Character pc = (Character)c;
				this.specialAttack = pc.specialAttack;
				this.chanceToBlock = pc.chanceToBlock;
				this.items = pc.items;
				this.posX = pc.posX;
				this.posY = pc.posY;
			}
		}

	public String toString()
		{
			return "Name: " + this.getName() + "\n" + "Hit Points: " + this.getHitPoints() + "\n" + "Healing Potions: "
					+ this.getNumHeal() + "\n" + "Vision Potioms: " + this.getNumVision() + "\n" + "Pillars of OO: "
					+ this.getNumPillars();
		}

		public new void subtractHitPoints(int hitPoints)
		{
			if (defend())
			{
				Console.WriteLine(this.getName() + " BLOCKED the attack!");
			}
			else
			{
				base.subtractHitPoints(hitPoints);
			}

		}

		public void takeDamage(int hitPoints)
		{
			base.subtractHitPoints(hitPoints);
		}
	}
}
