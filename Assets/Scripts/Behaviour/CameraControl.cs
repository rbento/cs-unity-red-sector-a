using System.Collections;
using UnityEngine;

public enum ZoomType
{
	kIn, kOut
}

public class CameraControl : MonoBehaviour 
{	
	private Vector3 origin;
	private Vector3 target;
	
	public float damping = 6.0f;
	
	public float speed = 0.2f;
	
	public float lookTime = 1.2f;
	public float moveTime = 2.4f;	
	
	Vector3 zoomPosition;
	
	Vector3 leftPosition;
	Vector3 backPosition;
	Vector3 rightPosition;
	Vector3 frontPosition;
	Vector3 topPosition;
	
	Vector3 currentPosition;
	
	void Start()
	{			
		Game game = GameObject.Find ("Game").GetComponent<Game>();
		
		Vector3 gridCenter = game.CurrentLevel.gridCenter;
		origin = new Vector3(gridCenter.x, gridCenter.y, gridCenter.z);
		damping = (game.CurrentLevel.widthInBlocks + game.CurrentLevel.depthInBlocks) / 2.4f;
		
		Debug.Log ("[CameraControl] Awake - damping: " + damping);
		
		frontPosition = new Vector3(gridCenter.x, gridCenter.y + damping, gridCenter.z + damping);
		topPosition = new Vector3(gridCenter.x, gridCenter.y + damping, gridCenter.z);
		leftPosition = new Vector3(gridCenter.x - damping, gridCenter.y + damping, gridCenter.z);
		backPosition = new Vector3(gridCenter.x, gridCenter.y + damping, gridCenter.z - damping);
		rightPosition = new Vector3(gridCenter.x + damping, gridCenter.y + damping, gridCenter.z);
		
		Debug.Log ("[CameraControl] Awake - origin: " + origin);
		
		ResetTarget();
		
		Front();
	}
		
	void Update () 
	{	
		iTween.LookUpdate(Camera.main.gameObject, target, 5.0f);
	}
				
	public void MoveTo(Vector3 position)
	{
		iTween.MoveTo (Camera.main.gameObject, iTween.Hash
		(
			"position", position,
			"looktarget", target,
			"looktime", lookTime,
			"time", moveTime
		));			
	}
	
	public void ZoomOut()
	{		
		float dampingByTwo = damping / 2.0f;
		
		float xsign = Mathf.Sign(currentPosition.x);
		float x = currentPosition.x + (xsign > 0 ? dampingByTwo : xsign < 0 ? -dampingByTwo : 0);
		
		float ysign = Mathf.Sign(currentPosition.y);
		float y = currentPosition.y + (ysign > 0 ? dampingByTwo : ysign < 0 ? -dampingByTwo : 0);
		
		float zsign = Mathf.Sign(currentPosition.z);
		float z = currentPosition.z + (zsign > 0 ? dampingByTwo : zsign < 0 ? -dampingByTwo : 0);		
		
		Vector3 zoom = new Vector3(x, y, z);
		
		ResetTarget();
		MoveTo (zoom);		
	}
	
	public void ZoomIn()
	{
		float dampingByTwo = damping / 2.0f;
		
		float xsign = Mathf.Sign(currentPosition.x);
		float x = currentPosition.x + (xsign > 0 ? -dampingByTwo : xsign < 0 ? dampingByTwo : 0);
		
		float ysign = Mathf.Sign(currentPosition.y);
		float y = currentPosition.y + (ysign > 0 ? -dampingByTwo : ysign < 0 ? dampingByTwo : 0);
		
		float zsign = Mathf.Sign(currentPosition.z);
		float z = currentPosition.z + (zsign > 0 ? -dampingByTwo : zsign < 0 ? dampingByTwo : 0);		
		
		Vector3 zoom = new Vector3(x, y, z);
		
		ResetTarget();
		MoveTo (zoom);		
	}	
	
	public void Front()
	{
		ResetTarget();
		currentPosition = frontPosition;
		MoveTo(currentPosition);
	}		
	
	public void Left()
	{
		ResetTarget();
		currentPosition = leftPosition;
		MoveTo(currentPosition);
	}	
	
	public void Back()
	{
		ResetTarget();
		currentPosition = backPosition;
		MoveTo(currentPosition);
	}	
	
	public void Right()
	{
		ResetTarget();
		currentPosition = rightPosition;
		MoveTo(currentPosition);
	}	
	
	public void Top()
	{
		ResetTarget();
		currentPosition = topPosition;
		MoveTo(currentPosition);
	}	
	
	public void ResetTarget()
	{
		target = origin;
	}	
	
	public void Focus(GameObject go)
	{
		target = go.transform.position;
	}
}
