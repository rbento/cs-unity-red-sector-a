using Pathfinding;
using System.Collections;
using UnityEngine;

public class LevelManager : ScriptableObject
{
	private static int currentLevel;
	private static int currentScore;
	
	private static string[] mLevelAssets = {"Data/level_01", "Data/level_02"};

	public static Level Load (int level)
	{
		Debug.Log ("[LevelManager] Load");
		
		JSONObject jsonObject = JSONUtils.JSONObjectFromAsset (mLevelAssets [level]);
		
		Debug.Log ("[LevelManager] Load - jsonObject: " + jsonObject);
		
		JSONParser jsonParser = new JSONParser (jsonObject);
		
		Debug.Log ("[LevelManager] Load - jsonParser: " + jsonParser);
		
		Level result = new Level ();

		result.gridCenter = jsonParser.GridCenter;
		result.widthInBlocks = jsonParser.GridWidthInBlocks;
		result.depthInBlocks = jsonParser.GridDepthInBlocks;		
		result.blockSize = jsonParser.BlockSize;	
		result.blockHeight = jsonParser.BlockHeight;	
		result.heightOffset = jsonParser.HeightOffset;
		result.soundtrack = jsonParser.Soundtrack;
		result.FreeSpace = jsonParser.FreeSpace;
		result.stage = jsonParser.Stage;
		result.Shields = jsonParser.Shields;

		result.grid = AssetFactory.CreateGrid (jsonObject);
		result.obstacles = AssetFactory.CreateObstacles (jsonObject);
		result.wave = AssetFactory.CreateWave (jsonObject);		
		
		Vector3 spwp = jsonParser.SpawnPoint;
		result.spawnPoint = new Vector3(spwp.x, spwp.y + jsonParser.BlockHeightByTwo, spwp.z);
		
		Vector3 tgtp = jsonParser.TargetPoint;
		result.targetPoint = new Vector3(tgtp.x, tgtp.y + jsonParser.BlockHeightByTwo, tgtp.z);

		SetupGridGraph (result.gridCenter, result.widthInBlocks, result.depthInBlocks, result.blockSize, result.blockHeight);
		
		return result;
	}
	
	public static void Unload (Level level)
	{
		Debug.Log ("[LevelManager] Unload");
		
		if (null != level) 
		{
			foreach (var go in level.obstacles) 
			{
				GameObject.DestroyObject (go);
			}
			
			foreach (var go in level.grid) 
			{
				GameObject.DestroyObject (go);
			}
		}
	}
	
	private static void SetupGridGraph (Vector3 gridCenter, int widthInBlocks, int depthInBlocks, float blockSize, float blockHeight)
	{
		Debug.Log ("[LevelManager] SetupGridGraph");
		
		float gx = gridCenter.x;
		float gy = gridCenter.y + (blockHeight / 2.0f) - 0.1f;
		float gz = gridCenter.z;
		
		// Get the currently active grid graph
		GridGraph graph = AstarPath.active.astarData.gridGraph;		
		
		int graphModifier = 3;
		
		// Grid dimension settings
		graph.width = widthInBlocks * graphModifier;
		graph.depth = depthInBlocks * graphModifier;
		graph.nodeSize = blockSize / graphModifier;
		graph.aspectRatio = 1.0f;
		graph.center.Set(gx, gy, gz);
		graph.rotation = Vector3.zero;
		graph.cutCorners = true;
		
		// Grid collision test settings
		graph.collision.collisionCheck = true;
		graph.collision.mask = 0;
		graph.collision.mask = 1 << UnityEngine.LayerMask.NameToLayer ("Obstacle");
		graph.collision.mask += 1 << UnityEngine.LayerMask.NameToLayer ("Tower");
		graph.collision.type = Pathfinding.ColliderType.Capsule;
		graph.collision.diameter = 1.5f;
		graph.collision.height = 2.0f;
		
		// Grid height test settings
		graph.collision.heightCheck = true;
		graph.collision.heightMask = 1 << UnityEngine.LayerMask.NameToLayer ("Ground");
		graph.collision.unwalkableWhenNoGround = true;		
		
		graph.UpdateSizeFromWidthDepth ();
		graph.Scan ();
	}
	
	public static int CurrentLevel {
		
		get {
			return currentLevel;
		}
		
		set {
			currentLevel = Mathf.Clamp(value, 0, mLevelAssets.Length - 1);
		}
	}
	
	public static int CurrentScore {
		
		get {
			return currentScore;
		}
		
		set {
			currentScore = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}
}

