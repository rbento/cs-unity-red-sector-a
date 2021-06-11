using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public float damage = 10.0f;
	public float life = 1.4f;
	public float decay = 0.6f;
	
	void Start()
	{
		Die (life);
	}
	
	void OnCollisionEnter(Collision collision)
	{	
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		
		if (null == enemy)
		{
			Physics.IgnoreCollision(gameObject.collider, collision.collider);
			
			return;
		}
		
		Debug.Log ("[Ammo] OnCollisionEnter - COLLIDED WITH AN ENEMY!!!!!!");
		
		enemy.TakeDamage(damage);
		
		rigidbody.isKinematic = true;
		
		Die ();
	}

	void OnDied()
	{
		DestroyImmediate(gameObject);
	}

	void Die(float delay = 0.0f)
	{
		iTween.ScaleTo(gameObject, iTween.Hash
		(
			"scale", Vector3.zero,
			"delay", delay,
			"time", decay,
			"oncomplete", "OnDied"
		));	
		
		Light light = GetComponentInChildren<Light>();
   	    light.intensity = Mathf.SmoothStep(light.intensity, 0.0f, decay);		
	}
}
