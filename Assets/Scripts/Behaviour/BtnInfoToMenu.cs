using UnityEngine;
using System.Collections;

public class BtnInfoToMenu : Button
{	
	private GameObject options;
	private Fade optionsFx;
	
	private GameObject info;
	private Fade infoFx;
	
	void Awake ()
	{
		options = GameObject.Find("Options");
		optionsFx = options.GetComponent<Fade>();

		info = GameObject.Find("Info");
		infoFx = info.GetComponent<Fade>();
	}
	
	protected override void OnClick ()
	{
		options.SetActiveRecursively(true);
		GUIHelper.BringToFront(options);
		
		GUIHelper.SendToBack(info);		
		infoFx.FadeOut (1.0f, 0.0f, "OnInfoHidden", gameObject);
	}
	
	void OnInfoHidden ()
	{
		info.SetActiveRecursively(false);
		optionsFx.FadeIn (1.0f, 1.0f);
	}
}
