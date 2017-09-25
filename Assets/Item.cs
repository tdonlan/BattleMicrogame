﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
	HealSelf,
	CureSelf,
	DamageEnemy,
}

public class ItemEffect
{
	//TODO: Need to include the target as a flag, instead of baked into effectType
	public EffectType effectType;
	public int Amount;
	public int Duration;

	public ItemEffect (EffectType effectType, int amount, int duration)
	{
		this.effectType = effectType;
		this.Amount = amount;
		this.Duration = duration;
	}

	public override string ToString ()
	{
		return string.Format ("{0}:{1}/{2}trns", effectType.ToString (), Amount, Duration);
	}

	//clone the effect
	public ItemEffect GetCopy ()
	{
		return new ItemEffect (this.effectType, this.Amount, this.Duration);
	}

	public void ApplyEffect (ITarget target)
	{
		if (this.Duration > 0) {
			switch (effectType) {
			case EffectType.HealSelf:
				target.Heal (Amount);
				break;
			case EffectType.CureSelf:
				target.Cure (Amount);
				break;
			case EffectType.DamageEnemy:
				target.Hit (Amount);
				break;
			default:
				break;
			}
		}

		this.Duration--;
		if (this.Duration <= 0) {
			target.RemoveEffect (this);
		}
	}
}

public class Item
{
	public string Name;
	public int Price;
	public int Level;
	public List<ItemEffect> itemEffectList = new List<ItemEffect> ();

	public Item (string name, ItemEffect itemEffect, int level)
	{
		this.Name = name;
		this.Level = level;
		if (itemEffect != null) {
			this.itemEffectList = new List<ItemEffect> (){ itemEffect };
		}

	}

	/*
	public Item (string name, List<ItemEffect> itemEffectList)
	{
		this.Name = name;
		this.itemEffectList = itemEffectList;
	}
	*/

	public override string ToString ()
	{
		var effects = "";
		if (itemEffectList.Count > 0) {
			foreach (var i in itemEffectList) {
				effects += i.ToString () + ", ";
			}
		}
		return string.Format ("Lvl:{0}\n{1}", this.Level, effects);
	}

	//need more generic way to apply effects to targets
	public void UseItem (ITarget source, ITarget target)
	{
		foreach (var effect in itemEffectList) {
			switch (effect.effectType) {
			case EffectType.HealSelf:
				source.AddEffect (effect);
				break;
			case EffectType.CureSelf:
				source.AddEffect (effect);
				break;
			case EffectType.DamageEnemy:
				target.AddEffect (effect);
				break;
			default:
				break;
			}
		}
	}
		
	//------------- FACTORIES -----------


	//put these in a factory / datalist?
	public static Item getHealingPotion ()
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 25, 1);
		return new Item ("Healing Potion", iEffect, 1);
	}

	public static Item getRegenPotion ()
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 5, 5);
		return new Item ("Regen Potion", iEffect, 1);
	}
		
	//lifetap potion?

	public static Item getGrenade ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 25, 1);
		return new Item ("Grenade", iEffect, 1);
	}

	public static Item getPoison ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 5, 5);
		return new Item ("Poison", iEffect, 1);
	}
		
}

public class Weapon : Item
{
	public int Damage;

	public Weapon (string name, int Damage, int level) : base (name, null, level)
	{
		this.Damage = Damage;
	}

	public override string ToString ()
	{
		return base.ToString () + "Dmg: " + Damage;
	}
}

public class Armor : Item
{
	public int Defense;

	public Armor (string name, int Defense, int level) : base (name, null, level)
	{
		this.Defense = Defense;
	}

	public override string ToString ()
	{
		return  base.ToString () + "Defense: " + Defense;
	}

}