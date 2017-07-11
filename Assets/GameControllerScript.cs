using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	private System.Random r;

	public int SliderValue;
	public int TotalSliderValue = 100;

	private float turnTimer;
	private float totalTurnTimer = 2;

	//UI components
	public Slider slider;
	public Button button;
	public Text text;

	// Use this for initialization
	void Start ()
	{
		r = new System.Random ();
	}

	// Update is called once per frame
	void Update ()
	{
		turnTimer += Time.deltaTime;
		if (turnTimer > totalTurnTimer) {
			turnTimer = 0;
			totalTurnTimer = getTimerValue ();
		}

		SliderValue = Mathf.RoundToInt ((turnTimer / totalTurnTimer) * 100);
		slider.value = SliderValue;
	}

	private float getTimerValue ()
	{
		return .5f + ((float)r.NextDouble () * 2f);
	}

	public void ClickButton ()
	{
		text.text = string.Format ("{0}", SliderValue);
	}
}
