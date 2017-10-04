using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour
{

	private Vector3 showPosition = new Vector3 (0, 0, 0);

	private float displayTimer = 0f;
	private float displayTime = 2f;

	private bool isHiding = true;

	public GameControllerScript gameControllerScript;

	public Text PlayerText;
	public Text EnemyText;

	public RectTransform rectTransform;

	// Use this for initialization
	void Start ()
	{
		rectTransform.localPosition = showPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isHiding) {
			displayTimer += Time.deltaTime;
			Debug.Log (string.Format ("{0} / {1} ", displayTimer, displayTime));
			if (displayTimer >= displayTime) {
				Hide ();
			}
		}
	}

	public void Show ()
	{
		displayTimer = 0;
		Debug.Log ("Showing splash screen");
		PlayerText.text = gameControllerScript.gameData.player.Name;
		EnemyText.text = gameControllerScript.enemy.Name;
		isHiding = false;
		rectTransform.localPosition = showPosition;
	}

	public void Hide ()
	{
		Debug.Log ("Calling splash screen hide");
		isHiding = true;
		gameControllerScript.isStart = true;
		rectTransform.localPosition = new Vector3 (0, -3000, 0);
	}
}
