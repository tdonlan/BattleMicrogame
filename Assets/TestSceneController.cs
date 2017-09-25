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

	public void GenerateWeapon ()
	{
		var weapon = ItemFactory.GenerateWeapon (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));
		var wepStr = weapon.Name + "\n" + weapon.ToString ();

		testText.text = wepStr;
	}

	public void GenerateArmor ()
	{
		var armor = ItemFactory.GenerateArmor (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));
		var armorStr = armor.Name + "\n" + armor.ToString ();

		testText.text = armorStr;
	}

	public void GenerateItem ()
	{
		var item = ItemFactory.GenerateItem (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));
		var itemStr = item.Name + "\n" + item.ToString ();

		testText.text = itemStr;
	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}
}
