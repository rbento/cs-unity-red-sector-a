using UnityEngine;
using System.Collections;

public class PhotonTower : Tower 
{	
	public override void Awake() 
	{
		base.Awake();
		
		type = TowerType.kPhoton;
	}	
}
