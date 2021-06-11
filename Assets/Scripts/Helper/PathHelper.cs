using Pathfinding;
using System.Collections;
using UnityEngine;

public class PathHelper : ScriptableObject
{
	public static Vector3[] HeightOffset(Vector3[] path, float yOffset)
	{
		int length = path.Length;
		
		Vector3[] result = new Vector3[length];		
		
		for (int i = 0; i < length; ++i)
		{	
			result[i] = new Vector3(path[i].x, yOffset, path[i].z);	
		}
		
		return result;
	}
	
	public static bool WillBlockPath(Vector3 start, Vector3 end)
	{
		Node node1 = AstarPath.active.GetNearest (start, NNConstraint.None, null);
		Node node2 = AstarPath.active.GetNearest (end, NNConstraint.None, null);				

		if (node1.area == node2.area) {
			
			return false;				
		}
		
		return true;
	}	
}

	