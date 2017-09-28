using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemControllerScript : MonoBehaviour
{

	const int NumItems = 12;

	public GameData gameData;

	public GameObject ItemEntryPrefab;

	public GameObject ItemEntryListPanel;
	private int itemOffset = 0;

	private List<Item> currentItemList;
	private List<ItemEntryControllerScript> currentItemEntryList;

	//used for pagination of long item lists
	private int currentPage;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();

		for (int i = 0; i < 50; i++) {
			gameData.player.itemList.Add (ItemFactory.GenerateWeapon (gameData.player.Level, UnityEngine.Random.Range (-1f, 1f)));
		}

		SelectItems ();
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
		itemOffset = 0;
		currentItemList = gameData.player.GetWeapons ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	public void SelectArmor ()
	{
		itemOffset = 0;
		currentItemList = gameData.player.GetArmor ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	public void SelectItems ()
	{
		itemOffset = 0;
		currentItemList = gameData.player.GetItems ();
		SetItemEntryList (currentItemList.Take (NumItems).ToList ());
	}

	//this should toggle through sorting criteria (alpha, level, stats)
	public void SortItems ()
	{
		itemOffset = 0;
		currentItemList = currentItemList.OrderBy (x => x.Name).ToList (); 
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
		gameData.player.EquipItem (itemScript.item);
		currentItemList.Remove (itemScript.item);
		currentItemEntryList.Remove (itemScript);
		Destroy (itemScript.gameObject);
	}

	public void DeleteItem (ItemEntryControllerScript itemScript)
	{
		gameData.player.itemList.Remove (itemScript.item); //call helper on player inventory here
		currentItemList.Remove (itemScript.item);
		currentItemEntryList.Remove (itemScript);
		Destroy (itemScript.gameObject);
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
