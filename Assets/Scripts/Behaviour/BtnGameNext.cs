using UnityEngine;
using System.Collections;

public class BtnGameNext : GameButton
{	
	protected override void OnClick ()
	{
		game.Next();
	}
}
