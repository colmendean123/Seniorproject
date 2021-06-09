using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WorldCharacter : MonoBehaviour
{
	public string name;
	public int maxHitPoints;
	public int currentHitPoints;
	public int attackSpeed;
	public double chanceToHit;
	public int damageMin, damageMax;
	public int numTurns;
	//public Weapons weapon;

	public string getName()
	{
		return name;
	}

	public void setName(string name)
	{
		this.name = name;
	}

	public int getHitPoints()
	{
		return currentHitPoints;
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

	//public Weapons getWeapon()
	//{
	//	return this.weapon;
	//}

	//seperate method to handle adding hit points to the character that needs hitpoints added.
	public string addHitPoints(int hitPoints)
    {
        string res = "";

        if (hitPoints <= 0)
            res = ("Hitpoint amount must be positive.");
        else if (hitPoints + this.currentHitPoints >= maxHitPoints)
        {
            this.currentHitPoints = maxHitPoints;
            res = ("You are back to full HP at: " + hitPoints);
        }
        else
        {
            this.currentHitPoints += hitPoints;
            res = ("Remaining Hit Points: " + hitPoints);

        }

        return res;
    }

    //seperate method to handle subtracting hit points to the character that needs hitpoints taken away.
    public string subtractHitPoints(int hitPoints)
    {
        string res = "";

        if (hitPoints < 0)
            res = ("Hitpoint amount must be positive.");
        else if (hitPoints > 0)
        {
            this.currentHitPoints -= hitPoints;
            if (this.currentHitPoints < 0)
                this.currentHitPoints = 0;
            res += "\n" + (getName() + " takes <" + hitPoints + "> points of damage.");
            res += "\n" + (getName() + " now has " + getHitPoints() + " hit points remaining.");
        }

        if (this.currentHitPoints == 0)
            res += "\n" + (getName() + " has been killed :-(");

        return res;

    }

    public void setCharacter(WorldCharacter character)
	{
		this.name = character.name;
		this.currentHitPoints = character.currentHitPoints;
		this.maxHitPoints = character.maxHitPoints;
		this.attackSpeed = character.attackSpeed;
		this.chanceToHit = character.chanceToHit;
		this.damageMin = character.damageMin;
		this.damageMax = character.damageMax;
		//this.weapon = character.weapon;
	}

	public bool isAlive()
	{
		return (currentHitPoints > 0);
	}

	public void setTurns(int turns)
	{
		this.numTurns = turns;
	}

}