using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

	public void Save ()
	{
		SavePlayerData sp = SavePlayerData.fromPlayer (this.player);
		string json = JsonUtility.ToJson (sp);
		Debug.Log (json);

		BinaryFormatter BinForm = new BinaryFormatter (); // creates a new variabe called "BinForm" that stores a "binary formatter" in charge of writing files to binary
		var path = Application.persistentDataPath + "/playerData.dat";

		FileStream file = File.Create (path); //creates a file

		BinForm.Serialize (file, sp); // writes the "data" container to the file
		file.Close (); // closes file
		Debug.Log ("Data Saved to " + path);

		Load ();

	}

	public void Load ()
	{
		if (File.Exists (Application.persistentDataPath + "/playerData.dat")) { 
			BinaryFormatter BinForm = new BinaryFormatter (); //creates a new variabe called "BinForm" that stores a "binary formatter" in charge of writing files to binary
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat", FileMode.Open); // if the file already exists it opens that file
			SavePlayerData sp = (SavePlayerData)BinForm.Deserialize (file); // deserialized the file and casts it to something we can understand (gamedata)binForm
			file.Close (); // closes file

			string json = JsonUtility.ToJson (sp);
			Debug.Log ("Just loaded" + json);

			this.player = new Player (this.assetData, sp);
		}
	}
}

