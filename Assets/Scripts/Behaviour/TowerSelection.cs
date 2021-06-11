using UnityEngine;
using System.Collections;

public class TowerSelection : MonoBehaviour {
	
	Tower tower;
	
	public float minRadius = 0.1f;
	public float maxRadius = 0.12f;
	public float smooth = 0.05f;
	
	float diffRadius;
	float size;
	Vector3 scale;
	
	void Awake()
	{
		tower = transform.parent.GetComponent<Tower>();
		
		Vector3 ls = transform.localScale;

		Vector3 localScale = new Vector3(minRadius, ls.y, minRadius);
		
		transform.localScale = localScale;				
		
		diffRadius = maxRadius - minRadius;
		size = minRadius;
		scale = new Vector3(size, 1, size);
	}

	void Update () {
		
		if (tower.IsPlaced && tower.IsSelected)
		{
			size = minRadius;
			
			scale.x = size;
			scale.z = size;
			
			return;
		}
		
		size = Mathf.PingPong(Time.time * smooth, diffRadius) + minRadius;

		scale.x = size;
		scale.z = size;
		
		transform.localScale = scale;	
	}
}
