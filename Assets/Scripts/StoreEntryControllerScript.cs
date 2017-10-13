using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEntryControllerScript : MonoBehaviour
{
	public Item item;
	public StoreControllerScript storeControllerScript;

	// Use this for initialization
	void Start ()
	{
		storeControllerScript = GameObject.FindObjectOfType<StoreControllerScript> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void SellItem ()
	{
		//storeControllerScript.SellItem (this);
	}

	public void BuyItem ()
	{
		//storeControllerScript.BuyItem (this);
	}

}
