using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public class AttackPool
    {
		private static AttackPool attackPool;
		private BasicAttack attack;
		private BasicHeal heal;
		private Dictionary<Abilities, SpecialAbility> specialAbility;
		private Dictionary<Weapons, Weapon> weapons;
		private Dictionary<Items, Item> items;

		private AttackPool()
		{
			this.attack = new BasicAttack();
			this.heal = new BasicHeal();
			this.specialAbility = generateSpecialAbilities();
			this.weapons = generateWeapons();
			this.items = generateItems();
		}

		private Dictionary<Abilities, SpecialAbility> generateSpecialAbilities()
		{
			Dictionary<Abilities, SpecialAbility> specialAbility = new Dictionary<Abilities, SpecialAbility>();
			specialAbility.Add(Abilities.SneakAttack, new SneakAttack());
			specialAbility.Add(Abilities.CrushingBlow, new CrushingBlow());
			specialAbility.Add(Abilities.HeroHeal, new HeroHeal());
			specialAbility.Add(Abilities.TwinStrike, new TwinStrike());
			specialAbility.Add(Abilities.Smite, new Smite());
			return specialAbility;
		}

		private Dictionary<Weapons, Weapon> generateWeapons()
		{
			Dictionary<Weapons, Weapon> weapons = new Dictionary<Weapons, Weapon>();
			weapons.Add(Weapons.Club, new Club());
			weapons.Add(Weapons.Dagger, new Dagger());
			weapons.Add(Weapons.FireStaff, new FireStaff());
			weapons.Add(Weapons.Flail, new Flail());
			weapons.Add(Weapons.GreatAxe, new GreatAxe());
			weapons.Add(Weapons.GreatSword, new GreatSword());
			weapons.Add(Weapons.Kris, new Kris());
			weapons.Add(Weapons.Rapier, new Rapier());
			weapons.Add(Weapons.RustyBlade, new RustyBlade());
			weapons.Add(Weapons.ShortBow, new ShortBow());
			weapons.Add(Weapons.Trident, new Trident());
			return weapons;
		}

		private Dictionary<Items, Item> generateItems()
		{
			Dictionary<Items, Item> items = new Dictionary<Items, Item>();
			items.Add(Items.Entrance, new Entrance());
			items.Add(Items.Exit, new Exit());
			items.Add(Items.HealingPotion, new HealPot());
			items.Add(Items.VisionPotion, new VisionPot());
			items.Add(Items.PillarOfAbstraction, new PillarOfAbstraction());
			items.Add(Items.PillarOfEncapsulation, new PillarOfEncapsulation());
			items.Add(Items.PillarOfInheritance, new PillarOfInheritance());
			items.Add(Items.PillarOfPolymorphism, new PillarOfPolymorphism());
			items.Add(Items.Pit, new Pit());
			return items;
		}

		public static AttackPool getInstanceOf()
		{
			if (attackPool == null)
			{
				return attackPool = new AttackPool();
			}
			return attackPool;
		}

		public SpecialAbility getSpecialAbility(Abilities ability)
		{
			if (ability == null)
			{
				throw new ArgumentException("Abilities type was null");
			}
			return this.specialAbility[ability];
		}

		public Weapon getWeapon(Weapons weapon)
		{
			if (weapon == null)
			{
				throw new ArgumentException("Weapons type was null");
			}
			return this.weapons[weapon];
		}

		public Item getItem(Items item)
		{
			if (item == null)
			{
				throw new ArgumentException("Items type was null");
			}
			return items[item];
		}

		public BasicAttack getBasicAttack()
		{
			return this.attack;
		}

		public BasicHeal getBasicHeal()
		{
			return this.heal;
		}

	}
}
