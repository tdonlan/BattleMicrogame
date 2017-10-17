using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AssetData
{
	public List<Sprite> PlayerList;
	public List<Sprite> DemonList;
	public List<Sprite> ElementalList;
	public List<Sprite> HumanoidList;
	public List<Sprite> ReptileList;
	public List<Sprite> UndeadList;

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

	}

	private List<Sprite> LoadSpriteResource (string name)
	{
		return Resources.LoadAll (name, typeof(Sprite)).Cast<Sprite> ().ToList ();
	}
}