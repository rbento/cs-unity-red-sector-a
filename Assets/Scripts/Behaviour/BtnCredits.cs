using UnityEngine;
using System.Collections;

public class BtnCredits : Button {
	
	private GameObject options;
	private Fade optionsFx;
	
	private GameObject credits;
	private Fade creditsFx;

	void Awake()
	{
		options = GameObject.Find("Options");
		optionsFx = options.GetComponent<Fade>();

		credits = GameObject.Find("Credits");
		creditsFx = credits.GetComponent<Fade>();
	}

	protected override void OnClick()
	{
		GUIHelper.SendToBack(options);
		
		credits.SetActiveRecursively(true);
		GUIHelper.BringToFront(credits);

		optionsFx.FadeOut(1.0f, 0.0f, "OnOptionsHidden", gameObject);
	}

	void OnOptionsHidden()
	{
		creditsFx.FadeIn(1.0f, 1.0f);
		options.SetActiveRecursively(false);
	}
}
