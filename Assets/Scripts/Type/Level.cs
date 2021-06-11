using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
	None, Player, Enemy
}

public class Level
{
	public List<Block> grid = new List<Block>();
	public List<GameObject> obstacles = new List<GameObject>();
	public Queue<Wave> wave = new Queue<Wave>();
	
	public Turn turn = Turn.None;
	
	public int widthInBlocks;
	public int depthInBlocks;
	public Vector3 gridCenter;
	public float blockSize;
	public float blockHeight;
	
	public Vector3 spawnPoint;
	public Vector3 targetPoint;
	public float heightOffset;
	public AudioClip soundtrack;
	
	public int stage;
	private int freeSpace;
	private int shields;
	public int elapsedTime;
	public float nextWave;
	public int score;
	public float waveCount;
	public float currentWave;
	
	public int Shields
	{
		get { return shields; }
		set { shields = value; shields = Mathf.Clamp(shields, 0, int.MaxValue);}
	}
	
	public int FreeSpace
	{
		get { return freeSpace; }
		set { freeSpace = value; freeSpace = Mathf.Clamp(freeSpace, 0, int.MaxValue);}
	}
}
