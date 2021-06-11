using UnityEngine;
using System.Collections;

public class BtnInfo : Button {
	
	private GameObject options;
	private Fade optionsFx;
	
	private GameObject info;
	private Fade infoFx;

	void Awake()
	{
		options = GameObject.Find("Options");
		optionsFx = options.GetComponent<Fade>();

		info = GameObject.Find("Info");
		infoFx = info.GetComponent<Fade>();
	}

	protected override void OnClick()
	{
		GUIHelper.SendToBack(options);
		
		info.SetActiveRecursively(true);
		GUIHelper.BringToFront(info);

		optionsFx.FadeOut(1.0f, 0.0f, "OnOptionsHidden", gameObject);
	}

	void OnOptionsHidden()
	{
		infoFx.FadeIn(1.0f, 1.0f);
		options.SetActiveRecursively(false);
	}
}
