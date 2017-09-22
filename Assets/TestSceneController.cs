using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneController : MonoBehaviour
{

	public GameData gameData;

	public Text testText;
	public Text playerText;

	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void LevelUp ()
	{
		gameData.player.LevelUp ();
		playerText.text = gameData.player.GetStats ();
	}

	public void GenerateEnemy ()
	{
		var enemy = EnemyFactory.GenerateEnemy (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));

		var enemyStr = enemy.ToString ();
		testText.text = enemyStr;
	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}
}
