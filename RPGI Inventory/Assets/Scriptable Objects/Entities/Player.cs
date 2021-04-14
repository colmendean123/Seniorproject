using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WorldCharacter
{
    public InventoryObject inventory;
	private double chanceToBlock;
	private ArrayList items;
	//private Abilities specialAttack;
	private int posX;
	private int posY;

	public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.addItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

	public double getChanceToBlock()
	{
		return this.chanceToBlock;
	}

	public bool defend()
	{
		return Random.Range(0.0f, 1.0f) <= chanceToBlock;
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

    public string toString()
    {
        return "Name: " + this.getName() + "\n" + "Hit Points: " + this.getHitPoints() + "\n";
    }

 //   public new string subtractHitPoints(int hitPoints)
	//{
	//	string res = "";

	//	if (defend())
	//	{
	//		res = (this.getName() + " BLOCKED the attack!");
	//	}
	//	else
	//	{
	//		base.subtractHitPoints(hitPoints);
	//	}

	//	return res;
	//}

	//public void takeDamage(int hitPoints)
	//{
	//	base.subtractHitPoints(hitPoints);
	//}

}
