using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
	private Game game;
	
	void Awake ()
	{
		game = transform.parent.GetComponent<Game> ();
	}
	
	public void Start ()
	{
		Level level = game.CurrentLevel;
		transform.position = new Vector3(level.spawnPoint.x,level.spawnPoint.y + level.heightOffset, level.spawnPoint.z);
	}
}
