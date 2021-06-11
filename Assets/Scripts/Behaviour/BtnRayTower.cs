using System.Collections;
using UnityEngine;

public class BtnRayTower : GameButton 
{	
	private int towerCost = 45;
	
	protected override void OnClick()
	{
		game.PickTower(TowerType.kRay);				
	}
	
	void Update ()
	{
		isEnabled = (game.CurrentLevel.turn == Turn.Player && game.CurrentLevel.FreeSpace >= towerCost);	
		
		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}	
	
	protected new void OnMouseEnter()
	{
		tip.text = "ray tower";
		cost.text = "requires " + towerCost + " space units";
		
		base.OnMouseEnter();			
	}	
}
