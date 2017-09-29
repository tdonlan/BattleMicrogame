using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
	BuffDamage,
	BuffDefense,
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
			case EffectType.BuffDamage:
				target.BuffDamage (Amount);
				break;
			case EffectType.BuffDefense:
				target.BuffDefense (Amount);
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


	public Item (string name, int level, int price, List<ItemEffect> itemEffectList)
	{
		this.Name = name;
		this.Level = level;
		this.Price = price;
		this.itemEffectList = itemEffectList;
	}

	public override string ToString ()
	{
		var effects = "";
		if (itemEffectList != null && itemEffectList.Count > 0) {
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
}

public class Weapon : Item
{
	public int Damage;

	public Weapon (string name, int level, int price, int Damage) : base (name, level, price, null)
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

	public Armor (string name, int level, int price, int Defense) : base (name, level, price, null)
	{
		this.Defense = Defense;
	}

	public override string ToString ()
	{
		return  base.ToString () + "Defense: " + Defense;
	}

}