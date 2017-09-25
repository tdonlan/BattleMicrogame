using System;
using System.Collections.Generic;
using UnityEngine;


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

	public static Item GenerateItem (int level, float variance)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		string name = getItemName (level, variance);
		return new Item (name, null, level);
	}

	public static Weapon GenerateWeapon (int level, float variance)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
		string name = getWeaponName (level, variance);
		int dmg = Mathf.Clamp (Core.vary (level * 10, variance), 1, 999999);
		var w = new Weapon (name, dmg, level); //add additional abilities
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
		string name = getArmorName (level, variance);
		int defense = Mathf.Clamp (Core.vary (level * 5, variance), 1, 999999);
		var a = new Armor (name, defense, level); //add additional abilities
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


