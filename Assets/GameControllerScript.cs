using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	private System.Random r;

	private int roundCount = 1;

	public int SliderValue;
	public int ClickValue;
	public int TotalSliderValue = 100;

	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	private bool isCooldown = true;
	private float cooldownTimer = 0;
	private float totalCooldownTimer = .5f;

	//UI components
	public Slider slider;
	public Button button;
	public Text text;
	public Text logText;

	// Use this for initialization
	void Start ()
	{
		r = new System.Random ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isCooldown) {
			slider.value = 0;
			cooldownTimer += Time.deltaTime;
			if (cooldownTimer >= totalCooldownTimer) {
				LogValue ();
				isCooldown = !isCooldown;

				totalTurnTimer = getNewTimerValue ();
				cooldownTimer = 0;
				turnTimer = 0;
				ClickValue = 0;
				roundCount++;
			}
		} else {
			turnTimer += Time.deltaTime;
			SliderValue = Mathf.RoundToInt ((turnTimer / totalTurnTimer) * 100);
			slider.value = SliderValue;
			if (turnTimer >= totalTurnTimer) {
				isCooldown = !isCooldown;
			}
		}
	}

	private float getNewTimerValue ()
	{
		return .5f + ((float)r.NextDouble () * 2f);
	}

	public void ClickButton ()
	{
		float clickTime = turnTimer + cooldownTimer;
		ClickValue = Mathf.RoundToInt ((clickTime / totalTurnTimer) * 100);
		text.text = string.Format ("{0} - {1}", ClickValue, getAccuracy ());
	}

	private float getAccuracy ()
	{
		if (ClickValue > TotalSliderValue) {
			ClickValue = TotalSliderValue - (ClickValue - TotalSliderValue);
		}

		float acc = (float)ClickValue / (float)TotalSliderValue;
		return acc;
	}

	private void LogValue ()
	{
		logText.text += string.Format ("{0} - {1}\n", roundCount, getAccuracy ());
	}
}
