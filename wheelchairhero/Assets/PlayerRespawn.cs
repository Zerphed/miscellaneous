using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour 
{
	public TextMesh respawnText3DL;
	public TextMesh respawnText3DR;

	public float waitTime;				// In seconds
	public float fallTimeThreshold;		// In seconds
	public float groundLevelThreshold;	// Threshold when to consider the player in air
	
	private float fallStartTime;		// In seconds since game started
	private float respawnStartTime;
	private float groundLevel;			// The current ground level in the y-axis

	private bool timerStarted;
	private bool isGrounded;			// Is the player touching the ground?
	private bool isRespawning;			// Is the player currently in the respawning process?

	private Transform player; 			// The player's transform

	// Use this for initialization
	void Start () 
	{
		if (GameObject.FindGameObjectsWithTag ("Player").Length != 1) 
		{
			throw new System.ArgumentException("Invalid number of GameObjects with tag Player", "Player");
		}
		if (respawnText3DL == null) 
		{
			throw new System.ArgumentException("You must set the respawnText3D to a non null value", "respawnText3DL");
		}
		if (respawnText3DR == null) 
		{
			throw new System.ArgumentException("You must set the respawnText3D to a non null value", "respawnText3DR");
		}

		waitTime = 5.0f;
		fallTimeThreshold = 2.0f;
		groundLevelThreshold = 0.2f;

		timerStarted = false;
		isGrounded = false;
		isRespawning = false;

		respawnText3DL.text = "";
		respawnText3DR.text = "";

		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () 
	{
		float zRot = player.localRotation.eulerAngles.z;
		float xRot = player.localRotation.eulerAngles.x;

		bool isFallen = isGrounded && ((zRot >= 60.0f && zRot <= 320.0f) || (xRot >= 60.0f && xRot <= 320.0f));

		if (isRespawning) 
		{
			respawnText3DL.text = "Respawning in " + ((waitTime - (Time.time - respawnStartTime))).ToString("F1") + " seconds..";
			respawnText3DR.text = "Respawning in " + ((waitTime - (Time.time - respawnStartTime))).ToString("F1") + " seconds..";
		}

		// If the player just fell, start the threshold timer
		if (isFallen && !timerStarted) 
		{
			Debug.Log ("Start timer");
			fallStartTime = Time.time;
			timerStarted = true;
		}
		// The player has to be fallen for a certain threshold time period in order
		// to start the respawning process
		else if (isFallen && timerStarted && !isRespawning)
		{
			if (Time.time - fallStartTime > fallTimeThreshold)
			{
				Debug.Log ("Start the respawning process");
				isRespawning = true;
				StartCoroutine(Respawn ());
			}
		}
		// If the player is no longer fallen stop the respawning process
		else if (!isFallen && timerStarted)
		{
			Debug.Log ("Cancel respawn");
			timerStarted = false;
			isRespawning = false;
		}
	}

	public IEnumerator Respawn() 
	{
		Debug.Log ("Raspawning started");

		respawnStartTime = Time.time;

		// Wait for a set amount of time before respawning
		yield return new WaitForSeconds(waitTime);

		// Reset the transform
		player.localEulerAngles = new Vector3(0.0f, player.localEulerAngles.y, 0.0f);
		player.localPosition = new Vector3(player.localPosition.x, player.localPosition.y + 1.5f, player.localPosition.z);

		// You are no longer fallen
		isRespawning = false;
		isGrounded = false;
		respawnText3DL.text = "";
		respawnText3DR.text = "";

		Debug.Log ("Player has respawned");
	}

	public void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Ground") 
		{
			Debug.Log ("Collision enter called.");
			groundLevel = player.localPosition.y;
			isGrounded = true;
		}
	}

	public void OnCollisionStay(Collision collision) 
	{
		groundLevel = player.localPosition.y;
	}

	public void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ground" && player.localPosition.y > groundLevel+groundLevelThreshold) 
		{
				Debug.Log ("Collision exit called.");
				isGrounded = false;
		}
	}

}
