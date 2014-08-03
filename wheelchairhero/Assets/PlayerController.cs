using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// There will be only one instance of PlayerController
public class PlayerController : MonoBehaviour 
{
	private static int 		scoreToWin 		= 30;		// Score required to win
	private static float 	curHp 			= 50.0f;	// Current hp for the player
	private static float 	maxHp 			= 50.0f;	// Maximum hp for the player
	private static float 	damageCooldown 	= 0.15f;	// Damage cooldown time
	private static int 		score 			= 0;		// Player score

	// A pool to keep track of enemies and last time they gave damage
	private static Dictionary<int, float> damagePool = new Dictionary<int, float>();
	
	// Use this for initialization
	void Start () 
	{
		curHp = 50.0f;
		score = 0;
	}

	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider other) 
	{
		// Check winning condition, the player has to have enough points to win
		if (other.gameObject.tag == "Exit" && score >= scoreToWin) 
		{
			if (score >= scoreToWin) 
			{
				Application.LoadLevel("SplashWinScreen");
			}
			else 
			{
				PlayerHUDController[] HUDControllers = GameObject.FindObjectsOfType(typeof(PlayerHUDController)) as PlayerHUDController[];
				
				// Enter text to HUD that you need more points.
				foreach (PlayerHUDController controller in HUDControllers) 
				{
					controller.displayInformation("You need more points to win.");
				}
			}
		}
	}

	static public float getHp() 
	{
		return curHp;
	}

	static public void addHp(float heal)
	{
		curHp = Mathf.Min (curHp + heal, maxHp);
	}


	static public float getMaxHp()
	{
		return maxHp;
	}

	static public int getScore() 
	{
		return score;
	}

	static public int getScoreRequiredToWin () 
	{
		return scoreToWin;
	}

	static public void addScore(int add) 
	{
		score += add;
	}
	
	static public void applyDamage(int enemyId, float damage)
	{
		float time = Time.time;
		if (!damagePool.ContainsKey (enemyId)) 
		{
			// Add the current time stamp and apply the damage
			damagePool.Add (enemyId, time);
			curHp -= damage;
		}
		else if ((time - damagePool[enemyId]) >= damageCooldown)
		{
			// Overwrite the old timestamp and apply the damage
			damagePool[enemyId] = time;
			curHp -= damage;
		}

		// Check whether the player is dead, react accordingly
		if (curHp <= 0.0f)
		{
			curHp = 0.0f;
			Application.LoadLevel("SplashGameOverScreen");
		}
	}
	
}