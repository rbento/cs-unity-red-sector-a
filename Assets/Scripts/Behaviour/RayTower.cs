using UnityEngine;
using System.Collections;

public class RayTower : Tower 
{
	private LineRenderer lineRenderer;
	private Vector3[] ray;
	
	public float rayWidth = 0.05f;
	
	public override void Awake() 
	{
		base.Awake();
		
		lineRenderer = GetComponent<LineRenderer>();		
		
		type = TowerType.kRay;
	}
	
	public override void Start()
	{
		base.Start();
		
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetWidth(rayWidth, rayWidth);
	}
	
	public override void Attack()
	{
		if (!TargetIsAcquired || !target.IsAlive)
		{
			lineRenderer.enabled = false;
			
			nextAttackTime = Time.time + attackRate;
			
			audio.Stop();
			
			return;
		}
		
		lineRenderer.enabled = true;
		
		lineRenderer.SetPosition(0, goCannon.transform.position);
		lineRenderer.SetPosition(1, target.transform.position);
		
		if (!audio.isPlaying)
		{
			AudioSource.PlayClipAtPoint(audio.clip, target.transform.position);
		}

 		if (Time.time > nextAttackTime)
		{
			target.TakeDamage(attackDamage);
			
			nextAttackTime = Time.time + attackRate;
		}
	}
}
