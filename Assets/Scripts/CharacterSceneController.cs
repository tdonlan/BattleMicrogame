using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CharacterSceneController : MonoBehaviour
{

	public GameData gameData;
	public Text StatsText;

	public Image playerImg;

	private int playerSpriteIndex = 0;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		SetStatsText ();
		SetPlayerImg ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void SetPlayerImg ()
	{
		if (gameData.player.avatarSprite == null) {
			var spriteAssetData = new SpriteAssetData ("Player", playerSpriteIndex);
			gameData.player.spriteAssetData = spriteAssetData;
			gameData.player.avatarSprite = playerImg.sprite;
		}
		playerImg.sprite = gameData.player.avatarSprite;
	}

	private void SetStatsText ()
	{
		StatsText.text = string.Format ("{0}\n\nKillCount: {1}\n\n", this.gameData.player.ToString (), gameData.KillCount);

		StatsText.text += getEnemyStatsText ();
	}

	//for testing reading from gameData
	private string getEnemyStatsText ()
	{
		string retval = "Enemies:\n";
		foreach (var e in gameData.enemyList) {
			retval += e.ToString () + "\n";
		}
		return retval;
	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}

	public void NextImg ()
	{
		var playerSpriteList = gameData.assetData.PlayerList;
		playerSpriteIndex++;
		if (playerSpriteIndex >= playerSpriteList.Count) {
			playerSpriteIndex = 0;
		}
			
		var spriteAssetData = new SpriteAssetData ("Player", playerSpriteIndex);

		playerImg.sprite = gameData.assetData.getSprite (spriteAssetData);
		gameData.player.spriteAssetData = spriteAssetData;
		gameData.player.avatarSprite = playerImg.sprite;
	}

	public void Save ()
	{
		gameData.Save ();

	}
}
