using UnityEngine;
using System.Collections;

public class DoorOpenAnimation1 : MonoBehaviour {


	public float angle = 85.0f;

	private float targetAngle = 0.0f;
	private float currentAngle = 0.0f;
	private float smooth = 0.05f;

	public GameObject hinge1;
	public GameObject hinge2;

	//private AudioClip doorAudio;
	private bool open = false;
	private bool close = false;
	private int count = 0; 

	void Awake () 
	{
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Player" && (PlayerController.getScore() >= PlayerController.getScoreRequiredToWin())) 
		{
			targetAngle = angle;
			currentAngle = 0.0f;
			open = true;
		}
	}

	void  OnTriggerExit (Collider other) 
	{ 
		if (other.gameObject.tag == "Player" && (PlayerController.getScore() >= PlayerController.getScoreRequiredToWin())) 
		{
			currentAngle = angle;
			targetAngle = 0.0f;
			close = true;
			count = 0;
		}
	}

	void Update() 
	{
		currentAngle = currentAngle + (targetAngle - currentAngle) * smooth;
		hinge1.transform.rotation = Quaternion.identity; 
		hinge1.transform.Rotate(0, currentAngle, 0); 

		hinge2.transform.rotation = Quaternion.identity; 
		hinge2.transform.Rotate(0, -currentAngle, 0); 

		if (open == true && count == 0 && !audio.isPlaying) 
		{
			//audio.clip = doorAudio;
			audio.Play();
			open = false;
			count = 1;
		} 
		else if (close == true && !audio.isPlaying) 
		{
			//audio.clip = doorAudio;
			audio.Play();
			close = false;
		}

	}   

		
}
	

