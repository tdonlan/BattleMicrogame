using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

	public int KillCount = 0;
	public Player player;

	public List<Enemy> enemyList;

	public AssetData assetData;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
		this.assetData = new AssetData ();

		InitPlayer ();

		enemyList = new List<Enemy> ();
		PopulateEnemyList (player.Level);
	
	}

	private void InitPlayer ()
	{
		this.player = new Player (this.assetData);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	private void PopulateEnemyList (int level)
	{
		enemyList.Add (EnemyFactory.GenerateEnemy (level, -.5f, this.assetData));
		enemyList.Add (EnemyFactory.GenerateEnemy (level, 0, this.assetData));
		enemyList.Add (EnemyFactory.GenerateEnemy (level, .5f, this.assetData));
	}

	//remove given enemy from list, populate a new one of same difficulty
	public void UpdateEnemyList (Enemy e)
	{
		var index = enemyList.IndexOf (e);
		enemyList.Remove (e);

		switch (index) {
		case 0: //easy
			enemyList.Insert (index, EnemyFactory.GenerateEnemy (player.Level, -.5f, this.assetData));
			break;
		case 1:
			enemyList.Insert (index, EnemyFactory.GenerateEnemy (player.Level, 0, this.assetData));
			break;
		case 2:
			enemyList.Insert (index, EnemyFactory.GenerateEnemy (player.Level, .5f, this.assetData));
			break;
		default:
			enemyList.Insert (index, EnemyFactory.GenerateEnemy (player.Level, 0, this.assetData));
			break;
		}
	}
}
