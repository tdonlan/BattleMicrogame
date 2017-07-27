using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

	public int KillCount = 0;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
