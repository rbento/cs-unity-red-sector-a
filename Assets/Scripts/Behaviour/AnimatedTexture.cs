using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour 
{
	public int animationTileX = 1;
	public int animationTileY = 1;
	public float framesPerSecond = 1.0f;
	
	private Vector2 size;
	
	void Start()
	{
	    // Size of every tile
	    size = new Vector2 (1.0f / animationTileX, 1.0f / animationTileY);		
	}
	
	// Update is called once per frame
	void Update () {

	    // Calculate index
	    int index = (int) (Time.time * framesPerSecond);
		
	    // Repeat when exhausting all frames
	    index = index % (animationTileX * animationTileY);
	    
	    // Split into horizontal and vertical index
	    int uIndex = index % animationTileX;
	    int vIndex = index / animationTileX;
	
	    // Build offset
	    // V coordinate is the bottom of the image in opengl so we need to invert.
	    Vector2 offset = new Vector2 (uIndex * size.x, 1.0f - size.y - vIndex * size.y);
	    
	    renderer.material.SetTextureOffset ("_MainTex", offset);
	    renderer.material.SetTextureScale ("_MainTex", size);		
	}
}
