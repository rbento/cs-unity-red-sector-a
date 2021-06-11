using UnityEngine;
using System.Collections;

public class PlayScreen : GameScreen
{
	IEnumerator Start()
	{
		yield return new WaitForSeconds(2.0f);
		GameObject.Find("ScreenCover").active = false;
		Camera.main.GetComponent<GUILayer>().enabled = true;
		FadeIn(5.0f, 1.0f, "OnScreenDisplayed", gameObject);
	}
	
	public void OnScreenDisplayed()
	{
		iTween.CameraFadeDestroy();
		
		Debug.Log("Done!");
	}
}
