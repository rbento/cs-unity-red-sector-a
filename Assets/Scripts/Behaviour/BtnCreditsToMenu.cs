using UnityEngine;
using System.Collections;

public class BtnCreditsToMenu : Button
{	
	private GameObject options;
	private Fade optionsFx;
	
	private GameObject credits;
	private Fade creditsFx;
	
	void Awake ()
	{
		options = GameObject.Find("Options");
		optionsFx = options.GetComponent<Fade>();

		credits = GameObject.Find("Credits");
		creditsFx = credits.GetComponent<Fade>();
	}
	
	protected override void OnClick ()
	{
		options.SetActiveRecursively(true);
		GUIHelper.BringToFront(options);
		
		GUIHelper.SendToBack(credits);		
		creditsFx.FadeOut (1.0f, 0.0f, "OnCreditsHidden", gameObject);
	}
	
	void OnCreditsHidden ()
	{
		credits.SetActiveRecursively(false);
		optionsFx.FadeIn (1.0f, 1.0f);
	}
}
