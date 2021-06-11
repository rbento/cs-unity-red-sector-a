using System.Collections;
using UnityEngine;

public class JSONParser
{
	private JSONObject jsonObject;
	
	public JSONParser (string assetPath)
	{
		this.jsonObject = JSONUtils.JSONObjectFromAsset (assetPath);
	}
	
	public JSONParser (JSONObject jsonObject)
	{
		this.jsonObject = jsonObject;
	}
	
	public Vector3 GridCenter {
		get {
			JSONObject jsonPosition = jsonObject.GetField ("center");
		
			float x = jsonPosition.GetField ("x").f;
			float y = jsonPosition.GetField ("y").f;
			float z = jsonPosition.GetField ("z").f;
		
			return new Vector3 (x, y, z);		
		}
	}
	
	public int GridWidthInBlocks {
		get {
			return jsonObject.GetField ("width").i;
		}
	}
	
	public int GridDepthInBlocks {
		get {
			return jsonObject.GetField ("depth").i;
		}
	}
	
	public float HeightOffset {
		get {
			return jsonObject.GetField ("heightOffset").f;
		}
	}	
	
	public int MaxTowerCount {
		get {
			return jsonObject.GetField ("maxTowerCount").i;
		}
	}
	
	public int Stage {
		get {
			return jsonObject.GetField ("stage").i;
		}
	}	
	
	public int Shields {
		get {
			return jsonObject.GetField ("shields").i;
		}
	}	
	
	public int FreeSpace {
		get {
			return jsonObject.GetField ("freeSpace").i;
		}
	}		
	
	public AudioClip Soundtrack {
		get {
			return Resources.Load (jsonObject.GetField ("soundtrack").str, typeof(AudioClip)) as AudioClip;
		}
	}
	
	public string BlockPrefab {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			return jsonBlock.GetField ("prefab").str;
		}
	}
		
	public Texture2D BlockTexture {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			return Resources.Load (jsonBlock.GetField ("texture").str, typeof(Texture2D)) as Texture2D;
		}
	}
	
	public float BlockSize {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			return jsonBlock.GetField ("size").f;
		}
	}
	
	public float BlockSizeByTwo {
		get {
			return BlockSize / 2.0f;
		}
	}
	
	public float BlockHeight {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			return jsonBlock.GetField ("height").f;
		}
	}
	
	public float BlockHeightByTwo {
		get {
			return BlockHeight / 2.0f;
		}
	}
	
	public Vector3 BlockInitialPosition {
		get {
			float offsetx = (GridWidthInBlocks * BlockSizeByTwo) - BlockSizeByTwo;
			float offsetz = (GridDepthInBlocks * BlockSizeByTwo) - BlockSizeByTwo;

			Vector3 gc = GridCenter;
			
			return new Vector3 (gc.x - offsetx, gc.y, gc.z - offsetz);	
		}
	}
	
	public Vector3 BlockScale {
		get {
			return new Vector3 (BlockSize, BlockHeight, BlockSize);
		}
	}
	
	public Color BlockColor {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			JSONObject jsonBlockColor = jsonBlock.GetField ("color");		
		
			return new Color (jsonBlockColor.GetField ("r").f,
						      jsonBlockColor.GetField ("g").f,
							  jsonBlockColor.GetField ("b").f,
	                          jsonBlockColor.GetField ("a").f);
		}
	}
	
	public Color BlockFreeColor {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			JSONObject jsonBlockFreeColor = jsonBlock.GetField ("freeColor");		
		
			return new Color (jsonBlockFreeColor.GetField ("r").f,
						      jsonBlockFreeColor.GetField ("g").f,
							  jsonBlockFreeColor.GetField ("b").f,
	                          jsonBlockFreeColor.GetField ("a").f);
		}
	}
	
	public Color BlockBlockedColor {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			JSONObject jsonBlock = jsonGrid.GetField ("block");
			JSONObject jsonBlockBlockedColor = jsonBlock.GetField ("blockedColor");		
		
			return new Color (jsonBlockBlockedColor.GetField ("r").f,
						      jsonBlockBlockedColor.GetField ("g").f,
							  jsonBlockBlockedColor.GetField ("b").f,
	                          jsonBlockBlockedColor.GetField ("a").f);
		}
	}
	
	public JSONObject CustomBlocks {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");			
			return jsonGrid.GetField ("customBlocks");
		}
	}
	
	public JSONObject EmptyBlocks {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");	
			return jsonGrid.GetField ("emptyBlocks");
		}
	}
	
	public JSONObject Obstacles {
		get {
			JSONObject jsonGrid = jsonObject.GetField ("grid");
			return jsonGrid.GetField ("obstacles");
		}
	}
	
	public JSONObject Wave {
		get {
			return jsonObject.GetField ("wave");
		}
	}
	
	public Vector3 GetBlockPosition (int targetw, int targetd)
	{
		int widthInBlocks = GridWidthInBlocks;
		int depthInBlocks = GridDepthInBlocks;
		
		float blockSize = BlockSize;	
		
		Vector3 blockPosition = BlockInitialPosition;
		float initialz = BlockInitialPosition.z;		
		
		for (int w = 0; w < widthInBlocks; ++w) {
			for (int d = 0; d < depthInBlocks; ++d) {				
				if (w == targetw && d == targetd) {
					return new Vector3 (blockPosition.x, blockPosition.y, blockPosition.z);
				}
				
				blockPosition.z += blockSize;
			}
		
			blockPosition.z = initialz;
			blockPosition.x += blockSize;					
		}		
		
		return Vector3.zero;
	}
	
	public JSONObject SpawnPointIndex {
		get {		
			return jsonObject.GetField ("spawnPoint");
		}
	}	
	
	public Vector3 SpawnPoint {
		get {
			JSONObject jsonSpawnPoint = jsonObject.GetField ("spawnPoint");
		
			int w = jsonSpawnPoint.GetField ("w").i;
			int d = jsonSpawnPoint.GetField ("d").i;
			
			return GetBlockPosition (w, d);			
		}
	}
	
	public Vector3 TargetPoint {
		get {
			JSONObject jsonTargetPoint = jsonObject.GetField ("targetPoint");
		
			int w = jsonTargetPoint.GetField ("w").i;
			int d = jsonTargetPoint.GetField ("d").i;
			
			return GetBlockPosition (w, d);		
		}
	}	
	
	public JSONObject TargetPointIndex {
		get {		
			return jsonObject.GetField ("targetPoint");
		}
	}	
}

	