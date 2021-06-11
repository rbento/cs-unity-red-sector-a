using UnityEngine;
using System.Collections;

public class BtnGameResume : GameButton
{	
	protected override void OnClick ()
	{
		game.Resume();
	}
}
