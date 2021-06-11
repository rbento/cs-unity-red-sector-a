using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetFactory : ScriptableObject
{	
	private static string[] kTowerAssetPath = {"Prefabs/PhotonTower","Prefabs/BlastTower","Prefabs/RayTower"};
	
	public static GameObject CreatePrefab (string assetPath, Vector3 position)
	{		
		return Instantiate (Resources.Load (assetPath), position, Quaternion.identity) as GameObject;
	}
	
	public static Tower CreateTower (TowerType type, Vector3 position)
	{
		string assetPath = kTowerAssetPath[(int)type];
		
		GameObject prefab = CreatePrefab (assetPath, position);
		
		Tower result = null;
		
		switch(type)
		{
			case TowerType.kBlast :
			{
				result = prefab.GetComponent<BlastTower> ();
			}
			break;
			
			case TowerType.kPhoton :
			{
				result =  prefab.GetComponent<PhotonTower> ();
			}
			break;
			
			case TowerType.kRay :
			{
				result = prefab.GetComponent<RayTower> ();
			}
			break;			
		}
		
		return result;
	}
	
	public static Block CreateBlock (string assetPath, Vector3 position, Vector3 scale, BlockData blockData)
	{		
		GameObject prefab = CreatePrefab (assetPath, position);
		
		prefab.transform.localScale = scale;
		
		prefab.GetComponent<Block> ().blockedColor = blockData.blockedColor;
		prefab.GetComponent<Block> ().color = blockData.color;
		prefab.GetComponent<Block> ().depthIndex = blockData.depthIndex;
		prefab.GetComponent<Block> ().freeColor = blockData.freeColor;
		prefab.GetComponent<Block> ().state = blockData.state;
		prefab.GetComponent<Block> ().texture = blockData.texture;
		prefab.GetComponent<Block> ().widthIndex = blockData.widthIndex;
		
		prefab.GetComponent<BoxCollider> ().center = Vector3.zero;
		prefab.GetComponent<BoxCollider> ().size = Vector3.one;
		
		return prefab.GetComponent<Block> ();
	}
	
	public static Queue<Wave> CreateWave (JSONObject jsonObject)
	{
		JSONObject jsonWave = jsonObject.GetField ("wave");
		
		Queue<Wave> result = new Queue<Wave> ();
		
		foreach (JSONObject jsonWaveItem in jsonWave.list) {
			
			string prefab = jsonWaveItem.GetField ("prefab").str;
			
			int count = jsonWaveItem.GetField ("count").i;
			
			float delay = jsonWaveItem.GetField ("delay").f;
			float spawnInterval = jsonWaveItem.GetField ("spawnInterval").f;
			float speed = jsonWaveItem.GetField ("speed").f;
			
			result.Enqueue (new Wave (prefab, count, delay, spawnInterval, speed));
		}
		
		return result;
	}

	public static List<Block> CreateGrid (JSONObject jsonObject)
	{
		Debug.Log ("Creating grid from json object...");
		Debug.Log ("jsonObject: " + jsonObject);		
		
		Debug.Log ("Parsing json objects...");
		
		JSONParser jsonParser = new JSONParser (jsonObject);
		
		JSONObject jsonCustomBlocks = jsonParser.CustomBlocks;
		JSONObject jsonEmptyBlocks = jsonParser.EmptyBlocks;
		JSONObject jsonObstacles = jsonParser.Obstacles;
		
		JSONObject jsonSpawnPoint = jsonParser.SpawnPointIndex;
		JSONObject jsonTargetPoint = jsonParser.TargetPointIndex;
		
		Debug.Log ("Setting properties...");
		
		Vector3 gridCenter = jsonParser.GridCenter;		
		Debug.Log ("gridCenter: " + gridCenter);		
		
		int widthInBlocks = jsonParser.GridWidthInBlocks;
		int depthInBlocks = jsonParser.GridDepthInBlocks;
		
		Debug.Log ("widthInBlocks: " + widthInBlocks);		
		Debug.Log ("depthInBlocks: " + depthInBlocks);		
		
		float blockSize = jsonParser.BlockSize;
		float blockHeight = jsonParser.BlockHeight;
		
		Debug.Log ("blockSize: " + blockSize);		
		Debug.Log ("blockHeight: " + blockHeight);
	
		string blockAsset = jsonParser.BlockPrefab;
		Debug.Log ("blockAsset: " + blockAsset);
		
		float initialz = jsonParser.BlockInitialPosition.z;
		
		Vector3 blockPosition = jsonParser.BlockInitialPosition;
		Debug.Log ("blockPosition: " + blockPosition);
		
		Vector3 blockScale = jsonParser.BlockScale;
		Debug.Log ("blockScale: " + blockScale);
		
		Texture2D blockTexture = jsonParser.BlockTexture;
		Debug.Log ("blockTexture: " + blockTexture);
		
		Color blockColor = jsonParser.BlockColor;
		Debug.Log ("blockColor: " + blockColor);
		
		Color blockFreeColor = jsonParser.BlockFreeColor;		
		Debug.Log ("blockFreeColor: " + blockFreeColor);
		
		Color blockBlockedColor = jsonParser.BlockBlockedColor;			
		Debug.Log ("blockBlockedColor: " + blockBlockedColor);
		
		List<Block> result = new List<Block> ();
		
		for (int w = 0; w < widthInBlocks; ++w) {		
			
			for (int d = 0; d < depthInBlocks; ++d) {
				
				if (BlockIsEmpty (jsonEmptyBlocks, w, d)) {
					
					blockPosition.z += blockSize;
					continue;
				}
				
				BlockData blockData = new BlockData ();
				
				blockData.blockedColor = blockBlockedColor;
				blockData.color = blockColor;
				blockData.depthIndex = d;				
				blockData.freeColor = blockFreeColor;
				blockData.state = GetBlockState (jsonObstacles, jsonSpawnPoint, jsonTargetPoint, w, d);
				blockData.texture = blockTexture;
				blockData.widthIndex = w;
				
				if (BlockIsCustom (jsonCustomBlocks, w, d)) {
					
					foreach (JSONObject customBlock in jsonCustomBlocks.list) {
						
						JSONObject jsonIndex = customBlock.GetField ("index");
						
						if (MatchIndex (jsonIndex, w, d)) {
							
							Texture2D customTexture = Resources.Load (customBlock.GetField ("texture").str) as Texture2D;
							
							blockData.texture = customTexture;		
							
							result.Add (CreateBlock (blockAsset, blockPosition, blockScale, blockData));
						}					
					}					
					
					blockPosition.z += blockSize;
					continue;
				}				
				
				result.Add (CreateBlock (blockAsset, blockPosition, blockScale, blockData));
					
				blockPosition.z += blockSize;
			}
		
			blockPosition.z = initialz;
			blockPosition.x += blockSize;
		}
		
		Debug.Log ("grid count: " + result.Count);
		
		return result;
	}
	
	public static List<GameObject> CreateObstacles (JSONObject jsonObject)
	{
		Debug.Log ("Creating obstacles from json object...");
		Debug.Log ("jsonObject: " + jsonObject);		
		
		JSONParser jsonParser = new JSONParser (jsonObject);
		JSONObject jsonObstacles = jsonParser.Obstacles;
		
		List<GameObject> result = new List<GameObject> ();
		
		if (jsonObstacles.list.Count <= 0)		
		{
			return result;
		}
		
		int widthInBlocks = jsonParser.GridWidthInBlocks;
		int depthInBlocks = jsonParser.GridDepthInBlocks;
		
		Vector3 blockPosition = jsonParser.BlockInitialPosition;
		float initialz = jsonParser.BlockInitialPosition.z;
		
		float blockSize = jsonParser.BlockSize;
		float blockHeightByTwo = jsonParser.BlockHeightByTwo;		
		
		for (int w = 0; w < widthInBlocks; ++w) {
			
			for (int d = 0; d < depthInBlocks; ++d) {
				
				foreach (JSONObject obstacle in jsonObstacles.list) {
					
					JSONObject jsonIndex = obstacle.GetField ("index");
					
					if (MatchIndex (jsonIndex, w, d)) {
						JSONObject jsonPrefab = obstacle.GetField ("prefab");
						JSONObject jsonSize = obstacle.GetField ("size");
						JSONObject jsonHeight = obstacle.GetField ("height");
						
						Vector3 obstaclePosition = new Vector3 (blockPosition.x, blockPosition.y, blockPosition.z);
						obstaclePosition.y += blockHeightByTwo + (jsonHeight.f / 2.0f);
						
						GameObject prefab = CreatePrefab (jsonPrefab.str, obstaclePosition);
						
						Vector3 obstacleScale = new Vector3 (jsonSize.f, jsonHeight.f, jsonSize.f);
						prefab.transform.localScale = obstacleScale;
						
						prefab.layer = UnityEngine.LayerMask.NameToLayer ("Obstacle");
						
						prefab.GetComponent<BoxCollider> ().center = Vector3.zero;
						prefab.GetComponent<BoxCollider> ().size = Vector3.one;
						
						result.Add (prefab);
					}
				}
					
				blockPosition.z += blockSize;
			}
		
			blockPosition.z = initialz;
			blockPosition.x += blockSize;
		}
		
		Debug.Log ("obstacles count: " + result.Count);
		
		return result;
	}
	
	private static bool BlockIsEmpty (JSONObject emptyBlocks, int w, int d)
	{
		foreach (JSONObject emptyBlock in emptyBlocks.list) {
			
			JSONObject jsonIndex = emptyBlock.GetField ("index");
			
			if (MatchIndex (jsonIndex, w, d)) {
				return true;
			}
		}
		
		return false;
	}
	
	private static bool BlockHasObstacle (JSONObject obstacles, int w, int d)
	{
		foreach (JSONObject obstacle in obstacles.list) {
			
			JSONObject jsonIndex = obstacle.GetField ("index");
			
			if (MatchIndex (jsonIndex, w, d)) {
				return true;
			}
		}
		
		return false;
	}
	
	private static bool MatchIndex (JSONObject index, int w, int d)
	{
		int width = index.GetField ("w").i;
		int depth = index.GetField ("d").i;
		
		return (w == width && d == depth);
	}
	
	private static bool BlockIsCustom (JSONObject customBlocks, int w, int d)
	{
		foreach (JSONObject customBlock in customBlocks.list) {
			
			JSONObject jsonIndex = customBlock.GetField ("index");
			
			if (MatchIndex (jsonIndex, w, d)) {
				return true;
			}
		}

		return false;
	}
	
	private static BlockState GetBlockState (JSONObject obstacles, JSONObject spawnPoint, JSONObject targetPoint, int w, int d)
	{
		return BlockHasObstacle (obstacles, w, d) || MatchIndex (spawnPoint, w, d) || MatchIndex (targetPoint, w, d) ? BlockState.kBlocked : BlockState.kFree;
	}
}