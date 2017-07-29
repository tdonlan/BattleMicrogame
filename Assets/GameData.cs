using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

	public int KillCount = 0;
	public Player player;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
		InitPlayer ();
	}

	private void InitPlayer ()
	{
		this.player = new Player ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
