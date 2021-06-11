using System.Collections;
using UnityEngine;

public enum BlockState
{
	kFree,
	kBlocked
}

public class Block : MonoBehaviour
{	
	private Game goGame;
	
	private AudioSource asFree;
	private AudioSource asDenied;
	
	public int widthIndex;
	public int depthIndex;
	
	public BlockState state;
	public Texture cursorBlockedTexture;
	public Texture cursorFreeTexture;
	public Texture texture;
	public Transform obstacle;
	public float width = 1.0f;
	public float height = 0.2f;
	public float depth = 1.0f;
	public Color color;
	public Color freeColor;
	public Color blockedColor;

	public bool isFocused;
	
	public bool isTransient;
	
	void Awake ()
	{
		goGame = GameObject.Find("Game").GetComponent<Game>();
		
		asDenied = GameObject.Find ("Denied").audio;		
		asFree = GameObject.Find ("Cursor").audio;
	}
	
	void Start ()
	{
		renderer.material.mainTexture = texture;
	}
	
	void OnMouseOver ()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (state == BlockState.kFree)
			{
				if (goGame.TowerIsPicked)
				{
					goGame.PlaceTower();
				}
				
				else if (goGame.TowerIsSelected)
				{
					goGame.SelectedTower.Unselect();	
				}
			}
		}
	}	
	
	void OnMouseEnter ()
	{
		if (isTransient)
		{
			return;
		}
		
		if (goGame.TowerIsPicked && state == BlockState.kFree)
		{
			Vector3 position = transform.position;			
			position.y += (goGame.CurrentLevel.blockHeight / 2.0f) + (goGame.SelectedTower.transform.localScale.y / 4.0f);
		
			goGame.SelectedTower.transform.position = position;
		}
		
		switch (state)
		{
			case BlockState.kFree :
			{
				renderer.material.color = freeColor;
				renderer.material.mainTexture = cursorFreeTexture;		
			
				asFree.Play();
			}
			break;
			
			case BlockState.kBlocked :
			{
				renderer.material.color = blockedColor;
				renderer.material.mainTexture = cursorBlockedTexture;	
			
				asDenied.Play();
			}
			break;
			
			default :
			{
				renderer.material.color = color;
				renderer.material.mainTexture = texture;				
			}
			break;			
		}
		
		isFocused = true;			
	}
	
	void OnMouseExit ()
	{
		renderer.material.color = color;
		renderer.material.mainTexture = texture;
		isFocused = false;
	}
}