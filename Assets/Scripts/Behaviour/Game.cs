using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	private Level currentLevel = new Level();
	protected static List<Tower> towers;
	
	private AudioSource asPlaceTower;
	private AudioSource asDeleteTower;
	private AudioSource asUpgradeTower;
	
	private GameObject gameButtons;
	private GameObject gamePauseMenu;
	private GameObject gameOverMenu;
	private GameObject gameStageCompleteMenu;
	private GameObject screenCover;
	
	private Hud hud;
	private CameraControl cameraControl;
	private Counter counter;
	private Counter hudCounter;
	private Seeker seeker;
	private Spawner spawner;
	private SpawnPoint spawnPoint;
	private TargetPoint targetPoint;
	private PlayScreen playScreen;
	private Tower selectedTower;
	
	private bool isStarted;
	private bool isPaused;
	private bool isStageComplete;
	private bool isGameOver;
	
	private float nextUpdateTime;
	
	private float musicVolume = 0.3f;

	void Awake ()
	{
		Debug.Log ("[Game] Awake");

		towers = new List<Tower>();
		
		asPlaceTower = GameObject.Find ("PlaceTower").audio;
		asDeleteTower = GameObject.Find ("DeleteTower").audio;
		asUpgradeTower = GameObject.Find ("UpgradeTower").audio;
		
		screenCover = GameObject.Find("ScreenCover");
		cameraControl = Camera.main.GetComponent<CameraControl>();
		playScreen = Camera.main.GetComponent<PlayScreen>();
		counter = GetComponent<Counter>();
		seeker = GetComponent<Seeker>();
		spawner = GetComponentInChildren<Spawner>();
		spawnPoint = GetComponentInChildren<SpawnPoint>();
		targetPoint = GetComponentInChildren<TargetPoint>();
		hud = GetComponentInChildren<Hud>();
		hudCounter = GameObject.Find ("Hud").GetComponent<Counter>();
		
		gameButtons = GameObject.Find ("Buttons");
		gamePauseMenu = GameObject.Find ("PauseMenu");		
		gameOverMenu = GameObject.Find ("GameOverMenu");		
		gameStageCompleteMenu = GameObject.Find ("StageCompleteMenu");
	}

	IEnumerator Start ()
	{
		Debug.Log ("[Game] Start");
			
		return Reset();
	}
	
	public IEnumerator Reset()
	{
		Debug.Log ("[Game] Reset");
			
		currentLevel = LevelManager.Load(LevelManager.CurrentLevel);
		
		spawnPoint.Start();
		targetPoint.Start();
		
		UpdateGraph ();		
		
		InitMusic ();		
		InitHud ();
		
		selectedTower = null;
		
		HidePauseMenu();
		HideGameOverMenu();
		HideStageCompleteMenu();
		
		isStarted = false;
		isPaused = false;		
		isStageComplete = false;
		
		currentLevel.score = LevelManager.CurrentScore;
		currentLevel.turn = Turn.None;
		
		gameButtons.SetActiveRecursively(true);
	
		playScreen.FadeIn(4.0f, 1.0f, "OnScreenDisplayed", gameObject);		
		
		yield return StartCoroutine(hud.ShowSecurityAlert());		
		yield return StartCoroutine(hud.ShowPlayerTurn());
		
		Begin ();			
	}
	
	void OnScreenDisplayed()
	{
		iTween.CameraFadeDestroy();
	}
	
	void OnGUI()
	{
		UpdateHud();				
	}

	void Update ()
	{
		HandleGlobalInput();
		
		if (!isStarted || isPaused || isGameOver || isStageComplete)
		{ 
			return;
		}
		
		HandleGameInput();
		
		UpdateLogic();		
		
		switch (currentLevel.turn)
		{
			case Turn.Enemy : 
			{
				Debug.Log("[Game] Update - spawner.Enemies.Count: " + spawner.Enemies.Count);
			
				if (spawner.HasSpawn && spawner.Enemies.Count == 0)
				{
					TriggerNextWave();
				
					currentLevel.turn = Turn.Player;
				
					StartCoroutine(hud.ShowMsg("player turn", 1.0f));
				
					SetMouseTransient(false);
				
					CancelTowerPlacement();
				
					SelectedTower = null;
				}
			}
			break;
			
			case Turn.Player : 
			{
				float timeLeft = counter.TimeLeft;
			
				if (timeLeft > 0)
				{
					currentLevel.nextWave = timeLeft;	
				}
				else
				{		
					currentLevel.nextWave = 0;
					currentLevel.turn  = Turn.Enemy;				
				
					StartCoroutine(hud.ShowMsg("enemy turn", 1.0f));
				
					SetMouseTransient(true);
					
					CancelTowerPlacement();
				
					SelectedTower = null;
				}				
			}
			break;			
		}
	}
	
	void HandleGameInput()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{			
			if (!isPaused)
			{
				Pause();
			}
			else
			{
				Resume();
			}
		}

		if (Input.GetMouseButtonUp (1))
		{
			CancelTowerPlacement();
		}
	}
	
	void HandleGlobalInput()
	{
		if (Input.GetKeyUp(KeyCode.M))
		{
			ToggleMusic ();
		}	

		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			cameraControl.ZoomIn();
		}	
		
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			cameraControl.ZoomOut();
		}		
		
		if (Input.GetKeyUp(KeyCode.Home))
		{
			cameraControl.Front();
		}			
		
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			cameraControl.Front();
		}	
		
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			cameraControl.Left();
		}	
		
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			cameraControl.Back();
		}	
		
		if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			cameraControl.Right();
		}	
		
		if (Input.GetKeyUp(KeyCode.Alpha5))
		{
			cameraControl.Top();
		}		
	}
	
	public void Begin()
	{
		currentLevel.turn = Turn.Player;
		
		StartGameTimer();
		TriggerNextWave();
		
		isStarted = true;
	}
	
	public void StartGameTimer()
	{
		hudCounter.SetWith(1.0f, UpdateTimer, true);
		hudCounter.Start();
	}

	public void TriggerNextWave()
	{
		counter.SetWith(NextWaveDelay, DeliverWave);
		counter.Start();		
	}		
	
	void DeliverWave()
	{
		if (currentLevel.wave.Count > 0)
		{
			Wave wave = currentLevel.wave.Dequeue();

			spawner.Spawn(wave, spawnPoint.transform.position);
		}
	}
	
	public void AdvanceWave()
	{
		counter.Skip();		
	}
	
	public void UpgradeTower()
	{
		if (!TowerIsSelected)
		{
			Debug.Log("[Game] UpgradeTower - Duh! Select a tower first.");
			
			return;
		}
		
		if (selectedTower.CanUpgrade())
		{
			currentLevel.FreeSpace -= selectedTower.cost;
			selectedTower.Upgrade();
			
			asUpgradeTower.Play();
		}
	}	
	
	public void DeleteTower()
	{
		if (!TowerIsSelected)
		{
			Debug.Log("[Game] DeleteTower - Duh! Select a tower first.");
			
			return;
		}
		
		towers.Remove(selectedTower);

		currentLevel.FreeSpace += Mathf.Abs(selectedTower.cost / 3);

		GameObject.DestroyImmediate(selectedTower.gameObject);
		
		selectedTower = null;
		
		UpdateGraph();		
		
		asDeleteTower.Play();
	}
	
	public void CancelTowerPlacement()
	{
		if (TowerIsPicked)
		{		
			GameObject.DestroyImmediate(selectedTower.gameObject);
			SelectedTower = null;
		}
	}
	
	public void PlaceTower()
	{
		selectedTower.gameObject.active = true;
		
		AstarPath.active.Scan();
		
		if (!PathHelper.WillBlockPath(currentLevel.spawnPoint, currentLevel.targetPoint))
		{
			selectedTower.GetOpaque();
			
			selectedTower.IsSelected = false;
			selectedTower.IsPlaced = true;	
			
			towers.Add (selectedTower);
			
			currentLevel.FreeSpace -= selectedTower.cost;
			
			asPlaceTower.Play();
			
			selectedTower = null;	
		}
		else
		{
			selectedTower.gameObject.active = false;
		}
		
		UpdateGraph();
	}
	
	public void PickTower(TowerType towerType)
	{
		if (TowerIsPicked)
		{
			Debug.Log("[Game] PickTower - A tower is already picked. Place it!");
			
			return;
		}
		
		else if (TowerIsSelected)
		{
			selectedTower.Unselect();
		}		
		
		selectedTower = AssetFactory.CreateTower(towerType, Camera.main.WorldToScreenPoint(Vector3.zero));
		
		selectedTower.GetTransparent();
	}
	
	private void UpdateGraph()
	{
		AstarPath.active.Scan();
		seeker.StartPath (currentLevel.spawnPoint, currentLevel.targetPoint);	
	}
	
	public void ToggleMusic()
	{
		audio.volume = (audio.volume == musicVolume) ? 0.0f : musicVolume;
	}
	
	void InitMusic ()
	{
		audio.Stop();
		audio.clip = currentLevel.soundtrack;
		audio.loop = true;
		audio.volume = musicVolume;
		audio.Play ();
	}
	
	public void UpdateTimer()
	{
		if (isStarted)
		{
			currentLevel.elapsedTime++;
		}
	}	
	
	public void UpdateHud()
	{
		hud.ElapsedTime = currentLevel.elapsedTime;
		hud.NextWave = currentLevel.nextWave;
		hud.Score = currentLevel.score;
		hud.FreeSpace = currentLevel.FreeSpace;
		hud.Shields = currentLevel.Shields;		
		hud.Turn = currentLevel.turn;
		hud.Wave = currentLevel.wave.Count;
	}

	void InitHud ()
	{
		hud.Stage = currentLevel.stage;
		hud.TotalWaves = currentLevel.wave.Count;
		hud.Wave = currentLevel.wave.Count;
		hud.Score = currentLevel.score;
		hud.FreeSpace = currentLevel.FreeSpace;
		hud.Shields = currentLevel.Shields;
		hud.Turn = currentLevel.turn;
	}
	
	public void Next()
	{
		Time.timeScale = 1;
		LevelManager.CurrentLevel++;
		LevelManager.CurrentScore = currentLevel.score;
		iTween.AudioTo(gameObject, 0.0f, 1.0f, 2.0f);
		playScreen.FadeOut(2.0f, 0.0f, "OnNext", gameObject);
	}
	
	IEnumerator OnNext()
	{
		audio.Stop ();
		Dump ();
		return Reset ();		
	}			
	
	public void Quit()
	{
		Time.timeScale = 1;
		iTween.AudioTo(gameObject, 0.0f, 1.0f, 2.0f);
		playScreen.FadeOut(2.0f, 0.0f, "OnQuit", gameObject);
	}

	void OnQuit()
	{
		screenCover.active = true;
		audio.Stop ();
		Dump ();
		Application.LoadLevel("Menu");
	}
	
	public void Retry()
	{
		LevelManager.CurrentScore = 0;
		Time.timeScale = 1;
		iTween.AudioTo(gameObject, 0.0f, 1.0f, 2.0f);
		playScreen.FadeOut(2.0f, 0.0f, "OnRetry", gameObject);		
	}	
	
	IEnumerator OnRetry()
	{
		audio.Stop ();
		Dump ();
		return Reset ();
	}

	public void Pause()
	{
		if (!isPaused)
		{
			isPaused = true;
			ShowPauseMenu();				
		}
	}

	public void Resume()
	{
		if (isPaused)
		{
			isPaused = false;
			HidePauseMenu();
		}
	}

	public void ShowPauseMenu()
	{
		if (isPaused)
		{
			CancelTowerPlacement();
			SelectedTower = null;
			gamePauseMenu.SetActiveRecursively(true);
			StartCoroutine(hud.ShowMsg("game paused"));			
			
			SetMouseTransient(true);
			gameButtons.SetActiveRecursively(false);
			Time.timeScale = 0;			
		}
	}

	public void HidePauseMenu()
	{
		hud.ClearMsg();
		gamePauseMenu.SetActiveRecursively(false);
		SetMouseTransient(false);
		gameButtons.SetActiveRecursively(true);
		Time.timeScale = 1;		
	}

	public void ShowStageCompleteMenu()
	{
		if (isStageComplete)
		{
			CancelTowerPlacement();
			SelectedTower = null;
			gameStageCompleteMenu.SetActiveRecursively(true);
			StartCoroutine(hud.ShowMsg("stage complete"));
			gameButtons.SetActiveRecursively(false);
			SetMouseTransient(true);
		}
	}	
	
	public void HideStageCompleteMenu()
	{
		gameStageCompleteMenu.SetActiveRecursively(false);
		SetMouseTransient(false);
		gameButtons.SetActiveRecursively(true);
		hud.ClearMsg();
	}		
	
	public void ShowGameOverMenu()
	{
		if (isGameOver)
		{
			CancelTowerPlacement();
			SelectedTower = null;			
			gameOverMenu.SetActiveRecursively(true);
			StartCoroutine(hud.ShowMsg("game over"));
			SetMouseTransient(true);
			gameButtons.SetActiveRecursively(false);
		}
	}
	
	public void HideGameOverMenu()
	{
		gameOverMenu.SetActiveRecursively(false);
		hud.ClearMsg();
		gameButtons.SetActiveRecursively(true);
		SetMouseTransient(false);
	}	
	
	void UpdateLogic()
	{
		if (currentLevel.Shields <= 0)
		{
			isGameOver = true;
			
			currentLevel.turn = Turn.None;
			
			hudCounter.Pause ();
			
			ShowGameOverMenu();
		}
		
		else if ((currentLevel.wave.Count <= 0 && spawner.HasFinishedSpawning && spawner.Enemies.Count <= 0) && currentLevel.Shields > 0)
		{
			isStageComplete = true;
			
			currentLevel.turn = Turn.None;
			
			hudCounter.Pause ();
			
			ShowStageCompleteMenu();
		}
	}
	
	void Dump()
	{
		CancelTowerPlacement();
		
		SelectedTower = null;
		
		spawner.Dump();
		
		if (null != counter)
		{
			counter.Stop();
			counter.Unset();
		}
		
		if (null != hudCounter)
		{
			hudCounter.Stop();
			hudCounter.Unset();
		}		
				
		if (null != currentLevel && null != currentLevel.obstacles)		
		{
			foreach (GameObject go in currentLevel.obstacles)
			{
				if (null != go)
					GameObject.DestroyImmediate(go);
			}
		}
		
		if (null != currentLevel && null != currentLevel.grid)
		{		
			foreach (Block b in currentLevel.grid)
			{
				if (null != b && null != b.gameObject)
					GameObject.DestroyImmediate(b.gameObject);
			}
		}
		
		if (null != towers)
		{		
			foreach (Tower t in towers)
			{
				if (null != t && null != t.gameObject)
					GameObject.DestroyImmediate(t.gameObject);
			}
		}
		
		currentLevel = new Level();
	}
	
	void SetMouseTransient(bool value)
	{
		if (null != towers)
		{
			foreach (Tower t in towers)
			{
				if (null != t)
				{
					t.isTransient = value;
				}
			}
		}
		
		if (null != currentLevel && null != currentLevel.grid)
		{		
			foreach (Block b in CurrentLevel.grid)
			{
				if (null != b)
				{
					b.isTransient = value;
				}
			}		
		}
	}
	
	private float NextWaveDelay 
	{
		get { return (currentLevel.wave.Count > 0) ? currentLevel.wave.Peek ().delay : 0.0f; }
	}	
	
	public bool TowerIsPicked
	{
		get { return (null != selectedTower && !selectedTower.IsPlaced && selectedTower.IsSelected); }
	}
	
	public bool TowerIsSelected
	{
		get { return (null != selectedTower && selectedTower.IsPlaced && selectedTower.IsSelected); }
	}	
	
	public List<Tower> Towers
	{
		get { return towers; }
	}	
	
	public Vector3[] CurrentPath
	{
		get { return seeker.GetCurrentPath().vectorPath;  }
	}	
	
	public Level CurrentLevel
	{
		get { return currentLevel; }
	}
	
	public Tower SelectedTower
	{
		get { return selectedTower; }
		set 
		{ 
			if (TowerIsSelected)
			{
				selectedTower.Unselect();
			}
			
			selectedTower = value;
		}
	}	
}