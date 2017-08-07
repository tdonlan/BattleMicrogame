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

	}

	void OnLevelWasLoaded ()
	{
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
		Debug.Log ("Loading GameScene");
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
}
