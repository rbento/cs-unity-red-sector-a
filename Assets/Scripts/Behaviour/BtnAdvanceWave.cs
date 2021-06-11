using UnityEngine;
using System.Collections;

public class BtnAdvanceWave : GameButton
{
	protected override void OnClick()
	{
		game.AdvanceWave();
	}
	
	void Update ()
	{
		isEnabled = (game.CurrentLevel.turn == Turn.Player && game.CurrentLevel.wave.Count > 0);	

		guiTexture.color = isEnabled ? enabledColor : disabledColor;
	}		
	
	protected new void OnMouseEnter()
	{
		tip.text = "advance wave";
		
		base.OnMouseEnter();			
	}	
}
