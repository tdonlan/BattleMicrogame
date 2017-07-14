using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
	private Vector3 showPosition = new Vector3 (0, 0, 0);

	public GameControllerScript gameControllerScript;
	public Text gameOverText;
	public RectTransform rectTransform;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Show (string text)
	{
		gameOverText.text = text;
		rectTransform.localPosition = showPosition;

	}

	public void Hide ()
	{
		rectTransform.localPosition = new Vector3 (0, -3000, 0);

	}

	public void RestartGame ()
	{
		Hide ();
		gameControllerScript.RestartGame ();
	}
}
