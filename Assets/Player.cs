using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	public string Name;
	public int Level;
	public int TotalHP;
	public int HP;

	public int Damage;

	public Player ()
	{
		this.Name = "Player";
		this.TotalHP = 100;
		this.HP = this.TotalHP;
		this.Level = 1;
		this.Damage = 25;
	}

	public bool Hit (int damage)
	{
		this.HP -= damage;
		if (this.HP <= 0) {
			return true;
		}
		return false;
	}

	public void Heal (int amt)
	{
		this.HP += amt;
		if (this.HP > this.TotalHP) {

			this.HP = this.TotalHP;
		}
	}
}
