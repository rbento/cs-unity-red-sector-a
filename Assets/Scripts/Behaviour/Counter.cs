using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CounterAction();

public class Counter : MonoBehaviour
{
	public float timeout = 1;
	public bool isContinuous = false;
	
	private float endTime;
	private float timeLeft;
	
	private bool isPaused;
	private bool isRunning;
	private bool isSet;
	
	CounterAction action;	

	public void SetWith(float timeout)
	{
		SetWith(timeout, null, false);
	}
	
	public void SetWith(float timeout, CounterAction action)
	{
		SetWith(timeout, action, false);
	}
	
	public void SetWith(float timeout, CounterAction action, bool isContinuous)
	{
		this.timeout = timeout;
		this.action = action;
		this.isContinuous = isContinuous;
		this.isRunning = false;
		this.isSet = true;
	}
	
	public void Unset()
	{
		this.timeout = 0;
		this.action = null;
		this.isContinuous = false;
		this.isRunning = false;		
		this.endTime = 0;
		this.timeLeft = 0;
		this.isSet = false;
	}
	
	public void Start()
	{
		if (!isSet)
		{
			return;
		}
		
		endTime = Time.time + timeout;
		timeLeft = endTime - Time.time;
		isRunning = true;
	}
	
	public void Resume()
	{
		isPaused = false;
	}	
	
	public void Pause()
	{
		isPaused = true;
	}
	
	public void Stop()
	{
		isRunning = false;
	}
	
	public void Skip()
	{
		if (isRunning)
		{
			endTime = Time.time;
		}
	}

	public void Update()
	{
		if (!isRunning || isPaused)
		{
			return;
		}
		
		timeLeft = (endTime - Time.time);		
		
		if (timeLeft <= 0)
		{
			timeLeft = 0;
			
			if (isContinuous)
			{
				endTime = Time.time + timeout;
			}
			else
			{
				isRunning = false;
			}				
			
			if (null != action)
			{
				action();
			}			
		}
	}
	
	public float TimeLeft
	{
		get { return timeLeft; }
	}
	
	public bool IsRunning
	{
		get { return isRunning; }
	}
	
	public bool IsDone
	{
		get { return !isContinuous && (!isRunning || timeLeft == 0); }
	}
}
