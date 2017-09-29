using System;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffectFactory
{
	public static List<ItemEffect> getItemEffectList (int level, float variance)
	{
		List<ItemEffect> effectList = new List<ItemEffect> ();

		var ratio = (float)level / (float)Enemy.MaxLevel;
		var numEffects = Mathf.Clamp (Core.vary (Mathf.RoundToInt (ratio * 4) + 1, variance), 1, 4);
		for (int i = 0; i < numEffects; i++) {
			var effectIndex = UnityEngine.Random.Range (0, Enum.GetNames (typeof(EffectType)).Length);
			var effectType = (EffectType)effectIndex;

			effectList.Add (getItemEffect (level, variance, effectType));
		}
		return effectList;
	}

	//should pass in the effect type based on name...
	public static ItemEffect getItemEffect (int level, float variance)
	{
		var totalAmt = Mathf.RoundToInt (Core.vary (level * 25, variance));

		var turns = UnityEngine.Random.Range (1, 5);
		var amt = Mathf.RoundToInt ((float)totalAmt / (float)turns);

		var effectIndex = UnityEngine.Random.Range (0, Enum.GetNames (typeof(EffectType)).Length);
		var effectType = (EffectType)effectIndex;
		return new ItemEffect (effectType, amt, turns);
	}

	public static ItemEffect getItemEffect (int level, float variance, EffectType type)
	{
		var baseVal = 10;
		switch (type) {
		case EffectType.BuffDefense:
			baseVal = 5;
			break;
		case EffectType.BuffDamage:
			baseVal = 10;
			break;
		case EffectType.CureSelf:
			baseVal = 1;
			break;
		case EffectType.DamageEnemy:
			baseVal = 10;
			break;
		case EffectType.HealSelf:
			baseVal = 25;
			break;
		default:
			break;
		}
		var totalAmt = Mathf.RoundToInt (Core.vary (level * baseVal, variance));

		var turns = UnityEngine.Random.Range (1, 5);
		var amt = Mathf.Clamp (Mathf.RoundToInt ((float)totalAmt / (float)turns), 5, 9999);

		return new ItemEffect (type, amt, turns);
	}
}

public class ItemFactory
{
	public static List<string> itemTypeList = new List<string> () {
		"potion", "elixir", "ointment", "bandage", "gemstone", "cocktail", "lollipop", "tincture", "flask", "dose", "syringe", "infusion", "powder", "dust"
	};

	public static List<string> weaponTypeList = new List<string> () {
		"sword", "dagger", "knife", "scimitar", "hatchet", "axe", "blade", 
		"stilletto", "greatsword", "pike", "spear", "trident", "crossbow", "bow", "battleaxe", 
		"quarterstaff", "staff", "mace", "flail", "morning star", "ball and chain", "katana"
	};

	public static List<string> armorTypeList = new List<string> () {
		"leather",
		"plate",
		"studded leather",
		"jerkin",
		"coat",
		"cloak",
		"tunic",
		"hauberk",
		"chain mail",
		"scale mail",
		"hide"
	};

	public static List<string> itemPrefixList = new List<string> () {
		"destroyed",
		"shoddy",
		"broken",
		"rusted",
		"frail",
		"poor",
		"solid",
		"common",
		"excellent",
		"elite",
		"enchanted",
		"magical",
		"legendary"
	};

	public static List<string> itemSuffixList = new List<string> () {
		"the Forest",
		"Frost",
		"the Desert",
		"the Ocean",
		"the Wasteland",
		"Poison",
		"the Arcane",
		"the Ancients",
		"Hell",
		"Dungeon",
		"Despair",
	};

	public static List<string> itemNamePartList = new List<string> () {"ab",
		"hal",
		"ren",
		"dar",
		"tor",
		"cam",
		"see",
		"tov",
		"fel",
		"pes",
		"bun",
		"mov",
		"lop",
		"tar",
		"hem",
		"gar",
		"chu",
		"stu"
	};

