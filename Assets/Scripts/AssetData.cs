using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SpriteAssetData
{
	public string sheetName;
	public int index;

	public SpriteAssetData (string sheetName, int index)
	{
		this.sheetName = sheetName;
		this.index = index;
	}
}

public class AssetData
{
	//Avatars
	public List<Sprite> PlayerList;
	public List<Sprite> DemonList;
	public List<Sprite> ElementalList;
	public List<Sprite> HumanoidList;
	public List<Sprite> ReptileList;
	public List<Sprite> UndeadList;
	public List<Sprite> MiscList;
	public List<Sprite> PestList;
	public List<Sprite> QuadList;

	//Items
	public List<Sprite> ArmorList;
	public List<Sprite> ChestList;
	public List<Sprite> LongWepList;
	public List<Sprite> MedWepList;
	public List<Sprite> PotionList;
	public List<Sprite> ShortWepList;

	public AssetData ()
	{
		LoadSprites ();

	}

	private void LoadSprites ()
	{
		PlayerList = LoadSpriteResource ("Player1");
		DemonList = LoadSpriteResource ("Demon1");
		ElementalList = LoadSpriteResource ("Elemental1");
		HumanoidList = LoadSpriteResource ("Humanoid1");
		ReptileList = LoadSpriteResource ("Reptile1");
		UndeadList = LoadSpriteResource ("Undead1");
		MiscList = LoadSpriteResource ("Misc1");
		PestList = LoadSpriteResource ("Pest1");
		QuadList = LoadSpriteResource ("Quadraped1");

		ArmorList = LoadSpriteResource ("Armor");
		ChestList = LoadSpriteResource ("Chest1");
		LongWepList = LoadSpriteResource ("LongWep");
		MedWepList = LoadSpriteResource ("MedWep");
		PotionList = LoadSpriteResource ("Potion");
		ShortWepList = LoadSpriteResource ("ShortWep");
	}

	private List<Sprite> LoadSpriteResource (string name)
	{
		return Resources.LoadAll (name, typeof(Sprite)).Cast<Sprite> ().ToList ();
	}

	public Sprite getSprite (SpriteAssetData assetData)
	{

		List<Sprite> spriteList;

		switch (assetData.sheetName) {
		case "Player":
			spriteList = PlayerList;
			break;
		case "Demon":
			spriteList = DemonList;
			break;
		case "Elemental":
			spriteList = DemonList;
			break;
		case "Humanoid":
			spriteList = DemonList;
			break;
		case "Reptile":
			spriteList = DemonList;
			break;
		case "Undead":
			spriteList = DemonList;
			break;
		case "Misc":
			spriteList = MiscList;
			break;
		case "Pest":
			spriteList = PestList;
			break;
		case "Quad":
			spriteList = QuadList;
			break;

		case "Armor":
			spriteList = ArmorList;
			break;
		case "Chest":
			spriteList = ChestList;
			break;
		case "LongWep":
			spriteList = LongWepList;
			break;
		case "MedWep":
			spriteList = MedWepList;
			break;
		case "ShortWep":
			spriteList = ShortWepList;
			break;
		case "Potion":
			spriteList = PotionList;
			break;
		default:
			spriteList = DemonList;
			break;
		}

		if (assetData.index < spriteList.Count) {
			return spriteList [assetData.index];
		}
		return null;
	}
}