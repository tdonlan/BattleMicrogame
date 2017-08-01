using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSceneController : MonoBehaviour
{

	public GameData gameData;
	public Text StatsText;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		SetStatsText ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
}
