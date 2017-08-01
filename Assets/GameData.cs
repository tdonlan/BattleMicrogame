using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

	public int KillCount = 0;
	public Player player;

	public List<Enemy> enemyList;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
		InitPlayer ();

		enemyList = new List<Enemy> ();
		PopulateEnemyList (player.Level);
	
	}

	private void InitPlayer ()
	{
		this.player = new Player ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	private void PopulateEnemyList (int level)
	{
		enemyList.Add (Enemy.GenerateEnemy (level, -.5f));
		enemyList.Add (Enemy.GenerateEnemy (level, 0));
		enemyList.Add (Enemy.GenerateEnemy (level, .5f));
	}

	//remove given enemy from list, populate a new one of same difficulty
	public void UpdateEnemyList (Enemy e)
	{
		var index = enemyList.IndexOf (e);
		enemyList.Remove (e);

		switch (index) {
		case 0: //easy
			enemyList.Insert (index, Enemy.GenerateEnemy (player.Level, -.5f));
			break;
		case 1:
			enemyList.Insert (index, Enemy.GenerateEnemy (player.Level, 0));
			break;
		case 2:
			enemyList.Insert (index, Enemy.GenerateEnemy (player.Level, .5f));
			break;
		default:
			enemyList.Insert (index, Enemy.GenerateEnemy (player.Level, 0));
			break;
		}
	}
}
