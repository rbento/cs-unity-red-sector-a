using UnityEngine;
using System.Collections;

public class BtnPlay : Button {
	
	private GameObject mainMenu;
	private MenuScreen menuScreen;
	
	void Awake()
	{
		mainMenu = GameObject.Find ("MainMenu");
		menuScreen = GameObject.Find("MainCamera").GetComponent<MenuScreen>();
	}

	protected override void OnClick()
	{
		LevelManager.CurrentLevel = 0;
		LevelManager.CurrentScore = 0;
		
		menuScreen.FadeOut(2.0f, 0.0f, "OnMenuScreenHidden", gameObject);
		
		iTween.AudioTo(mainMenu, iTween.Hash("audiosource", mainMenu.audio,"volume", 0.0f, "pitch", 1.0f, "time", 2.0f, "looptype", iTween.LoopType.none));
	}
	
	public void OnMenuScreenHidden()
	{
		Application.LoadLevel("Game");
	}
}
