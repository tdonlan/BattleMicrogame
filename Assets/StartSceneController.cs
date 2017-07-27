using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
}
