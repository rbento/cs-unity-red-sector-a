using UnityEngine;
using System.Collections;

public class BlastTower : Tower {

	public override void Awake() 
	{
		base.Awake();
		
		type = TowerType.kBlast;
	}	
}
