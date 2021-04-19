using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class Room
	{

		private static long serialVersionUID = 5977389200185624172L;
		private String top;
		private String mid;
		private String bot;
		private List<Items> items;
		private Monster m;
		private Items uniqueItem;
		private double healPotChance;
		private double visPotChance;
		private double trapChance;

		public Room()
		{
			top = "* * *";
			mid = "* E *";
			bot = "* * *";
			items = new List<Items>();
			m = null;
			uniqueItem = Items.Nothing;
			healPotChance = .2;
			visPotChance = .1;
			trapChance = .1;
		}

		public Boolean hasMonster()
		{
			return m != null;
		}

		public Monster getMonster()
		{
			Monster mon = this.m;
			this.m = null;
			setLetter();
			return mon;
		}

		public void setMonster(Monster monster)
		{
			if (hasMonster())
			{
				throw new ArgumentException("Room already has a Monster");
			}
			this.m = monster;
		}

		public Boolean hasItems()
		{
			return items.Count > 0;
		}

		public List<Items> getItems()
		{
			return this.items;
		}

		public void clearItems()
		{
			this.items.Clear();
			setLetter();
		}

		public void setItem(Items item)
		{
			if (item == null)
			{
				throw new ArgumentException("Passed item was null");
			}
			items.Add(item);
		}

		public Boolean hasUniqueItem()
		{
			return !(this.uniqueItem == Items.Nothing);
		}

		public Items getUniqueItem()
		{
			Items item = this.uniqueItem;
			if (item != Items.Entrance && item != Items.Exit)
			{
				this.uniqueItem = Items.Nothing;
			}
			setLetter();
			return item;
		}

		public Items peekUniqueItem()
		{
			return this.uniqueItem;
		}

		public void setUnique(Items item)
		{
			if (hasUniqueItem())
			{
				throw new ArgumentException("Room already has a Unique Item");
			}
			this.uniqueItem = item;
		}

		public void buildRoom(Room[,] world, int x, int y)
	{
		try
		{
			if (y > 0)
			{
				top = "* - *";
			}
			if (y < world.Length - 1)
			{
				bot = "* - *";
			}
			if (x > 0)
			{
				mid = "|" + mid.Substring(1);
			}
			if (x < world.GetLength(0) - 1)
			{
				mid = mid.Substring(0, world.GetLength(1) - 1) + "|";
			}
		}
		catch (ArgumentException e)
		{

		}
	}

	public String interactUnique(Character c)
	{
		if (uniqueItem != null)
		{
			String message = AttackPool.getInstanceOf().getItem(uniqueItem).interact(c);
			if (uniqueItem != Items.Entrance && uniqueItem != Items.Exit)
			{
				this.uniqueItem = Items.Nothing;
			}
			setLetter();
			return message;
		}
		else
		{
			return "";
		}
	}

	public void populateRoomItems(Room[,] world, int x, int y)
	{
		buildRoom(world, x, y);
		if (!hasUniqueItem())
		{
			var rng = new Random();
			double chance = rng.NextDouble();
			if (chance < healPotChance)
			{
				items.Add(Items.HealingPotion);
			}
			chance = rng.NextDouble();
			if (chance < visPotChance)
			{
				items.Add(Items.VisionPotion);
			}
			chance = rng.NextDouble();
			if (chance < trapChance)
			{
				items.Add(Items.Pit);
			}
		}
		setLetter();
	}

	private void setLetter()
	{
		String letter = "";
		if (this.uniqueItem != null)
		{
			letter += AttackPool.getInstanceOf().getItem(uniqueItem).getAbbreviation();
		}
		if (this.m != null)
		{
			letter += "X";
		}
		if (this.items.Count != 0)
		{
			foreach(Items item in this.items)
			{
				letter += AttackPool.getInstanceOf().getItem(item).getAbbreviation();
			}
		}
		if (letter.Length > 1)
		{
			this.mid = this.mid.Substring(0, 2) + "M" + this.mid.Substring(3);
		}
		else if (letter.Length == 1)
		{
			this.mid = this.mid.Substring(0, 2) + letter + this.mid.Substring(3);
		}
		else
		{
			this.mid = this.mid.Substring(0, 2) + "E" + this.mid.Substring(3);
		}
	}

	public void setRoom(Room r)
	{
		if (r == null)
		{
			throw new ArgumentException();
		}
		this.top = r.top;
		this.mid = r.mid;
		this.bot = r.bot;
		this.items = r.items;
		this.m = r.m;
		this.uniqueItem = r.uniqueItem;
	}

	public String toString()
	{
		return top + "\n" + mid + "\n" + bot;
	}

	public String getTop()
	{
		return this.top;
	}

	public String getMid()
	{
		return this.mid;
	}

	public String getBot()
	{
		return this.bot;
	}
}
}
