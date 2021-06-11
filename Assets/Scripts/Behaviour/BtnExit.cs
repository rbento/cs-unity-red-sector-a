using UnityEngine;
using System.Collections;

public class BtnExit : Button {
	
	protected override void OnClick()
	{
		Application.Quit();
	}
}
