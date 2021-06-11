using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {
	
	public float smooth = 0.1f;
	
	void Update () {
		transform.Rotate (Random.rotation.eulerAngles * Time.deltaTime * smooth);
	}
}
