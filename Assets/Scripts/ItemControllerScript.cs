using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemControllerScript : MonoBehaviour
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
	public GameObject CurrentItemPanel;
	private ItemDisplayType currentItemDisplay;

	//alpha, reverseAlpha, level, reverseLevel
	private int sortType = 0;
	private int itemOffset = 0;

	private List<Item> currentItemList;
	private List<ItemEntryControllerScript> currentItemEntryList;


	//used for pagination of long item lists
	private int currentPage;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();

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

	public void SelectWeapons ()
	{
		currentItemDisplay = ItemDisplayType.Weapon;
		SetCurrentItemText ();
		itemOffset = 0;
		currentItemList = gameData.player.GetWeapons ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	public void SelectArmor ()
	{
		currentItemDisplay = ItemDisplayType.Armor;
		SetCurrentItemText ();
		itemOffset = 0;
		currentItemList = gameData.player.GetArmor ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	public void SelectItems ()
	{
		currentItemDisplay = ItemDisplayType.Item;
		SetCurrentItemText ();
		itemOffset = 0;
		currentItemList = gameData.player.GetItems ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	private void RefreshItems ()
	{
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

	public void EquipItem (ItemEntryControllerScript itemScript)
	{
		//trade items in player inventory
		var oldItem = gameData.player.EquipItem (itemScript.item);
		if (oldItem != null) {
			currentItemList.Add (oldItem);
		}

		currentItemList.Remove (itemScript.item);

		SetCurrentItemText ();
		RefreshItems ();
	}

	private void SetCurrentItemText ()
	{
		switch (currentItemDisplay) {
		case ItemDisplayType.Weapon:
			SetCurrentItemText (gameData.player.weapon);
			break;
		case ItemDisplayType.Armor:
			SetCurrentItemText (gameData.player.armor);
			break;
		case ItemDisplayType.Item:
			SetCurrentItemText (gameData.player.usableItemList);
			break;
		}
	}

	private void SetCurrentItemText (List<Item> itemList)
	{
		var curItemText = CurrentItemPanel.GetComponentInChildren<Text> ();
		curItemText.text = "";
		foreach (var i in itemList) {
			curItemText.text += i.Name + ",    ";
		}
	}

	private void SetCurrentItemText (Item i)
	{
		var curItemText = CurrentItemPanel.GetComponentInChildren<Text> ();
		curItemText.text = "";
		if (i != null) {

			var itemStr = i.Name + "\n" + i.ToString ();
			curItemText.text = itemStr;
		}
	}

	public void DeleteItem (ItemEntryControllerScript itemScript)
	{
		gameData.player.itemList.Remove (itemScript.item); //call helper on player inventory here
		currentItemList.Remove (itemScript.item);
		RefreshItems ();
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

		var itemEntryScript = itemEntry.GetComponentInChildren<ItemEntryControllerScript> ();
		itemEntryScript.item = i;

		itemEntry.transform.parent = ItemEntryListPanel.transform; 

		return itemEntryScript;
	}
}
