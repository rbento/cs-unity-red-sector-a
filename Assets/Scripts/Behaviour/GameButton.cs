using UnityEngine;
using System.Collections;

public class GameButton : Button {

	protected Game game;
	
	protected GUIText tip;	
	protected GUIText cost;
	
	void Awake ()
	{
		game = GameObject.Find ("Game").GetComponent<Game> ();
		
		tip = GameObject.Find ("Tip").guiText;
		cost = GameObject.Find ("Cost").guiText;
	}	
	
	protected new void OnMouseExit()
	{
		base.OnMouseExit();
		
		tip.text = "";
		cost.text = "";
	}	
}
