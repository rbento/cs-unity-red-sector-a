using UnityEngine;
using System.Collections;

public class BtnDelete : GameButton {
	
	protected override void OnClick()
	{
		if (isEnabled)
		{
			game.DeleteTower();
		}
	}
	
	void Update ()
	{
		isEnabled = (null != game 
			&& null != game.SelectedTower);			

		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}		
	
	protected new void OnMouseEnter()
	{
		tip.text = "delete tower";
		
		if (null != game && null != game.SelectedTower)
		{		
			cost.text = "recovers " + (game.SelectedTower.cost / 3) + " space units";
		}
		
		base.OnMouseEnter();		
	}	
}
