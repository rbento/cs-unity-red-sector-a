using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour
{	
	private GUIText gtFreeSpace;
	private GUIText gtNextWave;
	private GUIText gtScore;
	private GUIText gtShields;
	private GUIText gtStage;
	private GUIText gtElapsedTime;
	private GUIText gtTurn;
	private GUIText gtWave;
	private GUIText gtMsg;
	
	private int totalWaves;

	void Awake ()
	{
		gtFreeSpace = GameObject.Find ("FreeSpace").guiText;
		gtNextWave = GameObject.Find ("NextWave").guiText;
		gtScore = GameObject.Find ("Score").guiText;
		gtShields = GameObject.Find ("Shields").guiText;	
		gtStage = GameObject.Find ("Stage").guiText;
		gtElapsedTime = GameObject.Find ("ElapsedTime").guiText;
		gtWave = GameObject.Find ("Wave").guiText;		
		
		gtTurn = GameObject.Find ("Turn").guiText;
		gtTurn.text = "";		
		
		gtMsg = GameObject.Find ("Msg").guiText;			
		gtMsg.gameObject.SetActiveRecursively(false);
	}
	
	public IEnumerator ShowSecurityAlert()
	{
		yield return new WaitForSeconds(7.0f);
		gtMsg.gameObject.SetActiveRecursively(true);
		gtMsg.text = "security alert";
		yield return new WaitForSeconds(2.0f);
		gtMsg.gameObject.SetActiveRecursively(false);
		yield return new WaitForSeconds(1.0f);
		gtMsg.gameObject.SetActiveRecursively(true);		
		gtMsg.text = "protect the core";
		yield return new WaitForSeconds(2.0f);
		gtMsg.gameObject.SetActiveRecursively(false);		
		yield return new WaitForSeconds(1.0f);
	}
	
	public IEnumerator ShowPlayerTurn()
	{
		return ShowMsg ("player turn", 2.0f);
	}
	
	public IEnumerator ShowEnemyTurn()
	{
		return ShowMsg ("enemy turn", 2.0f);
	}
	
	public void ClearMsg()
	{
		gtMsg.gameObject.SetActiveRecursively(true);
		gtMsg.text = "";		
		gtMsg.gameObject.SetActiveRecursively(false);
	}
	
	public IEnumerator ShowMsg(string msg, float duration = 0.0f)
	{
		gtMsg.gameObject.SetActiveRecursively(true);
		gtMsg.text = msg;
		
		yield return new WaitForSeconds(duration);
		
		if (duration > 0.0f)
		{		
			gtMsg.gameObject.SetActiveRecursively(false);
		}
	}	
	
	public int FreeSpace {
		set { gtFreeSpace.text = value.ToString (); }
	}
	
	public float NextWave {
		set { gtNextWave.text = string.Format ("{0:00}", value); }
	}
	
	public int Score {
		set { gtScore.text = value.ToString (); }
	}
	
	public int Shields {
		set { gtShields.text = value.ToString (); }
	}
	
	public int Stage {
		set { gtStage.text = value.ToString (); }
	}
	
	public int ElapsedTime {
		set { gtElapsedTime.text = value.ToString (); }
	}
	
	public int TotalWaves {
		set { totalWaves = value; }
	}
	
	public Turn Turn {
		set { gtTurn.text = (value == Turn.Player) ? "PLAYER TURN" : (value == Turn.Enemy) ? "ENEMY TURN" : ""; }
	}
	
	public int Wave {
		set { 
			int currentWave = (totalWaves - value);			
			gtWave.text = currentWave.ToString () + " / " + totalWaves.ToString (); 
		}
	}	
}
