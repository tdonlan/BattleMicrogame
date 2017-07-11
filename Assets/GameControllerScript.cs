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

	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	private bool isCooldown = true;
	private float cooldownTimer = 0;
	private float totalCooldownTimer = .5f;

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
		if (isCooldown) {
			slider.value = 0;
			cooldownTimer += Time.deltaTime;
			if (cooldownTimer >= totalCooldownTimer) {
				isCooldown = !isCooldown;
				cooldownTimer = 0;
			}
		} else {
			turnTimer += Time.deltaTime;
			SliderValue = Mathf.RoundToInt ((turnTimer / totalTurnTimer) * 100);
			slider.value = SliderValue;
			if (turnTimer >= totalTurnTimer) {
				turnTimer = 0;
				totalTurnTimer = getTimerValue ();
				isCooldown = !isCooldown;
			}
		}
	}

	private float getTimerValue ()
	{
		return .5f + ((float)r.NextDouble () * 2f);
	}

	public void ClickButton ()
	{
		if (!isCooldown) {
			text.text = string.Format ("{0}", SliderValue);
		}
	}
}
