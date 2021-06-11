using UnityEngine;
using System.Collections;

public class BtnGameQuit : GameButton
{	
	protected override void OnClick ()
	{
		game.Quit();
	}
}
