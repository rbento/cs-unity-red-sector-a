using System.Collections;
using UnityEngine;

public class KeepAlive : MonoBehaviour
{
	void Start ()
	{
		DontDestroyOnLoad (this);	
	}
}
