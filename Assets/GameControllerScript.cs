using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{

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
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		turnTimer += Time.deltaTime;
		if (turnTimer > totalTurnTimer) {
			turnTimer = 0;
		}

		SliderValue = Mathf.RoundToInt ((turnTimer / totalTurnTimer) * 100);
		slider.value = SliderValue;
	}

	public void ClickButton ()
	{
		text.text = string.Format ("{0}", SliderValue);
	}
}