	//------ Factories
	//put these in a factory / datalist?
	public static Item getHealingPotion ()
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 25, 1);
		return new Item ("Healing Potion", 1, 5, new List<ItemEffect> (){ iEffect });
	}

	public static Item getRegenPotion ()
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 5, 5);
		return new Item ("Regen Potion", 1, 5, new List<ItemEffect> (){ iEffect });
	}

	public static Item getGrenade ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 25, 1);
		return new Item ("Grenade", 1, 5, new List<ItemEffect> (){ iEffect });
	}

	public static Item getPoison ()
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 5, 5);
		return new Item ("Poison", 1, 5, new List<ItemEffect> (){ iEffect });
	}

	//-----------------

	public static Item GenerateItem (int level, float variance)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 10, variance));
		string name = getItemName (level, variance);
		var effectList = ItemEffectFactory.getItemEffectList (level, variance);
		return new Item (name, level, price, effectList);
	}

	public static Weapon GenerateWeapon (int level, float variance)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 50, variance));
		string name = getWeaponName (level, variance);
		int dmg = Mathf.Clamp (Core.vary (level * 10, variance), 1, 999999);
		var w = new Weapon (name, level, price, dmg); //TODO: add additional abilities
		return w;

	}

	private static string getWeaponName (int level, float variance)
	{
		string name = string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), getWeaponType (level), getSuffix (variance)).Trim ();
		return name [0].ToString ().ToUpper () + name.Substring (1);
	}


	private static string getWeaponType (int level)
	{
		return weaponTypeList [UnityEngine.Random.Range (0, weaponTypeList.Count)];
	}

	public static Armor GenerateArmor (int level, float variance)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 25, variance));
		string name = getArmorName (level, variance);
		int defense = Mathf.Clamp (Core.vary (level * 5, variance), 1, 999999);
		var a = new Armor (name, level, price, defense); //TODO: leveladd additional abilities
		return a;
	}

	private static string getArmorName (int level, float variance)
	{
		string name = string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), getArmorType (level), getSuffix (variance)).Trim ();
		return name [0].ToString ().ToUpper () + name.Substring (1);
	}


	private static string getArmorType (int level)
	{
		return armorTypeList [UnityEngine.Random.Range (0, armorTypeList.Count)];
	}

	public static string getItemName (int level, float variance)
	{
		string name = string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), getItemType (level), getSuffix (variance)).Trim ();
		return name [0].ToString ().ToUpper () + name.Substring (1);
	}

	private static string getItemType (int level)
	{
		return itemTypeList [UnityEngine.Random.Range (0, itemTypeList.Count)];
	}

	private static string getProperName (float variance)
	{
		if (variance > .75f) {
			string name = "";
			int parts = Mathf.RoundToInt (UnityEngine.Random.Range (1, 4));
			for (int i = 0; i < parts; i++) {
				name += itemNamePartList [Mathf.RoundToInt (UnityEngine.Random.Range (0, itemNamePartList.Count))];
			}

			return name [0].ToString ().ToUpper () + name.Substring (1) + " the ";
		}
		return "";
	}

	private static string getNamePrefix (float variance)
	{
		//only outliers get prefixes
		if (Math.Abs (variance) > .5) {
			var ratio = ((variance + 1) * .5);
			var index = Mathf.RoundToInt ((float)itemPrefixList.Count * (float)ratio);
			index = Mathf.RoundToInt (UnityEngine.Random.Range (index - 3, index + 3));
			index = Mathf.Clamp (index, 0, itemPrefixList.Count - 1);
			return itemPrefixList [index];
		}
		return "";
	}

	private static string getSuffix (float variance)
	{
		//only strong enemies get suffixes
		if (variance > .5) {
			return " of " + itemSuffixList [Mathf.RoundToInt (UnityEngine.Random.Range (0, itemSuffixList.Count - 1))];
		}
		return "";

	}
		
}