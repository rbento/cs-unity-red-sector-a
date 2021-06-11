using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour
{
	private Game game;
	private AudioSource asInvasion;
	private GUIText tip;
	
	void Awake ()
	{
		game = transform.parent.GetComponent<Game> ();
		asInvasion = GameObject.Find ("Invasion").audio;
		tip = GameObject.Find ("Tip").guiText;
	}

	public void Start ()
	{
		Level level = game.CurrentLevel;		
		
		transform.position = new Vector3(level.targetPoint.x,level.targetPoint.y + level.heightOffset, level.targetPoint.z);
	}
	
	void OnMouseEnter()
	{
		tip.text = "core";
	}
	
	void OnMouseExit()
	{
		tip.text = "";
	}	
	
	void OnCollisionEnter(Collision collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		
		if (null == enemy)
		{
			Physics.IgnoreCollision(gameObject.collider, collision.collider);
			
			return;
		}
		
		Debug.Log ("[TargetPoint] OnCollisionEnter - AN ENEMY HAS BREACHED THE SECURITY!!!");
		
		if (enemy.IsAlive)
		{		
			game.CurrentLevel.Shields--;
			asInvasion.Play();
			enemy.Die();
		}
	}	
}
