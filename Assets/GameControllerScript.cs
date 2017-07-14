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

public enum Accuracy
{
	Crit,
	Hit,
	Miss,
	Fail
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

	private bool hasClicked = false;
	public int SliderValue;
	public int ClickValue;
	public int TotalSliderValue = 100;

	private TurnData currentTurnData;
	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	private bool isPaused = false;
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
	public GameOverScript gameOverScript;

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
		if (!isPaused) {
			if (isCooldown) {
				slider.value = 0;
				cooldownTimer += Time.deltaTime;
				if (cooldownTimer >= totalCooldownTimer) {
					nextTurn ();
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
	}

	public void RestartGame ()
	{
		isPaused = false;
		this.enemy = new Enemy ("Rat", 1);
		this.player = new Player ();
		PlayerHPSlider.value = player.HPSliderValue;
		EnemyHPSlider.value = enemy.HPSliderValue;
		roundCount = 1;
		logText.text = "";
	}

	private void nextTurn ()
	{
		LogValue ();
		isCooldown = !isCooldown;
		hasClicked = false;

		currentTurnData = enemy.getNextTurnData ();
		totalTurnTimer = currentTurnData.duration;
		cooldownTimer = 0;
		turnTimer = 0;
		ClickValue = 0;
		roundCount++;
	}

	public void ClickButton (int iAttackType)
	{
		if (!hasClicked) {
			hasClicked = true;
			float clickTime = turnTimer + cooldownTimer;
			ClickValue = Mathf.RoundToInt ((clickTime / totalTurnTimer) * 100);
			var acc = getAccuracy ();
			var outcome = getTurnOutcome ((AttackType)iAttackType, currentTurnData.enemyAttackType);
			text.text = string.Format ("{0} - {1}\n{2} - {3}", outcome, getAccValue (acc), ClickValue, acc);
			ResolveBattle (outcome, getAccValue (acc));

		}
	}

	private void ResolveBattle (Outcome outcome, Accuracy acc)
	{
		//TODO: add multipliers based on accuracy.

		bool enemyDead = false;
		bool playerDead = false;
		switch (outcome) {
		case Outcome.Win:
			enemyDead = enemy.Hit (getModifiedDmg (player.Damage, acc));
			break;
		case Outcome.Draw:
			enemyDead = enemy.Hit (getModifiedDmg (Mathf.RoundToInt (player.Damage * .5f), acc));
			playerDead = player.Hit (Mathf.RoundToInt (enemy.Damage * .5f));
			break;
		case Outcome.Lose:
			playerDead = player.Hit (enemy.Damage); //todo: incorporate random / enemy skills into accuracy
			break;
		}

		PlayerHPSlider.value = player.HPSliderValue;
		EnemyHPSlider.value = enemy.HPSliderValue;


		//TODO;check for enemy/ player dead and popup battle over screen.
		if (playerDead) {
			isPaused = true;
			gameOverScript.Show ("You Died!");
		} else if (enemyDead) {
			isPaused = true;
			gameOverScript.Show ("You Win!");
		}
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

	private Accuracy getAccValue (float acc)
	{
		if (acc >= .98f) {
			return Accuracy.Crit;
		} else if (acc >= .85f) {
			return Accuracy.Hit;
		} else if (acc > .5f) {
			return Accuracy.Miss;
		} else {
			return Accuracy.Fail;
		}
	}

	private int getModifiedDmg (int dmg, Accuracy acc)
	{
		switch (acc) {
		case Accuracy.Crit:
			return Mathf.RoundToInt (dmg * 1.5f);
		case Accuracy.Hit:
			return dmg;
		case Accuracy.Miss:
			return 0;
		case Accuracy.Fail:
			return 0;
		default:
			return 0;
		}
	}

	private void LogValue ()
	{
		var acc = getAccuracy ();
		logText.text += string.Format ("{0} - {1}\n", roundCount, acc);
	}
}
