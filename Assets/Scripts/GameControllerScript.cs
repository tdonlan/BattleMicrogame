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
	Draw,
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

	public override string ToString ()
	{
		return string.Format ("{0} {1}", enemyAttackType.ToString (), duration.ToString ());
	}
}

public class GameControllerScript : MonoBehaviour
{

	public GameData gameData;

	private int roundCount = 1;

	private bool hasClicked = false;
	private bool hasClickedItem = false;
	public int SliderValue;
	public int ClickValue;
	public int TotalSliderValue = 100;

	private TurnData currentTurnData;
	private float turnTimer = 0;
	private float totalTurnTimer = 2f;

	public bool isStart = false;
	private float startTimer;
	private float startTime;

	public bool isPaused = true;
	private bool isCooldown = false;
	private float cooldownTimer = 0;
	private float totalCooldownTimer = .5f;

	//GameObjects
	public Enemy enemy;
	//public Player player;

	//UI components
	public Slider slider;
	public Text turnText;
	public Text logText;
	public Text PlayerDmgText;
	public Text EnemyDmgText;
	public Text StartText;

	public GameOverScript gameOverScript;
	public SplashScreenScript splashScreenScript;

	public Slider PlayerHPSlider;
	public Slider EnemyHPSlider;

	public Text PlayerStatsText;
	public Text EnemyNameText;
	public Text EnemyStatsText;

	public GameObject ItemButtonPanel;
	public GameObject ItemButtonPrefab;

	public GameObject TurnInfoPanel;
	public GameObject TurnInfoPrefab;
	public List<GameObject> TurnInfoList;

	//images
	public Sprite AttackSprite;
	public Sprite CounterSprite;
	public Sprite SpecialSprite;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		gameData.player.AttachGameController (this);

