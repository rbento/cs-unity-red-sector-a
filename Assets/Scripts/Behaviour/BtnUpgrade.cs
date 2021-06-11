using UnityEngine;
using System.Collections;

public class BtnUpgrade : GameButton {	

	protected override void OnClick()
	{
		if (isEnabled)
		{
			game.UpgradeTower();
		}
	}
	
	void Update ()
	{
		isEnabled = (null != game 
			&& null != game.SelectedTower 
			&& game.SelectedTower.CanUpgrade() 
			&& game.CurrentLevel.FreeSpace >= game.SelectedTower.cost);

		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}		
	
	protected new void OnMouseEnter()
	{		
		if (null != game && null != game.SelectedTower && game.SelectedTower.upgradeLevel < 3)
		{
			tip.text = "upgrade to level " + (game.SelectedTower.upgradeLevel + 1);	
			cost.text = "requires " + game.SelectedTower.cost + " space units";
		}
		else if (null != game && null != game.SelectedTower && game.SelectedTower.upgradeLevel >= 3)
		{
			tip.text = "max level";
		}		
		else
		{
			tip.text = "upgrade tower";	
		}
		
		base.OnMouseEnter();		
	}		
}
