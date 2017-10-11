using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntryControllerScript : MonoBehaviour
{
	public Item item;
	public ItemControllerScript itemControllerScript;

	// Use this for initialization
	void Start ()
	{
		itemControllerScript = GameObject.FindObjectOfType<ItemControllerScript> ();
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
}
