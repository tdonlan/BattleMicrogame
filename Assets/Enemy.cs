using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ITarget
{
	private GameControllerScript gameController;

	private System.Random r;
	public const int MaxLevel = 50;
	public const int MaxHp = 9999;
	public const int MinHp = 5;
	public const int MinDmg = 1;
	public const int MaxDmg = 9999;

	public const int BaseXP = 200;
	public const int BaseHP = 50;
	public const int BaseDamage = 20;
	public const int BaseGold = 10;

	public string Name;
	public int Level;
	public int HP;
	public int TotalHP;
	public int XP;

	public int Gold;
	public List<Item> ItemList = new List<Item> ();

	public List<ItemEffect> effectList = new List<ItemEffect> ();

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
		this.TotalHP = level * Enemy.BaseHP;
		this.HP = this.TotalHP;
		this.Damage = level * Enemy.BaseDamage;
		this.XP = level * Enemy.BaseXP;
		this.Gold = level * Enemy.BaseGold;
	}

	//For display stats in game
	public string GetStats ()
	{
		return string.Format ("Level: {3} \nHP: {0}/{1} \nDmg: {2}", HP, TotalHP, Damage, Level);
	}

	//for displaying full object info for testing
	public override string ToString ()
	{
		var turnsStr = "";
		foreach (var t in turnDataList) {
			turnsStr += t.ToString () + ", ";
		}
		var itemStr = "";
		foreach (var i in ItemList) {
			itemStr += i.Name + ", ";
		}
		return string.Format ("{0}\n Level {1}\n HP {2}/{3} Dmg: {5} \nXP: {4} Gold: {7}\n Items:{8}\n \nTurns: {6}", Name, Level, HP, TotalHP, XP, Damage, turnsStr, Gold, itemStr);
	}

	public void AttachGameController (GameControllerScript gameController)
	{
		this.gameController = gameController;
	}

	public void AddEffect (ItemEffect effect)
	{
		var newEffect = effect.GetCopy ();
		newEffect.ApplyEffect (this);
		if (newEffect.Duration > 0) {
			this.effectList.Add (newEffect);
		}
	}

	public void RemoveEffect (ItemEffect effect)
	{
		this.effectList.Remove (effect);	
	}

	public bool Hit (int damage)
	{
		gameController.DisplayDmg (true, damage);
		this.HP -= damage;
		if (this.HP <= 0) {
			return true;
		}
		return false;
	}

	public void Heal (int amt)
	{
		gameController.DisplayDmg (true, amt * -1);

		this.HP += amt;
		if (this.HP > this.TotalHP) {

			this.HP = this.TotalHP;
		}
	}

	public void Cure (int amount)
	{
		while (amount > 0 && effectList.Count > 0) {
			amount--;
			effectList.RemoveAt (0);
		}
	}

	public void BuffDamage (int amount)
	{
		//todo
	}

	public void BuffDefense (int amount)
	{
		//todo
	}

	public TurnData getNextTurnData ()
	{
		turnIndex++;
		if (turnIndex >= turnDataList.Count) {
			turnIndex = 0;
		}
		return turnDataList [turnIndex];
	}

	//helper for generating a decent timer duration range
	private float getNewTimerValue ()
	{
		return .5f + ((float)r.NextDouble () * 2f);
	}

	public void UpdateEffects ()
	{
		for (int i = effectList.Count - 1; i >= 0; i--) {
			effectList [i].ApplyEffect (this);
		}
	}
}
