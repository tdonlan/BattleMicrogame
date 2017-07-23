using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ITarget
{

	private System.Random r;

	public string Name;
	public int Level;
	public int HP;
	public int TotalHP;

	public List<ItemEffect> effectList = new List<ItemEffect> ();

	public int HPSliderValue {
		get {
			return Mathf.RoundToInt (((float)HP / (float)TotalHP) * 100);
		}
	}

	public int Damage;

	public int turnIndex = 0;
	public List<TurnData> turnDataList;

	public string GetStats ()
	{
		return string.Format ("HP: {0}/{1} Dmg: {2}", HP, TotalHP, Damage);
	}

	public Enemy (string name, int level)
	{

		this.r = new System.Random ();

		this.Name = name;
		this.Level = level;
		this.TotalHP = level * 50;
		this.HP = this.TotalHP;
		this.Damage = level * 10;

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

	public void Cure (int amount)
	{
		while (amount > 0 && effectList.Count > 0) {
			amount--;
			effectList.RemoveAt (0);
		}
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
