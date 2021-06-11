using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	private Game game;
	private Level level;
	private Counter counter;
	private Wave wave;
	private AudioSource asSpawn;
	private Vector3 spawnPoint;
	private int remaining;
	private bool hasSpawn;
	private bool hasFinishedSpawning;
	
	private List<Enemy> enemies;
	
	void Awake ()
	{
		game = transform.parent.GetComponent<Game> ();
		counter = gameObject.GetComponent<Counter> ();		
		
		asSpawn = GameObject.Find ("Spawn").audio;
	}

	void Start()
	{
		enemies = new List<Enemy>();
	}
	
	public void Dump()
	{
		if (null != counter)
		{
			counter.Stop();
			counter.Unset();
		}
		
		if (null != enemies)
		{
			foreach (Enemy e in enemies)
			{
				if (null != e && null != e.gameObject)
				{
					GameObject.DestroyImmediate(e.gameObject);
				}
			}
			
			enemies.Clear();
		}
	}

	public void Spawn (Wave wave, Vector3 spawnPoint)
	{
		Debug.Log ("[Spawner] Spawn");

		this.wave = wave;
		this.remaining = wave.count;
		this.spawnPoint = spawnPoint;

		enemies.Clear();
		
		hasSpawn = false;
		hasFinishedSpawning = false;

		counter.SetWith (wave.spawnInterval, SpawnCallback, true);
		counter.Start ();
	}

	public void SpawnCallback ()
	{
		Debug.Log ("[Spawner] SpawnCallback");
		
		GameObject prefab = AssetFactory.CreatePrefab (wave.prefab, spawnPoint);
		
		enemies.Add(prefab.GetComponent<Enemy>());
		
		PathFollow pathFollow = prefab.GetComponent<PathFollow> ();
		
		Level level = game.CurrentLevel;		
		float yOffset = (level.gridCenter.y + (level.blockHeight / 2.0f)) + level.heightOffset;

		pathFollow.yOffset = yOffset;
		pathFollow.speed = wave.speed;
		
		pathFollow.Follow(game.CurrentPath);
		
		AudioSource.PlayClipAtPoint(asSpawn.clip, spawnPoint);
		
		hasSpawn = true;
		
		remaining--;

		if (remaining == 0) 
		{
			counter.Stop ();	
			hasFinishedSpawning = true;
		}
	}
	
	public Counter Counter
	{
		get { return counter; }
	}
	
	public bool HasSpawn
	{
		get { return hasSpawn; }
	}	
	
	public bool HasFinishedSpawning
	{
		get { return hasFinishedSpawning; }
	}
	
	public bool IsSpawning
	{
		get { return remaining > 0 && counter.IsRunning && hasSpawn; }
	}

	public List<Enemy> Enemies
	{
		get { return enemies; }
	}
}

