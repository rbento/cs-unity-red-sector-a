using UnityEngine;
using System.Collections;

public class BtnGameRetry : GameButton
{	
	protected override void OnClick ()
	{
		game.Retry();
	}
}
