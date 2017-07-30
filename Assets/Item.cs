using UnityEngine;
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
	public List<ItemEffect> itemEffectList;

	public Item (string name, ItemEffect itemEffect)
	{
		this.Name = name;
		this.itemEffectList = new List<ItemEffect> (){ itemEffect };
	}

	public Item (string name, List<ItemEffect> itemEffectList)
	{
		this.Name = name;
		this.itemEffectList = itemEffectList;
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
		return new Item ("Healing Potion", iEffect);
	}

	public static Item getRegenPotion ()
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 5, 5);
		return new Item ("Regen Potion", iEffect);
	}
		
	//lifetap potion?

	public static Item getGrenade ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 25, 1);
		return new Item ("Grenade", iEffect);
	}

	public static Item getPoison ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 5, 5);
		return new Item ("Poison", iEffect);
	}




}

