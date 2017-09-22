using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneController : MonoBehaviour
{

	public GameData gameData;

	public Text testText;
	// Use this for initialization
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
		gameData.player.Level++;
	}

	public void GenerateEnemy ()
	{
		var enemy = EnemyFactory.GenerateEnemy (1, Random.Range (.75f, 1));

		var enemyStr = enemy.ToString ();
		testText.text = enemyStr;
	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}
}
