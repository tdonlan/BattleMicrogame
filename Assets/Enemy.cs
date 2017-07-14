using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{

	private System.Random r;

	public string Name;
	public int Level;
	public int HP;
	public int TotalHP;

	public int HPSliderValue {
		get {
			return Mathf.RoundToInt (((float)HP / (float)TotalHP) * 100);
		}
	}


	public int Damage;

	public int turnIndex = 0;
	public List<TurnData> turnDataList;

	public Enemy (string name, int level)
	{

		this.r = new System.Random ();

		this.Name = name;
		this.Level = level;
		this.TotalHP = level * 50;
		this.HP = this.TotalHP;
		this.Damage = level * 20;

		this.turnDataList = generateTurnDataList ();

	}

	private List<TurnData> generateTurnDataList ()
	{
		//difficulty of turn data should be a function of enemy level, but for now just hardcode to something easy.
		List<TurnData> turnDataList = new List<TurnData> ();
		turnDataList.Add (new TurnData () {
			duration = 1.5f,
			enemyAttackType = AttackType.Attack
		});

		return turnDataList;
	}

	public bool Hit (int damage)
	{
		this.HP -= damage;
		if (this.HP <= 0) {
			return true;
		}
		return false;
	}

	public TurnData getNextTurnData ()
	{
		turnIndex++;
		if (turnIndex >= turnDataList.Count) {
			turnIndex = 0;
		}
		return turnDataList [turnIndex];
	}
}
