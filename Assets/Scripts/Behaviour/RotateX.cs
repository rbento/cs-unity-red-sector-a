using UnityEngine;
using System.Collections;

public class RotateX : MonoBehaviour {
	
	public float smooth = 2.0f;

	void Update () {
		
		transform.Rotate(90 * Time.deltaTime * smooth, 0, 0);
	}
}
