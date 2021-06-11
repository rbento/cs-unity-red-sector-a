using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
	kNone = -1,
	kPhoton,
	kBlast,
	kRay 
}

public class Tower : MonoBehaviour
{
	protected TowerType type = TowerType.kNone;

	protected Game game;
	protected Spawner spawner;
	
	protected bool isSelected = false;
	protected bool isPlaced = false;	
	
	protected GameObject goBase;
	protected GameObject goCannon;
	protected GameObject goRange;
	protected GameObject goSelection;
	
	protected float nextAttackTime = 0.0f;
	
	protected const int maxUpgradeLevel = 3;
	public int cost = 10;	
	
	public Color color = Color.white;
	
	public string ammoAsset = "Prefabs/Photon";
	public Enemy target = null;
	
	public int upgradeLevel = 1;
	
	public float attackRate = 0.8f;
	public float attackRateDowngradeStep = 0.1f;
	public float attackPower = 100.0f;
	public float attackDamage = 10.0f;
	public float attackDamageUpgradeStep = 10.0f;
	public float attackPowerUpgradeStep = 10.0f;
	public float attackRange = 1.4f;
	public float attackRangeUpgradeStep = 0.3f;
	
	public bool isTransient;
	
	public virtual void  Awake()
	{
		game = GameObject.Find("Game").GetComponent<Game>();
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		
		goBase = transform.FindChild("Base").gameObject;
		goCannon = transform.FindChild("Cannon").gameObject;
		goRange = transform.FindChild("Range").gameObject;
		goSelection = transform.FindChild("Selection").gameObject;
	}
	
	public virtual void Start()
	{
		GetTransparent();	
		SetAttackRange ();
		
		isPlaced = false;
		isSelected = true;
		
		goRange.active = true;
		goSelection.active = false;
		
		gameObject.active = false;
	}
	
	void Update()
	{
		if (!isPlaced) 
		{
			return;
		}
		
		UpdateRange();
		
		AcquireTarget();
		
		Attack ();
	}
	
	void OnMouseOver()
	{		
		if (isTransient)
		{
			return;
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			OnLeftClick();
		}
		
		else if (Input.GetMouseButtonUp(1))
		{
			OnRightClick();
		}			
	}
	
	void OnMouseEnter ()
	{
		if (isTransient)
		{
			goSelection.active = false;
			
			return;
		}
		
		if (!game.TowerIsPicked && isPlaced && !isSelected) {
		
			goSelection.active = true;
		}
	}
	
	void OnMouseExit ()
	{
		if (isTransient)
		{
			goSelection.active = false;
			
			return;
		}
		
		if (!isPlaced) 
		{
			return;
		}
		
		goSelection.active = false;
	}
	
	void OnLeftClick ()
	{
		if (isTransient)
		{
			return;
		}
		
		if (!isPlaced)
		{
			return;
		}
		
		if (!isSelected)
		{
			Select();	
		}
		else
		{					
			Unselect();
		}
	}
	
	void OnRightClick ()
	{
		if (isTransient)
		{
			return;
		}
		
		Select ();
	}
	
	void UpdateRange()
	{
		if (isPlaced && isSelected)
		{
			goRange.active = true;
		}		
		
		else if (isPlaced && !isSelected)
		{
			goRange.active = false;
		}		
	}
	
	void UpdateColor()
	{
		string shaderName = isPlaced ? "Specular" : "Transparent/Diffuse";
		
		Shader shader = Shader.Find(shaderName);
		
		goBase.renderer.material.color = color;		
		goBase.renderer.material.shader = shader;
		
		goCannon.renderer.material.color = color;		
		goCannon.renderer.material.shader = shader;
		
		goRange.renderer.material.color = color;		
		goSelection.renderer.material.color = color;	
	}	

	public void Select()
	{
		if (game.TowerIsPicked)
		{
			Debug.Log ("[Tower] Select - Should place this tower before selecting another.");	
			return; 
		}
		
		if (game.TowerIsSelected)
		{
			game.SelectedTower.Unselect();
		}
		
		isSelected = true;
		goSelection.active = false;
		
		game.SelectedTower = this;
		
		Debug.Log ("[Tower] Select - game.SelectedTower: " + game.SelectedTower);
	}
	
	public void Unselect()
	{
		isSelected = false;
		goSelection.active = false;	
		game.SelectedTower = null;
	}	
	
	public void SetAttackRange()
	{
		float range = attackRange / 4.0f;
		
		Vector3 localScale = new Vector3(range, goRange.transform.localScale.y, range);		
		goRange.transform.localScale = localScale;
	}	
	
	public bool TargetIsInRange(Vector3 targetPosition)
	{
		float distance = Vector3.Distance(transform.position, targetPosition);				
		
		if (distance <= attackRange)			
		{	
			return true;
		}
		
		return false;
	}
	
	public void AcquireTarget()
	{		
		if (TargetIsAcquired)
		{
			if (TargetIsInRange(target.transform.position))
			{
				return;	
			}
			else
			{
				Debug.Log ("[AttackingTower] AcquireTarget - But we can't see it anymore, goodbye!"); 
				
				target = null;
			}
		}
		
		List<Enemy> enemies = spawner.Enemies;
			
		foreach (Enemy enemy in enemies)
		{
			if (null == enemy)
			{
				continue;
			}
			
			if (TargetIsInRange(enemy.transform.position) && enemy.IsAlive)			
			{
				target = enemy;
				
				Debug.Log ("[AttackingTower] AcquireTarget - TARGET AQCUIRED!!!!!!!!!!! " );
				
				return;
			}
		}
	}
	
	public virtual void Attack()
	{		
		if (!TargetIsAcquired || Time.time < nextAttackTime)
		{
			return;
		}		
		
		Vector3 direction = target.transform.position - goCannon.transform.position;		
		direction.Normalize();
		
		GameObject prefab = AssetFactory.CreatePrefab(ammoAsset, goCannon.transform.position);
		Ammo ammo = prefab.GetComponent<Ammo>();

		ammo.damage = attackDamage;
		ammo.rigidbody.AddForce(direction * attackPower);
		
		AudioSource.PlayClipAtPoint(ammo.audio.clip , goCannon.transform.position);
		
		nextAttackTime = Time.time + attackRate;
	}
	
	public void GetTransparent()
	{
		color.a = 0.2f;
		UpdateColor();
	}
	
	public void GetOpaque()
	{
		color.a = 1.0f;
		UpdateColor();
	}	
	
	public bool CanUpgrade()
	{
		return upgradeLevel < maxUpgradeLevel;
	}
	
	public void Upgrade()
	{
		if (CanUpgrade())
		{			
			attackDamage += attackDamageUpgradeStep;
			attackPower += attackPowerUpgradeStep;
			attackRange += attackRangeUpgradeStep;
			attackRate -= attackRateDowngradeStep;			
			upgradeLevel++;
			
			SetAttackRange ();
		}
	}
	
	public int UpgradeCost
	{
		get { return upgradeLevel * cost;}
	}
	
	public bool IsPlaced
	{
		get { return isPlaced; }
		set { isPlaced = value; }
	}

	public bool IsSelected
	{
		get { return isSelected; }
		set { isSelected = value; }
	}	
	
	public bool TargetIsAcquired
	{
		get { return null != target && null != target.gameObject && target.gameObject.active;  }
	}	
	
	public TowerType Type {
		get { return type; }
	}
}
