using UnityEngine;
using System.Collections;

public class TeleportController : MonoBehaviour 
{

	public float 				rotationSpeed;
	public TeleportController 	exitLocation;

	private bool				fromAnotherTeleport;

	// Use this for initialization
	void Start () 
	{
		if (exitLocation == null) 
		{
			throw new System.ArgumentException("You must set the exitLocation to a non null value", "exitLocation");
		}

		fromAnotherTeleport = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround (transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
	}

	void OnTriggerEnter(Collider other) 
	{

		if (other.gameObject.tag == "Player" && !fromAnotherTeleport) 
		{
			exitLocation.fromAnotherTeleport = true;
			other.gameObject.transform.position = new Vector3(exitLocation.transform.position.x,
			                                                  exitLocation.transform.position.y+1.0f,
			                                                  exitLocation.transform.position.z);
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.gameObject.tag == "Player" && fromAnotherTeleport) 
		{
			fromAnotherTeleport = false;
		}
	}
}
