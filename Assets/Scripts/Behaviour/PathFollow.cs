using Pathfinding;
using System.Collections;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
	public float baseHeight = 0.2f;
	public float heightOffset = 0.5f;
	
	public float yOffset  =0.5f;
	public float speed = 0.2f;

	public string axis = "y";
	public iTween.EaseType easetype = iTween.EaseType.easeOutSine;
	public float lookahead = 0.05f;
	public float looktime = 0.1f;
	public bool orienttopath = true;

	public void Follow (Vector3[] path)
	{
		if (null == path)
		{
			Debug.Log ("[PathFollow] OnPathCompleted");
			
			return;
		}
		
		iTween.MoveTo (gameObject, iTween.Hash
		(
			"path", PathHelper.HeightOffset (path, yOffset),
			"orienttopath", orienttopath,
			"looktime", looktime,
			"lookahead", lookahead,
			"axis", axis,
			"speed", speed,
			"easetype", easetype,
			"oncomplete", "OnPathCompleted"
		));			
	}
	
	public void OnPathCompleted ()
	{		
		Debug.Log ("[PathFollow] OnPathCompleted");
	}	
}
