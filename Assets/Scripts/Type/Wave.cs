
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave 
{
	public string prefab;
	public int count;
	public float delay;
	public float spawnInterval;
	public float speed;
	
	public Wave() {}
	
	public Wave(string prefab, int count, float delay, float spawnInterval, float speed) 
	{
		this.prefab = prefab;
		this.count = count;
		this.delay = delay;
		this.spawnInterval = spawnInterval;
		this.speed = speed;
	}
}