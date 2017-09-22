using System;

using System.Collections.Generic;
using UnityEngine;


public class EnemyFactory
{

	//use variance for scaling
	static List<string> prefixList = new List<string> () {
		"Dying",
		"Cursed",
		"Weakened",
		"Sluggish",
		"Sickly",
		"Ill",
		"Haggard",
		"Obese",
		"Frail",
		"Angry",
		"Elder",
		"Strong",
		"Feirce",
		"Quick",
		"Bold",
		"Enraged",
		"Blessed",
		"Heroic",
		"Elite",
		"Royal",
		"Legendary"
	};

	//random - tie into abilities
	static List<string> suffixList = new List<string> () {
		"the forest",
		"frost",
		"the desert",
		"the ocean",
		"the wasteland",
		"poison",
		"the arcane",
		"the ancients",
		"hell",
		"dungeon",
		"despair",
	};

	static List<string> typeList = new List<string> () {
		"Rat", "Spider", "Snake", "Imp", "Kobold", "Goblin", "Brigand", "Orc", "Zombie", "Orc Warlord", "Dire Wolf", "Werewolf", "Gryphon", "Drake", "Wyvern",
		"Wraith", "Skeleton Warrior", "Ogre", "Giant", "Vampire", "Dragon", "Lich"
	};


	static List<string> namePartList = new List<string> () {
		"ab",
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
		"hem"
	};


	//------- Enemy Factory

	//variance = -1 to 1.  will scale the difficulty of the enemy
	public static Enemy GenerateEnemy (int level, float variance)
	{
		level = Core.vary (level, variance);
		string name = getName (level, variance);
		var e = new Enemy (name, level);
		e.TotalHP = Core.vary (e.TotalHP, variance);
		e.HP = e.TotalHP;
		e.Damage = Core.vary (e.Damage, variance);
		e.XP = e.XP + Mathf.RoundToInt (e.XP * variance);
		return e;
	}

	private static string getName (int level, float variance)
	{
		return string.Format ("{0} {1} {2} {3}", getProperName (variance), getNamePrefix (variance), getType (level), getSuffix (variance));
	
	}

	private static string getType (int level)
	{
		var lvlRatio = (float)level / (float)Enemy.MaxLevel;
		var index = Mathf.RoundToInt ((float)typeList.Count * lvlRatio);
		index = Mathf.RoundToInt (UnityEngine.Random.Range (index - 3, index + 3));
		index = Mathf.Clamp (index, 0, typeList.Count - 1);
		return typeList [index];

	}

	private static string getProperName (float variance)
	{
		if (variance > .75f) {
			string name = "";
			int parts = Mathf.RoundToInt (UnityEngine.Random.Range (1, 4));
			for (int i = 0; i < parts; i++) {
				name += namePartList [Mathf.RoundToInt (UnityEngine.Random.Range (0, namePartList.Count))];
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
			var index = Mathf.RoundToInt ((float)prefixList.Count * (float)ratio);
			index = Mathf.RoundToInt (UnityEngine.Random.Range (index - 3, index + 3));
			index = Mathf.Clamp (index, 0, prefixList.Count - 1);
			return prefixList [index];
		}
		return "";
	}


	private static string getSuffix (float variance)
	{
		//only strong enemies get suffixes
		if (variance > .5) {
			return " of " + suffixList [Mathf.RoundToInt (UnityEngine.Random.Range (0, suffixList.Count - 1))];
		}
		return "";

	}


	private static int vary (int val, float variation, System.Random r)
	{
		var rVal = r.NextDouble ();
		return Mathf.RoundToInt ((float)val + ((float)val * variation * (float)rVal));
	}


}


