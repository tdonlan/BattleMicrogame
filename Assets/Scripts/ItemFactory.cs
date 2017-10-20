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
		"stilletto", "greatsword", "pike", "spear", "trident", "battleaxe", 
		"quarterstaff", "staff", "mace", "flail", "morning star", "ball and chain", "katana"
	};

	static Dictionary<string,SpriteAssetData> weaponSpriteLookup = new Dictionary<string, SpriteAssetData> () {
		{ "sword",new SpriteAssetData ("LongWep", 7) }, 
		{ "dagger",new SpriteAssetData ("ShortWep", 0) }, 
		{ "knife",new SpriteAssetData ("ShortWep", 1) }, 
		{ "scimitar",new SpriteAssetData ("MedWep", 5) }, 
		{ "hatchet",new SpriteAssetData ("MedWep", 6) }, 
		{ "axe",new SpriteAssetData ("LongWep", 23) }, 
		{ "blade",new SpriteAssetData ("MedWep", 0) }, 
		{ "stilletto",new SpriteAssetData ("ShortWep", 7) }, 
		{ "greatsword",new SpriteAssetData ("LongWep", 8) }, 
		{ "pike",new SpriteAssetData ("LongWep", 17) }, 
		{ "spear",new SpriteAssetData ("LongWep", 0) }, 
		{ "trident",new SpriteAssetData ("LongWep", 15) }, 
		{ "battleaxe",new SpriteAssetData ("MedWep", 7) }, 
		{ "quarterstaff",new SpriteAssetData ("LongWep", 25) }, 
		{ "staff",new SpriteAssetData ("LongWep", 5) },
		{ "mace",new SpriteAssetData ("ShortWep", 18) }, 
		{ "flail",new SpriteAssetData ("ShortWep", 19) }, 
		{ "morning star",new SpriteAssetData ("ShortWep", 18) }, 
		{ "ball and chain",new SpriteAssetData ("ShortWep", 19) }, 
		{ "katana",new SpriteAssetData ("LongWep", 11) }
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

	static Dictionary<string,SpriteAssetData> armorSpriteLookup = new Dictionary<string, SpriteAssetData> () {
		{ "leather",new SpriteAssetData ("Armor", 44) }, 
		{ "plate",new SpriteAssetData ("Armor", 42) }, 
		{ "studded leather",new SpriteAssetData ("Armor", 55) }, 
		{ "jerkin",new SpriteAssetData ("Armor", 45) }, 
		{ "coat",new SpriteAssetData ("Armor", 35) }, 
		{ "cloak",new SpriteAssetData ("Armor", 51) }, 
		{ "tunic",new SpriteAssetData ("Armor", 24) }, 
		{ "hauberk",new SpriteAssetData ("Armor", 8) }, 
		{ "chain mail",new SpriteAssetData ("Armor", 5) }, 
		{ "scale mail",new SpriteAssetData ("Armor", 39) }, 
		{ "hide",new SpriteAssetData ("Armor", 58) }, 
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
	public static Item getHealingPotion (AssetData assetData)
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 25, 1);
		var i = new Item ("Healing Potion", 1, 5, new List<ItemEffect> (){ iEffect });
		i.itemSprite = getItemSprite (assetData);
		return i;
	}

	public static Item getRegenPotion (AssetData assetData)
	{
		var iEffect = new ItemEffect (EffectType.HealSelf, 5, 5);
		var i = new Item ("Regen Potion", 1, 5, new List<ItemEffect> (){ iEffect });
		i.itemSprite = getItemSprite (assetData);
		return i;
	}

	public static Item getGrenade (AssetData assetData)
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 25, 1);
		var i = new Item ("Grenade", 1, 5, new List<ItemEffect> (){ iEffect });
		i.itemSprite = getItemSprite (assetData);
		return i;
	}

	public static Item getPoison (AssetData assetData)
	{
		var iEffect = new ItemEffect (EffectType.DamageEnemy, 5, 5);
		var i = new Item ("Poison", 1, 5, new List<ItemEffect> (){ iEffect });
		i.itemSprite = getItemSprite (assetData);
		return i;
	}

	//-----------------

	public static List<Item> GenerateLoot (int level, float variance, AssetData assetData)
	{
		List<Item> lootList = new List<Item> ();
		var ratio = (float)level / (float)Enemy.MaxLevel;
		var numLoot = Mathf.Clamp (Core.vary (Mathf.RoundToInt (ratio) + 1, variance), 0, 5);

		for (int i = 0; i < numLoot; i++) {

			var itemVariance = UnityEngine.Random.Range (variance / 4, variance);

			var lootType = UnityEngine.Random.Range (0, 1f);
			if (lootType > .666f) {
				lootList.Add (ItemFactory.GenerateWeapon (level, itemVariance, assetData));
			} else if (lootType > .333f) {
				lootList.Add (ItemFactory.GenerateArmor (level, itemVariance, assetData));
			} else {
				lootList.Add (ItemFactory.GenerateItem (level, itemVariance, assetData));
			}

		}

		return lootList;

	}

	public static List<Item> GenerateStore (int level, float variance, AssetData assetData)
	{
		List<Item> lootList = new List<Item> ();

		var numLoot = Mathf.Clamp (Core.vary (10, variance), 1, 99);

		for (int i = 0; i < numLoot; i++) {

			var itemVariance = UnityEngine.Random.Range (variance / 4, variance);

			var lootType = UnityEngine.Random.Range (0, 1f);
			if (lootType > .666f) {
				lootList.Add (ItemFactory.GenerateWeapon (level, itemVariance, assetData));
			} else if (lootType > .333f) {
				lootList.Add (ItemFactory.GenerateArmor (level, itemVariance, assetData));
			} else {
				lootList.Add (ItemFactory.GenerateItem (level, itemVariance, assetData));
			}

		}

		return lootList;
	}



	public static Weapon GenerateWeapon (int level, float variance, AssetData assetData)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 50, variance));
		string type = getWeaponType (level);
		string name = getWeaponName (type, variance);
		int dmg = Mathf.Clamp (Core.vary (level * 10, variance), 1, 999999);
		var w = new Weapon (name, level, price, dmg); //TODO: add additional abilities
		w.Type = type;
		w.itemSprite = getWeaponSprite (type, assetData);
		return w;
	}

	private static string getWeaponName (string type, float variance)
	{
		string name = string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), type, getSuffix (variance)).Trim ();
		return name [0].ToString ().ToUpper () + name.Substring (1);
	}

	private static Sprite getWeaponSprite (string type, AssetData assetData)
	{
		return assetData.getSprite (weaponSpriteLookup [type]);
	}


	private static string getWeaponType (int level)
	{
		return weaponTypeList [UnityEngine.Random.Range (0, weaponTypeList.Count)];
	}

	public static Armor GenerateArmor (int level, float variance, AssetData assetData)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 25, variance));
		string type = getArmorType (level);
		string name = getArmorName (type, variance);
		int defense = Mathf.Clamp (Core.vary (level * 5, variance), 1, 999999);
		var a = new Armor (name, level, price, defense); //TODO: leveladd additional abilities
		a.Type = type;
		a.itemSprite = getArmorSprite (type, assetData);
		return a;
	}

	private static string getArmorName (string type, float variance)
	{
		string name = string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), type, getSuffix (variance)).Trim ();
		return name [0].ToString ().ToUpper () + name.Substring (1);
	}

	private static Sprite getArmorSprite (string type, AssetData assetData)
	{
		return assetData.getSprite (armorSpriteLookup [type]);
	}

	private static string getArmorType (int level)
	{
		return armorTypeList [UnityEngine.Random.Range (0, armorTypeList.Count)];
	}


	public static Item GenerateItem (int level, float variance, AssetData assetData)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		var price = Mathf.RoundToInt (Core.vary (level * 10, variance));
		string name = getItemName (level, variance);
		var effectList = ItemEffectFactory.getItemEffectList (level, variance);
		var i = new Item (name, level, price, effectList);
		i.itemSprite = getItemSprite (assetData);
		return i;
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

	//just return a random potion
	private static Sprite getItemSprite (AssetData assetData)
	{
		return assetData.PotionList [UnityEngine.Random.Range (0, assetData.PotionList.Count - 1)];
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