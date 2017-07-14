using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType
{
	Attack,
	Counter,
	Special
}

public enum Outcome
{
	Win,
	Lose,
	Draw
}

public class TurnData
{
	public float duration;
	public AttackType enemyAttackType;
}

public class GameControllerScript : MonoBehaviour
{
	private System.Random r;

	private int roundCount = 1;



	public int SliderValue;
	public int ClickValue;
	public int TotalSliderValue = 100;

	private TurnData currentTurnData;
	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	private bool isCooldown = true;
	private float cooldownTimer = 0;
	private float totalCooldownTimer = .5f;

	//GameObjects
	public Enemy enemy;
	public Player player;

	//UI components
	public Slider slider;
	public Button button;
	public Text text;
	public Text logText;

	public Slider PlayerHPSlider;
	public Slider EnemyHPSlider;

	// Use this for initialization
	void Start ()
	{
		r = new System.Random ();

		this.enemy = new Enemy ("Rat", 1);
		this.player = new Player ();
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

				currentTurnData = enemy.getNextTurnData ();
				totalTurnTimer = currentTurnData.duration;
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

	public void ClickButton (int iAttackType)
	{
		float clickTime = turnTimer + cooldownTimer;
		ClickValue = Mathf.RoundToInt ((clickTime / totalTurnTimer) * 100);
		var acc = getAccuracy ();
		var outcome = getTurnOutcome ((AttackType)iAttackType, currentTurnData.enemyAttackType);
		text.text = string.Format ("{0} - {1}\n{2} - {3}", outcome, getAccValue (acc), ClickValue, acc);
		ResolveBattle (outcome, acc);
	}

	private void ResolveBattle (Outcome outcome, float accuracy)
	{
		//TODO: add multipliers based on accuracy.

		bool enemyDead = false;
		bool playerDead = false;
		switch (outcome) {
		case Outcome.Win:
			enemyDead = enemy.Hit (player.Damage);
			break;
		case Outcome.Draw:
			enemyDead = enemy.Hit (Mathf.RoundToInt (player.Damage * .5f));
			playerDead = player.Hit (Mathf.RoundToInt (enemy.Damage * .5f));
			break;
		case Outcome.Lose:
			playerDead = player.Hit (enemy.Damage);
			break;

		}

		PlayerHPSlider.value = player.HPSliderValue;
		EnemyHPSlider.value = enemy.HPSliderValue;

		//TODO;check for enemy/ player dead and popup battle over screen.
	}

	//who wins the battle?
	private Outcome getTurnOutcome (AttackType playerAttackType, AttackType enemyAttackType)
	{
		if (playerAttackType == enemyAttackType) {
			return Outcome.Draw;
		} else if (enemyAttackType == AttackType.Attack && playerAttackType == AttackType.Special) {
			return Outcome.Lose;
		} else if (playerAttackType == AttackType.Attack && enemyAttackType == AttackType.Special) {
			return Outcome.Win;
		} else if ((int)playerAttackType > (int)enemyAttackType) {
			return Outcome.Win;
		}
		return Outcome.Lose;
	}

	private float getAccuracy ()
	{
		if (ClickValue > TotalSliderValue) {
			ClickValue = TotalSliderValue - (ClickValue - TotalSliderValue);
		}

		float acc = (float)ClickValue / (float)TotalSliderValue;
		return acc;
	}



	private string getAccValue (float acc)
	{
		if (acc >= .98f) {
			return "CRIT!";
		} else if (acc >= .85f) {
			return "Hit";
		} else if (acc > .5f) {
			return "Miss";
		} else {
			return "Fail!";
		}
	}

	private void LogValue ()
	{
		var acc = getAccuracy ();
		logText.text += string.Format ("{0} - {1}\n", roundCount, acc);
	}
}
