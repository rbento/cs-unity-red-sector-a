using UnityEngine;
using System.Collections;

public class BtnBlastTower : GameButton
{
	private int towerCost = 30;
	
	protected override void OnClick()
	{
		game.PickTower (TowerType.kBlast);
	}	
	
	void Update ()
	{
		isEnabled = (game.CurrentLevel.turn == Turn.Player && game.CurrentLevel.FreeSpace >= towerCost);	
		
		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}	
	
	protected new void OnMouseEnter()
	{
		tip.text = "blast tower";
		cost.text = "requires " + towerCost + " space units";
		
		base.OnMouseEnter();			
	}
}
