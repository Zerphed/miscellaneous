using UnityEngine;
using System.Collections;

public class PlayerHUDController : MonoBehaviour 
{
	public TextMesh		healthText3D;			// TextMesh for the Health in the HUD
	public TextMesh		scoreText3D;			// TextMesh for current score
	public TextMesh 	timeText3D;				// TextMesh for the Time in the HUD
	public TextMesh 	leftWheelPowerText3D;	// TextMesh for the right wheel power in the HUD
	public TextMesh 	rightWheelPowerText3D;	// TextMesh for the left wheel power in the HUD
	public TextMesh		informationText3D;		// TextMesh for temporary information text	
	public float 		defaultInformationDisplayTime;	// Time the temporary information text is displayed in seconds

	private float 		informationDisplayTime;
	private float 		informationStartTime;
	private bool		informationTimerStarted;

	private float 		startTime;				// This is the starting time of the game


	// Use this for initialization
	void Start () 
	{
		if (scoreText3D == null) 
		{
			throw new System.ArgumentException("You must set the healthText3D to a non null value", "healthText3D");
		}
		if (healthText3D == null) 
		{
			throw new System.ArgumentException("You must set the timeText3D to a non null value", "scoreText3D");
		}
		if (timeText3D == null) 
		{
			throw new System.ArgumentException("You must set the timeText3D to a non null value", "timeText3D");
		}
		if (leftWheelPowerText3D == null) 
		{
			throw new System.ArgumentException("You must set the leftWheelPowerText3D to a non null value", "leftWheelPowerText3D");
		}
		if (rightWheelPowerText3D == null) 
		{
			throw new System.ArgumentException("You must set the rightWheelPowerText3D to a non null value", "rightWheelPowerText3D");
		}
		if (informationText3D == null) 
		{
			throw new System.ArgumentException("You must set the informationText3D to a non null value", "informationText3D");
		}

		startTime = Time.time;
		healthText3D.text = "Health: " + ((PlayerController.getHp () / PlayerController.getMaxHp ())*100.0f).ToString("F0") + " %";
		scoreText3D.text = "Score: " + PlayerController.getScore().ToString() + " / " + PlayerController.getScoreRequiredToWin();
		timeText3D.text = "Time: 0" + " sec";
		leftWheelPowerText3D.text = "Left wheel power: 0%";
		rightWheelPowerText3D.text = "Right wheel power: 0%";
	}

	// Update is called once per frame
	void Update () 
	{
		int time = (int)(Time.time - startTime);

		if (informationTimerStarted && (Time.time - informationStartTime > informationDisplayTime)) 
		{
			informationTimerStarted = false;
			informationText3D.text = "";
		}

		healthText3D.text = "Health: " + ((PlayerController.getHp () / PlayerController.getMaxHp ())*100.0f).ToString("F0") + " %";
		scoreText3D.text = "Score: " + PlayerController.getScore().ToString() + " / " + PlayerController.getScoreRequiredToWin();
		timeText3D.text = "Time: " + (time > 60 ? ((time/60).ToString() + " min " + (time%60).ToString() + " sec") : (time.ToString() + " sec"));
		leftWheelPowerText3D.text = "Left wheel power: " + Mathf.Abs((Drive.torqueL / Drive.maxTorque) * 100).ToString("F2") + " %";
		rightWheelPowerText3D.text = "Right wheel power: " + Mathf.Abs((Drive.torqueR / Drive.maxTorque) * 100).ToString("F2") + " %";
	}

	public void displayInformation(string information, int time = 0)
	{
		informationDisplayTime = (time == 0 ? defaultInformationDisplayTime : time);
		if (!informationTimerStarted) 
		{
			informationStartTime = Time.time;
			informationTimerStarted = true;
			informationText3D.text = information;
		}
	}
}
