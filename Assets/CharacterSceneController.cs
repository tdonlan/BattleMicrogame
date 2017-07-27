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
		StatsText.text = string.Format ("KillCount: {0}", gameData.KillCount);
	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}
}
