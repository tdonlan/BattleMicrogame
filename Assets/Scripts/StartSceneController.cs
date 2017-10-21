using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
	public GameData gameData;
	public Text EnemyStatsText;



	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		PopulateEnemyBattleButtons ();

	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public void PopulateEnemyBattleButtons ()
	{
		if (gameData.enemyList != null) {
			foreach (var e in gameData.enemyList) {
				EnemyStatsText.text += e.ToString () + "\n";
			}
		}
	}

	public void StartBattle ()
	{
		SceneManager.LoadScene ("GameScene", LoadSceneMode.Single);
	}

	public void ShowCharStats ()
	{
		SceneManager.LoadScene ("CharacterScene", LoadSceneMode.Single);
	}

	public void ShowTestScreen ()
	{
		SceneManager.LoadScene ("TestScene", LoadSceneMode.Single);
	}

	public void ShowItems ()
	{
		SceneManager.LoadScene ("ItemScene", LoadSceneMode.Single);
	}

	public void ShowStore ()
	{
		SceneManager.LoadScene ("StoreScene", LoadSceneMode.Single);
	}

	public void ShowLoc ()
	{
		SceneManager.LoadScene ("LocScene", LoadSceneMode.Single);
	}
}