		RestartGame ();
	}

	public void RestartGame ()
	{
		ClearItemButtons ();
		ClearTurnInfo ();
		turnTimer = 0;
		cooldownTimer = 0;
		hasClicked = false;
		hasClickedItem = false;
		ClickValue = 0;
		isCooldown = false;
		isPaused = true;
		isStart = false;
		startTimer = 3;

		this.enemy = EnemyFactory.GenerateEnemy (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));
		this.enemy.AttachGameController (this);

		currentTurnData = enemy.getNextTurnData ();
		EnemyDmgText.text = "";
		PlayerDmgText.text = "";
		roundCount = 1;
		logText.text = "";

		LoadPlayerItemButtons ();

		UpdateStats ();

		InitTurnInfoPanel ();
		splashScreenScript.Show ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isStart) {
			startTimer -= Time.deltaTime;
			StartText.text = string.Format ("{0}", Math.Ceiling (startTimer));
			if (startTimer <= 0) {
				isStart = false;
				isPaused = false;
				StartText.text = "";
			}
		} else if (!isPaused) {
			if (isCooldown) {
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
		slider.value = 0;
		gameData.player.UpdateEffects ();
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
		HighlightCurrentTurnInfo ();

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
			var dmg = getModifiedDmg (gameData.player.Damage, acc);
			enemy.Hit (dmg);
			break;
		case Outcome.Draw:
			var eDmg = getModifiedDmg (Mathf.RoundToInt (gameData.player.Damage * .5f), acc);
			enemy.Hit (eDmg);
			var pDmg = Mathf.RoundToInt (enemy.Damage * .5f);
			gameData.player.Hit (pDmg);
			break;
		case Outcome.Lose:
			dmg = Mathf.RoundToInt (enemy.Damage);
			gameData.player.Hit (dmg);
			break;
		}
			
		SetCurrentTurnInfo (currentTurnData.enemyAttackType, outcome);

		UpdateStats ();

	}

	//
	public void CheckHealth ()
	{
		if (enemy.HP <= 0) {
			isPaused = true;
			Win ();
		} else if (gameData.player.HP <= 0) {
			isPaused = true;
			Lose ();
		}
	}

	public void Win ()
	{
		gameData.KillCount++;
		gameData.player.GetXP (enemy.XP);
		gameData.player.Gold += enemy.Gold;
		gameData.player.itemList.AddRange (enemy.ItemList);

		var itemListStr = "";
		foreach (var i in enemy.ItemList) {
			itemListStr += i.Name + "\n";
		}
		gameOverScript.Show (string.Format ("You Win! \n XP: {0}\nGold:{1}\nLoot:{2}", enemy.XP, enemy.Gold, itemListStr));
	}

	public void Lose ()
	{
		var lostGold = Mathf.FloorToInt (gameData.player.Gold * .1f);
		gameOverScript.Show (string.Format ("You Died!\n You lost {0} gold! ", lostGold));
		gameData.player.HP = gameData.player.TotalHP;
		gameData.player.Gold -= lostGold;
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

	public void DisplayDmg (bool isEnemy, int amt)
	{
		var dmgColor = Color.red;
		if (amt < 0) {
			amt *= -1;
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
		if (!isPaused) {
			if (!hasClickedItem) {
				if (gameData.player.usableItemList.Count > index) {
					hasClickedItem = true;
					var item = gameData.player.usableItemList [index];
				
					//TODO: dont use item yet
					item.UseItem (gameData.player, enemy);
					gameData.player.usableItemList.RemoveAt (index);

					logText.text += string.Format ("Used {0}\n", item.Name);
					LoadPlayerItemButtons ();
				}
			}

			UpdateStats ();
		}
	}

	private void UpdateStats ()
	{
		PlayerStatsText.text = gameData.player.GetStats ();
		EnemyNameText.text = enemy.Name;
		EnemyStatsText.text = enemy.GetStats ();
		PlayerHPSlider.value = gameData.player.HPSliderValue;
		EnemyHPSlider.value = enemy.HPSliderValue;
	}

	private void ClearItemButtons ()
	{
		foreach (Transform child in ItemButtonPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	private void ClearTurnInfo ()
	{
		foreach (Transform child in TurnInfoPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	private void LoadPlayerItemButtons ()
	{
		foreach (Transform child in ItemButtonPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}

		var count = 0;
		foreach (var item in gameData.player.usableItemList) {
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

	//------------TURN INFO----------

	private void InitTurnInfoPanel ()
	{
		TurnInfoList = new List<GameObject> ();
		foreach (var ti in enemy.turnDataList) {
			TurnInfoList.Add (InitTurnInfo ());
		}
		HighlightCurrentTurnInfo ();
	}

	private GameObject InitTurnInfo ()
	{
		var turnInfo = Instantiate (TurnInfoPrefab);

		turnInfo.transform.parent = TurnInfoPanel.transform;

		return turnInfo;
	}

	private void HighlightCurrentTurnInfo ()
	{
		foreach (var ti in TurnInfoList) {
			var tiImages = ti.GetComponentsInChildren<Image> ();
			foreach (Image t in tiImages) {
				if (t.gameObject.name == "HighlightPanel") {
					if (ti.Equals (TurnInfoList [enemy.turnIndex])) {
						t.enabled = true;
					} else {
						t.enabled = false;
					}
				}
			}
		}
	}

	private void SetCurrentTurnInfo (AttackType enemyAttack, Outcome outcome)
	{
		var turnInfo = TurnInfoList [enemy.turnIndex];

		var tiImage = turnInfo.GetComponent<Image> ();
	
		var tiColor = Color.white;
		switch (outcome) {
		case Outcome.Win:
			tiColor = Color.green;
			break;
		case Outcome.Draw:
			tiColor = Color.yellow;
			break;
		case Outcome.Lose:
			tiColor = Color.red;
			break;
		default:
			break;
		}
		tiImage.color = tiColor;

		var images = turnInfo.GetComponentsInChildren<Image> ();
		foreach (var i in images) {
			if (i.gameObject.name == "AttackImage") {
				switch (enemyAttack) {
				case AttackType.Attack:
					i.sprite = AttackSprite;
					break;
				case AttackType.Counter:
					i.sprite = CounterSprite;
					break;
				case AttackType.Special:
					i.sprite = SpecialSprite;
					break;
				default:
					break;

				}
			}
		}
	}


}
