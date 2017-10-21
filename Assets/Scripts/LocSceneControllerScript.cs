using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocSceneControllerScript : MonoBehaviour
{


	public Text LocText;

	// Use this for initialization
	void Start ()
	{
		Input.location.Start ();
	}

	private void GetLocation ()
	{
		 
	}
	
	// Update is called once per frame
	void Update ()
	{
		var displayText = Input.location.isEnabledByUser.ToString ();
		var status = Input.location.status;
		displayText += status;

		if (status == LocationServiceStatus.Running) {
			displayText += Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
		}

		LocText.text = displayText;
	}
}
