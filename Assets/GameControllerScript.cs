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

	public GameData gameData;

	private System.Random r;

	private int roundCount = 1;

	private bool hasClicked = false;
	private bool hasClickedItem = false;
	public int SliderValue;
	public int ClickValue;
	public int TotalSliderValue = 100;

	private TurnData currentTurnData;
	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	private bool isPaused = false;
	private bool isCooldown = false;
	private float cooldownTimer = 0;
	private float totalCooldownTimer = .5f;

	//GameObjects
	public Enemy enemy;
	public Player player;

	//UI components
	public Slider slider;
	public Text turnText;
	public Text logText;
	public Text PlayerDmgText;
	public Text EnemyDmgText;

	public GameOverScript gameOverScript;

	public Slider PlayerHPSlider;
	public Slider EnemyHPSlider;
	public Text PlayerStatsText;
	public Text EnemyStatsText;

	public GameObject ItemButtonPanel;
	public GameObject ItemButtonPrefab;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		r = new System.Random ();

		RestartGame ();
	}

	public void RestartGame ()
	{
		ClearItemButtons ();
		turnTimer = 0;
		cooldownTimer = 0;
		hasClicked = false;
		hasClickedItem = false;
		ClickValue = 0;
		isCooldown = false;
		isPaused = false;
		this.enemy = new Enemy ("Rat", 1);
		this.player = new Player ();
		currentTurnData = enemy.getNextTurnData ();
		EnemyDmgText.text = "";
		PlayerDmgText.text = "";
		roundCount = 1;
		logText.text = "";

		LoadPlayerItemButtons ();

		UpdateStats ();
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

	private void nextTurn ()
	{

		player.UpdateEffects ();
		enemy.UpdateEffects ();

		//Player never clicked - auto lose
		if (!hasClicked) {
			//log this?
			ResolveBattle (Outcome.Lose, Accuracy.Fail);
		}

		CheckHealth ();

		//Reset for next turn

		LogValue ();
		EnemyDmgText.text = "";
		PlayerDmgText.text = "";
		isCooldown = !isCooldown;
		hasClicked = false;
		hasClickedItem = false;

		currentTurnData = enemy.getNextTurnData ();
		totalTurnTimer = currentTurnData.duration;
		cooldownTimer = 0;
		turnTimer = 0;
		ClickValue = 0;
		turnText.text = "";
		roundCount++;

		UpdateStats ();
	}

	public void ClickButton (int iAttackType)
	{
		if (!hasClicked) {
			hasClicked = true;
			float clickTime = turnTimer + cooldownTimer;
			ClickValue = Mathf.RoundToInt ((clickTime / totalTurnTimer) * 100);
			var acc = getAccuracy ();
			var outcome = getTurnOutcome ((AttackType)iAttackType, currentTurnData.enemyAttackType);
			turnText.text = string.Format ("{0} - {1}\n{2} - {3}", outcome, getAccValue (acc), ClickValue, acc);
			ResolveBattle (outcome, getAccValue (acc));
		}
	}

	private void ResolveBattle (Outcome outcome, Accuracy acc)
	{
		
		switch (outcome) {
		case Outcome.Win:
			var dmg = getModifiedDmg (player.Damage, acc);
			displayDmg (true, dmg);
			enemy.Hit (dmg);
			break;
		case Outcome.Draw:
			var eDmg = getModifiedDmg (Mathf.RoundToInt (player.Damage * .5f), acc);
			displayDmg (true, eDmg);
			enemy.Hit (eDmg);

			var pDmg = Mathf.RoundToInt (enemy.Damage * .5f);
			displayDmg (false, pDmg);
			player.Hit (pDmg);
			break;
		case Outcome.Lose:
			dmg = Mathf.RoundToInt (enemy.Damage);
			displayDmg (false, dmg);
			player.Hit (dmg);
			break;
		}

		UpdateStats ();

	}

	//
	public void CheckHealth ()
	{
		if (enemy.HP <= 0) {
			isPaused = true;
			gameData.KillCount++;
			gameOverScript.Show ("You Win!");
		} else if (player.HP <= 0) {
			isPaused = true;
			gameOverScript.Show ("You Died!");
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

	private void displayDmg (bool isEnemy, int amt)
	{
		var dmgColor = Color.red;
		if (amt < 0) {
			dmgColor = Color.green;
		}

		if (isEnemy) {
			EnemyDmgText.text = amt.ToString ();
			EnemyDmgText.color = dmgColor;
		} else {
			PlayerDmgText.text = amt.ToString ();
			PlayerDmgText.color = dmgColor;
		}
			
	}

	public void UseItem (int index)
	{
		if (!hasClickedItem) {
			if (player.itemList.Count > index) {
				hasClickedItem = true;
				var item = player.itemList [index];

				//TODO: dont use item yet
				item.UseItem (player, enemy);
				logText.text += string.Format ("Used {0}\n", item.Name);
			}
		}

		UpdateStats ();
	
	}

	private void UpdateStats ()
	{
		PlayerStatsText.text = player.GetStats ();
		EnemyStatsText.text = enemy.GetStats ();
		PlayerHPSlider.value = player.HPSliderValue;
		EnemyHPSlider.value = enemy.HPSliderValue;
	}


	private void ClearItemButtons ()
	{
		foreach (Transform child in ItemButtonPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	private void LoadPlayerItemButtons ()
	{
		var count = 0;
		foreach (var item in player.itemList) {
			Debug.Log (string.Format ("Adding item {0} ", item.Name));
			LoadItemButton (item.Name, count);
			count++;
		}
	}

	private void LoadItemButton (string name, int index)
	{
		//instantiate prefab
		var itemButton = Instantiate (ItemButtonPrefab);


		//update button text
		var itemButtonText = itemButton.GetComponentInChildren<Text> ();
		itemButtonText.text = name;

		//update button on click
		var itemButtonBtn = itemButton.GetComponent<Button> ();
		itemButtonBtn.onClick.AddListener (delegate {
			UseItem (index);
		});

		//add to panel.
		itemButton.transform.SetParent (ItemButtonPanel.transform);
	}
}
