using System.Collections;
using UnityEngine;

public class BtnPhotonTower : GameButton
{
	private int towerCost = 15;
	
	protected override void OnClick()
	{
		game.PickTower(TowerType.kPhoton);
	}	
	
	void Update ()
	{
		isEnabled = (game.CurrentLevel.turn == Turn.Player && game.CurrentLevel.FreeSpace >= towerCost);	
		
		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}	
	
	protected new void OnMouseEnter()
	{
		tip.text = "photon tower";
		cost.text = "requires " + towerCost + " space units";
		
		base.OnMouseEnter();		
	}	
}
