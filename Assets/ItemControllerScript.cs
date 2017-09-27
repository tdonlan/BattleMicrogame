using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemControllerScript : MonoBehaviour
{
	public GameData gameData;

	public GameObject ItemEntryPrefab;

	public GameObject ItemEntryListPanel;

	private List<Item> currentItemList;
	private List<ItemEntryControllerScript> currentItemEntryList;

	//used for pagination of long item lists
	private int currentPage;

	// Use this for initialization
	void Start ()
	{
		gameData = GameObject.FindObjectOfType<GameData> ();

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
		currentItemList = gameData.player.GetWeapons ();
		SetItemEntryList (currentItemList);
	}

	public void SelectArmor ()
	{
		currentItemList = gameData.player.GetArmor ();
		SetItemEntryList (currentItemList);
	}

	public void SelectItems ()
	{
		currentItemList = gameData.player.GetItems ();
		SetItemEntryList (currentItemList);
	}

	public void DeleteItem (ItemEntryControllerScript itemScript)
	{
		gameData.player.itemList.Remove (itemScript.item);
		currentItemList.Remove (itemScript.item);
		currentItemEntryList.Remove (itemScript);
		Destroy (itemScript.gameObject);
	}

	//given a list of items (15?), set the itemEntryPanel
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
