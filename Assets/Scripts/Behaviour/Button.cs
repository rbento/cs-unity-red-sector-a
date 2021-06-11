using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
	private AudioSource asClick;	
	private AudioSource asHover;
	private AudioSource asDenied;
	
	public Color disabledColor = new Color (0.5f, 0.5f, 0.5f);
	public Color enabledColor = new Color (1.0f, 1.0f, 1.0f);
	public Color hoverColor = new Color (1.0f, 1.0f, 0.0f);
	
	public bool isEnabled = true;
	
	void Start()
	{
		asClick = GameObject.Find ("Click").audio;
		asHover = GameObject.Find ("Hover").audio;
		asDenied = GameObject.Find ("Denied").audio;
	}
	
	protected void OnMouseOver ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			if (isEnabled) 
			{
				OnClick ();
				
				if (null != asClick && null != asClick.audio)	
				{
					asClick.audio.Play();
				}
			}
			else
			{
				if (null != asDenied && null != asDenied.audio)		
				{
					asDenied.audio.Play();
				}
			}	
			
			if (null != guiTexture)
			{			
				guiTexture.color = isEnabled ? hoverColor : disabledColor;	
			}
		}
	}
	
	protected void OnMouseEnter ()
	{
		if (null != guiTexture)
		{			
			guiTexture.color = isEnabled ? hoverColor : disabledColor;	
		}
		
		if (isEnabled)
		{
			if (null != asHover && null != asHover.audio)		
			{
				asHover.audio.Play();			
			}
		}
		else
		{
			if (null != asDenied && null != asDenied.audio)		
			{
				asDenied.audio.Play();					
			}
		}
	}
	
	protected void OnMouseExit ()
	{
		if (null != guiTexture)
		{			
			guiTexture.color = isEnabled ? enabledColor : disabledColor;	
		}		
	}
	
	protected virtual void OnClick ()
	{
	}
}	

