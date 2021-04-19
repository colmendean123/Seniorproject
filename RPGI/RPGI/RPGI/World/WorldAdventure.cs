using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace RPGI.World
{
	public class WorldAdventure
	{

		public static void main(String[] args)
		{
			Boolean cont = false;


			do
			{
                World theWorld = new World(5, 5, 5);

				Intro();
				Console.WriteLine();

				Character theCharacter = chooseCharacter();
				Room curRoom = theWorld.manageEntrance(theCharacter);

				do
				{

					if (curRoom.hasMonster())
					{
						Monster theMonster = curRoom.getMonster();
						battle(theCharacter, theMonster);
					}

					Console.WriteLine(curRoom.toString());

					if (theCharacter.isAlive())
					{
						if (curRoom.hasUniqueItem())
						{
							Items uniqueItem = curRoom.getUniqueItem();
							Console.WriteLine(AttackPool.getInstanceOf().getItem(uniqueItem).trigger(theCharacter));
							Console.WriteLine(AttackPool.getInstanceOf().getItem(uniqueItem).interact(theCharacter));
						}

						if (curRoom.hasItems())
						{
							List<Items> curRoomItems = curRoom.getItems();
							foreach (Items i in curRoomItems)
							{
								Console.WriteLine(AttackPool.getInstanceOf().getItem(i).trigger(theCharacter));
								if (i == Items.Pit)
								{
									Console.WriteLine(AttackPool.getInstanceOf().getItem(i).interact(theCharacter));
								}
							}
							curRoom.clearItems();
						}

						Console.WriteLine(theCharacter.toString());

						int act = chooseAction();
						curRoom = executeAction(act, theCharacter, theWorld, curRoom);

					}
				} while (theCharacter.isAlive() && (theCharacter.getNumPillars() != 4 || curRoom.getUniqueItem() != Items.Exit));

				if (theCharacter.isAlive())
				{
					Items uniqueItem = curRoom.getUniqueItem();
					Console.WriteLine(AttackPool.getInstanceOf().getItem(uniqueItem).trigger(theCharacter));
					Console.WriteLine(AttackPool.getInstanceOf().getItem(uniqueItem).interact(theCharacter));
					Victory();
				}
				else
				{
					Console.WriteLine("The hero has been defeated!");
				}
				
				cont = playAgain();
			} while (cont);

		}

		private static int chooseAction()
		{
			//Secret key to display whole dungeon: 627
			int action = 0;
			do
			{
				Console.WriteLine("What do you want to do?\n" +
						"1. Move North\n" +
						"2. Move East\n" +
						"3. Move South\n" +
						"4. Move West\n" +
						"5. Use Healing potion\n" +
						"6. Use Vision potion\n" +
						"7. Save\n" +
						"8. Load\n");
				try
				{
					action = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception e)
				{
					Console.WriteLine("Please enter valid number(1-8)");
				}
			} while ((action < 0 || action > 8) && action != 627);

			return action;
		}

		private static Room executeAction(int d, Character theCharacter, World theWorld, Room curRoom)
		{
			switch (d)
			{
				case 1:
					return curRoom = theWorld.moveCharacter(theCharacter, "North");

				case 2:
					return curRoom = theWorld.moveCharacter(theCharacter, "East");

				case 3:
					return curRoom = theWorld.moveCharacter(theCharacter, "South");

				case 4:
					return curRoom = theWorld.moveCharacter(theCharacter, "West");

				case 5:
					theCharacter.consumeHeal();
					return curRoom;

				case 6:
					theCharacter.consumeVision();
					Console.WriteLine(theWorld.displayVision(theCharacter));
					return curRoom;

				case 7:
					try
					{
						saveState(theWorld, curRoom, theCharacter);
					}
					catch (ArgumentException e)
					{
						Console.WriteLine("Could not save dungeon state...");
						Console.WriteLine(e.ToString());
					}
					return curRoom;

				case 8:
					try
					{
						List<Object> saves = loadSerializedFile();
						theWorld.setWorld((World)saves[0]);
						curRoom.setRoom((Room)saves[1]);
						theCharacter.setCharacter((Character)saves[2]);
					}
					catch (Exception e)
					{
						Console.WriteLine("Could not load dungeon state...");
						Console.WriteLine(e.ToString());
					}
					return curRoom;
				case 627:
					Console.WriteLine(theWorld.toString());
					return curRoom;
				default:
					throw new ArgumentException("Invalid Action");
			}
		}

		private static Character chooseCharacter()
		{
			int choice = 0;
			CharacterFactory character = new CharacterFactory();
			Character toRet;

			Console.WriteLine(
					"Choose a hero:\n" + "1. Warrior\n" + "2. Sorceress\n" + "3. Thief\n" + "4. Paladin\n" + "5. Ranger");
			try
			{
				choice = Convert.ToInt32(Console.ReadLine());
			}
			catch (ArgumentException e)
			{
				
				choice = 0;
			}

			if (choice == 1)
			{
				toRet = character.createCharacter(Characters.Warrior);
			}
			else if (choice == 2)
			{
				toRet = character.createCharacter(Characters.Sorceress);
			}
			else if (choice == 3)
			{
				toRet = character.createCharacter(Characters.Thief);
			}
			else if (choice == 4)
			{
				toRet = character.createCharacter(Characters.Paladin);
			}
			else if (choice == 5)
			{
				toRet = character.createCharacter(Characters.Ranger);
			}
			else if (choice == 32301)
			{
				toRet = character.createCharacter(Characters.Floridaman);
			}
			else
			{
				Console.WriteLine("Invalid entry. Please enter an integer 1 through 5...");
				return chooseCharacter();
			}

			toRet.readName();

			return toRet;
		}

		private static void battle(Character theCharacter, Monster theMonster)
		{
			Console.WriteLine(theCharacter.getName() + " battles " + theMonster.getName());
			Console.WriteLine("---------------------------------------------");

			do
			{

				int turns = theCharacter.getAttackSpeed() / theMonster.getAttackSpeed();
				if (turns == 0)
				{
					turns = 1;
				}
				theCharacter.setTurns(turns);

				while (theCharacter.getTurns() > 0 && theMonster.isAlive())
				{
					int option = 0;
					Console.WriteLine("1. Attack Opponent");
					Console.WriteLine("2. " + theCharacter.readSpecial());
					Console.WriteLine("Choose an option: ");
					try
					{
						option = Convert.ToInt32(Console.ReadLine());
					}
					catch (ArgumentException e)
					{
						option = 0;
					}

					if (option == 1)
					{
						AttackPool.getInstanceOf().getBasicAttack().attack(theCharacter, theMonster);
					}
					else if (option == 2)
					{
						theCharacter.special(theMonster);
					}
					else
					{
						Console.WriteLine("Invalid input...");
						theCharacter.setTurns(theCharacter.getTurns() + 1);
					}

					theCharacter.setTurns(theCharacter.getTurns() - 1);

				}

				if (theMonster.isAlive())
				{
					AttackPool.getInstanceOf().getBasicAttack().attack(theMonster, theCharacter);

				}

			} while (theCharacter.isAlive() && theMonster.isAlive());

			if (!theMonster.isAlive())
				Console.WriteLine(theCharacter.getName() + " was victorious!");
		else if (!theCharacter.isAlive())
				Console.WriteLine(theCharacter.getName() + " was defeated :-(");
		else
				Console.WriteLine("Quitters never win ;-)");

		}

		private static Boolean playAgain()
		{
			String again;

			Console.WriteLine("Play again (y/n)?");
			again = Console.ReadLine();

			return (again == "Y" || again == "y");
		}

		private static void Intro()
		{
			Console.WriteLine("------Welcome Adventurer!------");
			Console.WriteLine("You have answered a quest for the promise of adventure and LOOT upon "
					+ "exploring a mysterious cave.\nAs you reach the cave you find a sign posted at the entrance, it reads: "
					+ "\n\n"
					+ "\"Adventurers and Heroes complete the dungeon inside here by collecting the four pillars of OO."
					+ "\nBeware many obstacles will be in your way, \r\n"
					+ "some helpful items can also be found to keep you alive.\"");
		}

		private static void Victory()
		{
			Console.WriteLine("--------------------------------------------------------------------------------");
			Console.WriteLine("Congratulations by collecting the four pillars of OO!");
			Console.WriteLine("Your Prize is: ");
			Console.WriteLine("50 gold pieces");
		}

		//needs work/////////////////////////////////////////////////////////////////
	private static List<Object> loadSerializedFile() {
			List<Object> itemsToLoad = new List<Object>();
			try
			{
				//File saveFile = new File("./saveGame");
				//ObjectInputStream in = new ObjectInputStream(new FileInputStream(saveFile));
				//itemsToLoad = (ArrayList<Object>) in.readObject();
			//in.close();
				Console.WriteLine("Adventure loaded!");
			}
			catch (ArgumentException e)
			{
				Console.WriteLine(e.ToString());
			}
			return itemsToLoad;
		}

		private static void saveState(World World, Room currentRoom, Character Character)
		{
			List<Object> itemsToSave = new List<Object>();
			itemsToSave.Add(World);
			itemsToSave.Add(currentRoom);
			itemsToSave.Add(Character);
			string path = "./saveGame";
            if (!File.Exists(path))
            {
				using(StreamWriter sw = File.CreateText(path))
                {
					sw.WriteLine(itemsToSave);
					Console.WriteLine("Adventure saved!");
                }
            }
		}
		/////////////////////////////////////////////////////////////////////////

	}
}
