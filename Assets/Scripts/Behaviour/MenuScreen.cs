using System.Collections;
using UnityEngine;

public class MenuScreen : GameScreen 
{
	private GameObject mainMenu;
	private Fade titleFx;
	private Fade optionsFx;
	private Fade creditsFx;
	private Fade infoFx;
	
	IEnumerator Start()
	{
		mainMenu = GameObject.Find ("MainMenu");
		titleFx = GameObject.Find("Title").GetComponent<Fade>();
		optionsFx = GameObject.Find("Options").GetComponent<Fade>();
		creditsFx = GameObject.Find("Credits").GetComponent<Fade>();
		infoFx = GameObject.Find("Info").GetComponent<Fade>();
		
		yield return new WaitForSeconds(2.0f);
		
		infoFx.gameObject.SetActiveRecursively(false);
		creditsFx.gameObject.SetActiveRecursively(false);
		
		mainMenu.audio.Stop();
		mainMenu.audio.loop = true;
		mainMenu.audio.volume = 0.8f;
		mainMenu.audio.Play ();		
	
		GameObject.Find("ScreenCover").active = false;
		Camera.main.GetComponent<GUILayer>().enabled = true;
		FadeIn(5.0f, 1.0f, "OnScreenDisplayed", gameObject);
	}	
	
	public void OnScreenDisplayed()
	{
		titleFx.FadeTo(0.5f, 3.0f, 1.0f, "OnTitleDisplayed", gameObject);
	}
	
	public void OnTitleDisplayed()
	{
		optionsFx.FadeIn(2.0f, 1.0f, "OnOptionsDisplayed", gameObject);
	}	
	
	public void OnOptionsDisplayed()
	{		
		iTween.CameraFadeDestroy();
		Debug.Log("Done!");
	}		
}