using System;
using UnityEngine;



public class Core
{
	public static System.Random r = new System.Random ();

	public static int vary (int val, float variation)
	{
		var rVal = r.NextDouble ();
		return Mathf.RoundToInt ((float)val + ((float)val * variation * (float)rVal));
	}
}


