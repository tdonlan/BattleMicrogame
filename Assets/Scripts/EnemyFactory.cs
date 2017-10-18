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
		"hem",
		"gar",
		"chu",
		"stu"
	};

	//TODO: Load this from a csv
	static Dictionary<string,SpriteAssetData> enemySpriteLookup = new Dictionary<string, SpriteAssetData> () {
		{ "Rat",new SpriteAssetData ("Quad", 0) }, 
		{ "Spider",new SpriteAssetData ("Pest", 16) },
		{ "Snake",new SpriteAssetData ("Reptile", 6) }, 
		{ "Imp",new SpriteAssetData ("Demon", 25) }, 
		{ "Kobold",new SpriteAssetData ("Demon", 5) }, 
		{ "Goblin",new SpriteAssetData ("Humanoid", 33) }, 
		{ "Brigand",new SpriteAssetData ("Humanoid", 17) }, 
		{ "Orc",new SpriteAssetData ("Humanoid", 27) }, 
		{ "Zombie",new SpriteAssetData ("Undead", 4) }, 
		{ "Orc Warlord",new SpriteAssetData ("Humanoid", 30) }, 
		{ "Dire Wolf",new SpriteAssetData ("Quad", 7) }, 
		{ "Werewolf",new SpriteAssetData ("Humanoid", 49) }, 
		{ "Gryphon",new SpriteAssetData ("Demon", 2) }, 
		{ "Drake",new SpriteAssetData ("Reptile", 13) }, 
		{ "Wyvern",new SpriteAssetData ("Reptile", 3) },
		{ "Wraith",new SpriteAssetData ("Undead", 36) }, 
		{ "Skeleton Warrior",new SpriteAssetData ("Undead", 16) }, 
		{ "Ogre",new SpriteAssetData ("Humanoid", 9) }, 
		{ "Giant",new SpriteAssetData ("Humanoid", 1) }, 
		{ "Vampire",new SpriteAssetData ("Undead", 26) }, 
		{ "Dragon",new SpriteAssetData ("Reptile", 27) }, 
		{ "Lich",new SpriteAssetData ("Undead", 22) }
	};

	//variance = -1 to 1.  will scale the difficulty of the enemy
	public static Enemy GenerateEnemy (int level, float variance, AssetData assetData)
	{
		level = Mathf.Clamp (Core.vary (level, variance), 1, Enemy.MaxLevel);
	
		string type = getType (level);
		string name = getName (type, variance);
		var e = new Enemy (name, level);
		e.type = type;
		e.TotalHP = Mathf.Clamp (Core.vary (e.TotalHP, variance), Enemy.MinHp, Enemy.MaxHp);
		e.HP = e.TotalHP;
		e.Damage = Mathf.Clamp (Core.vary (e.Damage, variance), Enemy.MinDmg, Enemy.MaxDmg);
		e.Gold = Mathf.Clamp (Core.vary (e.Gold, variance), 0, 99999);
		e.ItemList = ItemFactory.GenerateLoot (level, variance);
		e.XP = Mathf.Clamp (e.XP + Mathf.RoundToInt (e.XP * variance), 10, 9999999);
		e.avatarSprite = getSprite (e.type, assetData);
	
		e.turnDataList = generateTurnDataList (level, variance);
		return e;
	}

	private static string getName (string type, float variance)
	{
		return string.Format ("{0}{1} {2}{3}", getProperName (variance), getNamePrefix (variance), type, getSuffix (variance));
	}

	private static Sprite getSprite (string type, AssetData assetData)
	{
		return assetData.getSprite (enemySpriteLookup [type]);
	}


	private static string getType (int level)
	{
		var lvlRatio = (float)level / (float)Enemy.MaxLevel;
		var index = Mathf.RoundToInt ((float)typeList.Count * lvlRatio);
		index = Mathf.RoundToInt (UnityEngine.Random.Range (index - 5, index + 5));
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

	private static List<TurnData> generateTurnDataList (int level, float variance)
	{
		List<TurnData> turnDataList = new List<TurnData> ();

		var ratio = (float)level / (float)Enemy.MaxLevel;

		var numTurns = Mathf.Clamp (Core.vary (Mathf.RoundToInt (ratio * 4) + 1, variance), 1, 4);
		for (int i = 1; i <= numTurns; i++) {
			turnDataList.Add (new TurnData () {
				duration = UnityEngine.Random.Range (.5f, 2f),
				enemyAttackType = (AttackType)Core.r.Next (3)
			});
		}
		return turnDataList;
	}


}