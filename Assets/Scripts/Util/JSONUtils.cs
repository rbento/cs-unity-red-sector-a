using System.Collections;
using System.IO;
using UnityEngine;

public class JSONUtils : ScriptableObject
{
	public static string StringFromAsset (string assetPath)
	{		
		if (string.IsNullOrEmpty(assetPath))
		{
			throw new System.ArgumentException("Trying to read a json string from an empty asset path.");
		}
		
		TextAsset textAsset = Resources.Load(assetPath) as TextAsset;		

		return textAsset.text;
	}
	
	public static JSONObject JSONObjectFromAsset (string assetPath)	
	{
		return JSONObjectFromString(StringFromAsset(assetPath));
	}
	
	public static JSONObject JSONObjectFromString (string jsonString)	
	{
		return new JSONObject(jsonString);
	}	
}
