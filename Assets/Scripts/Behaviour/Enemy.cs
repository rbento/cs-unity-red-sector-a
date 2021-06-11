using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public float health = 5.0f;
	public float speed;
	public float decay = 1.4f;
	public int prize = 100;
	public int space = 10;

	private bool isDying;
	private bool wasKilled;
	
	private Game game;	
	private Spawner spawner;
	
	void Start()
	{
		game = GameObject.Find("Game").GetComponent<Game>();
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
	}

	void OnDied()
	{
		if (wasKilled)
		{
			game.CurrentLevel.score += prize;		
			game.CurrentLevel.FreeSpace += space;
		}
		
		spawner.Enemies.Remove(this);		
		DestroyImmediate(gameObject);
	}

	public void Die()
	{		
		isDying = true;
		
		iTween.ColorTo(gameObject, iTween.Hash
		(
			"color", Color.cyan,
			"time", decay / 2.0f,
			"oncomplete", "Minimize"
		));	
	}		
	
	public void Minimize()
	{		
		iTween.ScaleTo(gameObject, iTween.Hash
		(
			"scale", Vector3.zero,
			"time", decay,
			"oncomplete", "OnDied"
		));	
	}		
	
	public void TakeDamage(float damage)
	{
		health -= damage;
		
		if (null != audio)
			audio.Play();
		
		if (health <= 0.0f)
		{
			this.wasKilled = true;
			
			Die ();
		}
	}
	
	public bool IsAlive
	{
		get { return !isDying && health > 0.0f; }
	}
}
