using UnityEngine;
using System.Collections;

public class GUIHelper : ScriptableObject
{	
	public static void BringToFront (GameObject go)
	{
		Vector3 gopos = go.transform.position;
		gopos.z = 0;	
		go.transform.position = gopos;
		
		GUITexture[] gts = go.GetComponentsInChildren<GUITexture>();
		
		foreach (GUITexture gt in gts)
		{
			Vector3 gtpos = gt.transform.position;
			gtpos.z = 0;	
			gt.transform.position = gtpos;
		}
	}
	
	public static void SendToBack (GameObject go)
	{
		Vector3 gopos = go.transform.position;
		gopos.z = -1;	
		go.transform.position = gopos;
		
		GUITexture[] gts = go.GetComponentsInChildren<GUITexture>();
		
		foreach (GUITexture gt in gts)
		{
			Vector3 gtpos = gt.transform.position;
			gtpos.z = -1;	
			gt.transform.position = gtpos;
		}
	}	
}
