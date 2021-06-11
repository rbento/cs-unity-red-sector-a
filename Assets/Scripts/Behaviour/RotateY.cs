using UnityEngine;
using System.Collections;

public class RotateY : MonoBehaviour {
	
	public float smooth = 2.0f;

	void Update () {
		
		transform.Rotate(0,-90 * Time.deltaTime * smooth, 0);
	}
}
