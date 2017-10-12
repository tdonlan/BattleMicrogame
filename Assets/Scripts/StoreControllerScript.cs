using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreControllerScript : MonoBehaviour
{

	public enum ItemDisplayType
	{
		Weapon,
		Armor,
		Item,
	}

	const int NumItems = 12;

	public GameData gameData;

	public GameObject ItemEntryPrefab;

	public GameObject ItemEntryListPanel;
	private ItemDisplayType currentItemDisplay;

	//alpha, reverseAlpha, level, reverseLevel
	private int sortType = 0;
	private int itemOffset = 0;

	private bool isSell = true;
	private List<Item> storeList;
	private List<Item> currentItemList;
	private List<ItemEntryControllerScript> currentItemEntryList;

	public Text PlayerGoldText;


	//used for pagination of long item lists
	private int currentPage;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();
		storeList = ItemFactory.GenerateStore (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f));
		PlayerGoldText.text = string.Format ("{0}", gameData.player.Gold);

		SelectWeapons ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void BackButton ()
	{
		SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
	}

	public void SelectBuyView ()
	{
		isSell = false;
		SelectWeapons ();
	}

	public void SelectSellView ()
	{
		isSell = true;
		SelectWeapons ();
	}

	public void SelectWeapons ()
	{
		currentItemDisplay = ItemDisplayType.Weapon;
		itemOffset = 0;
		if (isSell) {
			currentItemList = gameData.player.GetWeapons ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		} else {
			var wepList = from i in storeList
			              where i is Weapon
			              select i;
			currentItemList = wepList.ToList ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		}

	}

	public void SelectArmor ()
	{
		currentItemDisplay = ItemDisplayType.Armor;
		itemOffset = 0;
		if (isSell) {
			currentItemList = gameData.player.GetArmor ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		} else {
			var armorList = from i in storeList
			                where i is Armor
			                select i;
			currentItemList = armorList.ToList ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		}
	
	}

	public void SelectItems ()
	{
		currentItemDisplay = ItemDisplayType.Item;
		itemOffset = 0;
		if (isSell) {
			currentItemList = gameData.player.GetItems ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		} else {
			var iList = from i in storeList
			            where (!(i is Armor)) && (!(i is Weapon))
			            select i;
			currentItemList = iList.ToList ();
			SetItemEntryList (currentItemList.Take (NumItems).ToList ());
		}
	
	}

	private void RefreshItems ()
	{
		PlayerGoldText.text = string.Format ("{0} gp", gameData.player.Gold);
		SetItemEntryList (currentItemList.Skip (itemOffset * NumItems).Take (NumItems).ToList ());
	}

	//this should toggle through sorting criteria (alpha, level, stats)
	public void SortItems ()
	{
		itemOffset = 0;

		sortType++;
		if (sortType > 3) {
			sortType = 0;
		}
		switch (sortType) {
		case 0:
			//alpha
			currentItemList = currentItemList.OrderBy (x => x.Name).ToList (); 
			break;
		case 1:
			//reverse alpha
			currentItemList = currentItemList.OrderBy (x => x.Name).Reverse ().ToList (); 
			break;
		case 2:
			//level
			currentItemList = currentItemList.OrderBy (x => x.Level).ToList (); 
			break;
		case 3:
			//reverse level
			currentItemList = currentItemList.OrderBy (x => x.Level).Reverse ().ToList (); 
			break;
		default:
			break;
		}

		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	public void Next ()
	{
		if (itemOffset < Mathf.FloorToInt ((float)currentItemList.Count / (float)NumItems)) {
			itemOffset++;
		}
		SetItemEntryList (currentItemList.Skip (itemOffset * NumItems).Take (NumItems).ToList ());
	}

	public void Prev ()
	{
		if (itemOffset > 0) {
			itemOffset--;
		}

		SetItemEntryList (currentItemList.Skip (itemOffset * NumItems).Take (NumItems).ToList ());
	}

	public void SellItem (ItemEntryControllerScript itemScript)
	{
		if (gameData.player.itemList.Contains (itemScript.item)) {
			storeList.Add (itemScript.item);
			gameData.player.Gold += itemScript.item.Price; //multiplier for store?
			gameData.player.itemList.Remove (itemScript.item);
			currentItemList.Remove (itemScript.item);
			RefreshItems ();
		}
	}

	public void BuyItem (ItemEntryControllerScript itemScript)
	{
		if (gameData.player.Gold >= itemScript.item.Price && storeList.Contains (itemScript.item)) {
			storeList.Remove (itemScript.item);
			gameData.player.Gold -= itemScript.item.Price;
			currentItemList.Remove (itemScript.item);
			gameData.player.itemList.Add (itemScript.item);
			RefreshItems ();
		}
	}

	//given a list of items (12), set the itemEntryPanel
	private void SetItemEntryList (List<Item> itemList)
	{
		//clear current  panel
		foreach (Transform child in ItemEntryListPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}

		currentItemEntryList = new List<ItemEntryControllerScript> ();
		foreach (var i in itemList) {
			var itemEntry = InitItemEntry (i);
			currentItemEntryList.Add (itemEntry);
		}
	}

	private ItemEntryControllerScript InitItemEntry (Item i)
	{
		var itemEntry = Instantiate (ItemEntryPrefab);
		var texts = itemEntry.GetComponentsInChildren<Text> ();
		foreach (var t in texts) {
			if (t.gameObject.name == "ItemNameText") {
				t.text = i.Name;
			}
			if (t.gameObject.name == "ItemStatsText") {
				t.text = i.ToString ();
			}
		}
		var buttons = itemEntry.GetComponentsInChildren<Button> ();
		foreach (var b in buttons) {
			if (b.gameObject.name == "EquipButton") {
				b.gameObject.SetActive (false);
			}
			if (b.gameObject.name == "DropButton") {
				b.gameObject.SetActive (false);
			}
			if (b.gameObject.name == "BuyButton") {
				if (isSell) {
					b.gameObject.SetActive (false);
				} else {
					b.gameObject.SetActive (true);
				}
			}
			if (b.gameObject.name == "SellButton") {
				if (isSell) {
					b.gameObject.SetActive (true);
				} else {
					b.gameObject.SetActive (false);
				}
			}
		}

		var itemEntryScript = itemEntry.GetComponentInChildren<ItemEntryControllerScript> ();
		itemEntryScript.item = i;

		itemEntry.transform.parent = ItemEntryListPanel.transform; 

		return itemEntryScript;
	}
}
