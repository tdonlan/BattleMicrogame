using System;
using UnityEngine;

public class Core
{
	public static System.Random r = new System.Random ();

	//uses variation to alter given value
	public static int vary (int val, float variation)
	{
		var rVal = r.NextDouble ();
		return Mathf.RoundToInt ((float)val + ((float)val * variation * (float)rVal));
	}
}