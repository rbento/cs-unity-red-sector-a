	using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour 
{
	public bool hideOnStart = true;
	
	void Start()
	{
		if (hideOnStart)
		{			
			FadeOut (0, 0);
		}
	}
	
	public void FadeIn()	
	{
		FadeTo (1.0f, 1.0f, 0.0f, "", gameObject);
	}	
	
	public void FadeIn(float time)	
	{
		FadeTo (1.0f, time, 0.0f, "", gameObject);
	}	
	
	public void FadeIn(float time, float delay)	
	{
		FadeTo (1.0f, time, delay, "", gameObject);
	}	
	
	public void FadeIn(float time, float delay, string oncomplete)	
	{
		FadeTo (1.0f, time, delay, oncomplete, gameObject);
	}
	
	public void FadeIn(float time, float delay, string oncomplete, GameObject oncompletetarget)	
	{
		FadeTo (1.0f, time, delay, oncomplete, oncompletetarget);
	}	
	
	public void FadeTo(float alpha, float time, float delay, string oncomplete, GameObject oncompletetarget)
	{
		iTween.FadeTo(gameObject, iTween.Hash
		(
			"alpha", alpha,
			"includechildren", true,
			"time", time,
			"delay", delay,
			"oncompletetarget", oncompletetarget,
			"oncomplete", oncomplete	
		));
	}		
	
	public void FadeOut()	
	{
		FadeTo (0.0f, 1.0f, 0.0f, "", gameObject);
	}	
	
	public void FadeOut(float time)	
	{
		FadeTo (0.0f, time, 0.0f, "", gameObject);
	}	
	
	public void FadeOut(float time, float delay)	
	{
		FadeTo (0.0f, time, delay, "", gameObject);
	}	
	
	public void FadeOut(float time, float delay, string oncomplete)	
	{
		FadeTo (0.0f, time, delay, oncomplete, gameObject);
	}
	
	public void FadeOut(float time, float delay, string oncomplete, GameObject oncompletetarget)	
	{
		FadeTo (0.0f, time, delay, oncomplete, oncompletetarget);
	}		
}