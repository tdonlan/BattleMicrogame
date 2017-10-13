using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntryControllerScript : MonoBehaviour
{
	public Item item;
	public ItemControllerScript itemControllerScript;
	public StoreControllerScript storeControllerScript;

	// Use this for initialization
	void Start ()
	{
		itemControllerScript = GameObject.FindObjectOfType<ItemControllerScript> ();
		storeControllerScript = GameObject.FindObjectOfType<StoreControllerScript> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void DropItem ()
	{
		itemControllerScript.DeleteItem (this);
	}


	public void EquipItem ()
	{
		itemControllerScript.EquipItem (this);
	}

	public void SellItem ()
	{
		storeControllerScript.SellItem (this);
	}

	public void BuyItem ()
	{
		storeControllerScript.BuyItem (this);
	}
}
