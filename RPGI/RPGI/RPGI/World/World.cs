using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class World
	{

		private static long serialVersionUID = 4374465783899583065L;
		private Room[,] worldArray;

		public World(int x, int y, int monsters)
		{
			generateWorld(x, y);
			populateUniqueItems(x, y);
			populateMonsters(x, y, monsters);
			populateRoomItems(x, y);
		}

		private void generateWorld(int x, int y)
		{
			if (x < 3 || y < 3)
			{
				throw new ArgumentException("Dungeon may not be smaller than 3 x 3");
			}
			this.worldArray = new Room[y,x];
			for (int i = 0; i < y; i++)
			{
				for (int j = 0; j < x; j++)
				{
					worldArray[i,j] = new Room();
				}
			}
		}

		private void populateUniqueItems(int x, int y)
		{
			if (x < 3 || y < 3)
			{
				throw new ArgumentException("Dungeon may not be smaller than 3 x 3");
			}
			ArrayList unique = generateUniques();
			foreach (Items item in unique)
			{
				Boolean placed = false;
				while (!placed)
				{
					var rng = new Random();
					int roomY = (int)(rng.Next() * y);
					int roomX = (int)(rng.Next() * x);
					if (placed = !this.worldArray[roomY,roomX].hasUniqueItem())
					{
						this.worldArray[roomY,roomX].setUnique(item);
					}
				}
			}
		}

		private void populateMonsters(int x, int y, int monsters)
		{
			if (x < 3 || y < 3)
			{
				throw new ArgumentException("Dungeon may not be smaller than 3 x 3");
			}
			if (((y * x) - monsters) < 6)
			{
				throw new ArgumentException("Too many monsters for this dungeon");
			}
			ArrayList mon = generateMonsterList(monsters);
			foreach (Monster m in mon)
			{
				Boolean placed = false;
				while (!placed)
				{
					var rng = new Random();
					int roomY = (int)(rng.Next() * y);
					int roomX = (int)(rng.Next() * x);
					if (!this.worldArray[roomY,roomX].hasUniqueItem())
					{
						if (placed = !this.worldArray[roomY,roomX].hasMonster())
						{
							this.worldArray[roomY,roomX].setMonster(m);
						}
					}
				}
			}

		}

		private void populateRoomItems(int x, int y)
		{
			if (x < 3 || y < 3)
			{
				throw new ArgumentException("Dungeon may not be smaller than 3 x 3");
			}
			for (int i = 0; i < y; i++)
			{
				for (int j = 0; j < x; j++)
				{
					worldArray[i,j].populateRoomItems(worldArray, j, i);
				}
			}
		}

		private static Monster generateMonster()
		{
			int choice = 0;
			MonsterFactory m = new MonsterFactory();

			var rng = new Random();

			choice = (int)(rng.Next() * 5) + 1;

			switch (choice)
			{
				case 1:
					return m.createMonster(Monsters.Ogre);

				case 2:
					return m.createMonster(Monsters.Gremlin);

				case 3:
					return m.createMonster(Monsters.Skeleton);

				case 4:
					return m.createMonster(Monsters.Minotuar);

				case 5:
					return m.createMonster(Monsters.Bugbear);

				default:
					throw new ArgumentException("Random Object generated a unexpected value of " + choice + ".");

			}
		}

		private ArrayList generateMonsterList(int num)
		{
			ArrayList group = new ArrayList();
			for (int i = 0; i < num; i++)
			{
				group.Add(generateMonster());

			}
			return group;
		}

		private ArrayList generateUniques()
		{
			ArrayList items = new ArrayList();
			items.Add(Items.Entrance);
			items.Add(Items.Exit);
			items.Add(Items.PillarOfAbstraction);
			items.Add(Items.PillarOfEncapsulation);
			items.Add(Items.PillarOfInheritance);
			items.Add(Items.PillarOfPolymorphism);
			return items;
		}

		public Room moveCharacter(Character c, String s)
		{
			if (c == null)
			{
				throw new ArgumentException("Hero passed in was null");
			}
			if (s == null)
			{
				throw new ArgumentException("String passed in was null");
			}

			int characterX = c.getX();
			int characterY = c.getY();
			Room curRoom = worldArray[characterY,characterX];
			Room nextRoom = new Room();

			try
			{
				switch (s)
				{
					case ("North"):
						characterY = characterY - 1;
						nextRoom = worldArray[characterY,characterX];
						break;
					case ("South"):
						characterY = characterY + 1;
						nextRoom = worldArray[characterY,characterX];
						break;
					case ("East"):
						characterX = characterX + 1;
						nextRoom = worldArray[characterY,characterX];
						break;
					case ("West"):
						characterX = characterX - 1;
						nextRoom = worldArray[characterY,characterX];
						break;
					default:
						nextRoom = curRoom;
						break;
				}
				c.setX(characterX);
				c.setY(characterY);
			}
			catch (ArgumentException e)
			{
				Console.WriteLine(c.getName() + " cannot move " + s);
				return curRoom;
			}

			return nextRoom;
		}

		public Room manageEntrance(Character c)
		{
			if (c == null)
			{
				throw new ArgumentException("Hero passed in was null");
			}

			for (int i = 0; i < worldArray.Length; i++)
			{
				for (int j = 0; j < worldArray.GetLength(0); j++)
				{
					Items item = worldArray[i,j].peekUniqueItem();
					if (item == Items.Entrance)
					{
						c.setY(i);
						c.setX(j);
						return worldArray[i,j];
					}
				}
			}
			return null;
		}

		public String displayVision(Character c)
		{
			if (c == null)
			{
				throw new ArgumentException("Hero passed in was null");
			}

			String output = "";

			for (int i = c.getY() - 1; i <= c.getY() + 1; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = c.getX() - 1; k <= c.getX() + 1; k++)
					{
						try
						{
							if (j == 0)
							{
								output += worldArray[i,k].getTop() + " ";
							}
							else if (j == 1)
							{
								output += worldArray[i,k].getMid() + " ";
							}
							else if (j == 2)
							{
								output += worldArray[i,k].getBot() + " ";
							}
						}
						catch (ArgumentException e)
						{
							output += "* * * ";
						}

					}
					output += "\n";
				}
			}

			return output;
		}

		public void setWorld(World w)
		{
			if (w == null)
			{
				throw new ArgumentException("World passed in was null");
			}
			this.worldArray = w.worldArray;
		}

		public String toString()
		{
			String worldString = "";
			for (int i = 0; i < worldArray.Length; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < worldArray.GetLength(0); k++)
					{
						if (j == 0)
						{
							worldString += worldArray[i,k].getTop() + " ";
						}
						else if (j == 1)
						{
							worldString += worldArray[i,k].getMid() + " ";
						}
						else if (j == 2)
						{
							worldString += worldArray[i,k].getBot() + " ";
						}
					}
					worldString += "\n";
				}
			}
			return worldString;
		}
	}
}