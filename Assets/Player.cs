using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ITarget
{
	private GameControllerScript gameController;

	public string Name;
	public int Level;
	public int TotalHP;
	public int HP;

	public List<Item> itemList;

	public List<ItemEffect> effectList = new List<ItemEffect> ();

	public int HPSliderValue {
		get {
			return Mathf.RoundToInt (((float)HP / (float)TotalHP) * 100);
		}
	}

	public int Damage;

	public string GetStats ()
	{
		return string.Format ("HP: {0}/{1} Dmg: {2}", HP, TotalHP, Damage);
	}

	public Player ()
	{
		this.Name = "Player";
		this.TotalHP = 100;
		this.HP = this.TotalHP;
		this.Level = 1;
		this.Damage = 20;
		itemList = new List<Item> ();
		itemList.Add (Item.getHealingPotion ());
	
		itemList.Add (Item.getRegenPotion ());
		itemList.Add (Item.getGrenade ());
		itemList.Add (Item.getPoison ());
	
	}

	public void AttachGameController (GameControllerScript gameController)
	{
		this.gameController = gameController;
	}

	public override string ToString ()
	{
		return string.Format ("{0}\nLevel:{1}\nHP:{2}/{3}", this.Name, this.Level, this.HP, this.TotalHP);
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
		gameController.DisplayDmg (false, damage);

		this.HP -= damage;
		if (this.HP <= 0) {
			return true;
		}
		return false;
	}

	public void Heal (int amt)
	{
		gameController.DisplayDmg (false, amt * -1);

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

	public void UpdateEffects ()
	{
		for (int i = effectList.Count - 1; i >= 0; i--) {
			effectList [i].ApplyEffect (this);
		}
	}
}
