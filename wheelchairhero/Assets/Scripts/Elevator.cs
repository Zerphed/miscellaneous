

using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
	public int 				stopTime;				// Time the elevator stays still in seconds
	public Rigidbody		rigidbody;
	
	public float            velocity;               // Velocity of the elevator
	public Vector3          startPosition;          // Starting position of the elevator
	public Vector3          endPosition;            // End position of the elevator

	private bool            timerStarted;           // Is the timer started?
	private float           stoppingTime;           // The timestamp when the elevator stopped
	private bool            moving;                 // Is the elevator moving?
	
	private Vector3         direction;              // Current movement direction
	private Vector3         directionToStart;       // Direction from end to start
	private Vector3         directionToEnd;         // Direction from start to end

	private float			distanceThreshold;

	void Start ()
	{
		if (startPosition.x == 0.0f && startPosition.y == 0.0f && startPosition.z == 0.0f) 
		{
			startPosition = transform.position;
		}
//		else
//		{
			//transform.position = transform.TransformPoint(new Vector3(startPosition.x, startPosition.y, startPosition.z));
//		}

		//endPosition = transform.TransformPoint (endPosition);

		timerStarted = false;
		moving = false;
		
		direction = Vector3.Normalize(endPosition - startPosition);
		directionToEnd = direction;
		directionToStart = -direction;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Start the timer we are close to stopping point
		// Also stop moving
		if ((Vector3.Distance (transform.position, endPosition) < distanceThreshold || 
			 Vector3.Distance (transform.position, startPosition) < distanceThreshold) && !timerStarted) 
		{
			moving = false;
		}
		if (!timerStarted && !moving)
		{
			if (Vector3.Distance(transform.position, endPosition) < distanceThreshold)
			{
				direction = directionToStart;
			}
			else if (Vector3.Distance(transform.position, startPosition) < distanceThreshold)
			{
				direction = directionToEnd;
			}
			
			stoppingTime = Time.time;
			timerStarted = true;
		}
		if (timerStarted && (Time.time - stoppingTime > stopTime))
		{
			timerStarted = false;
			moving = true;
		}
		
		if (moving)
		{
			// Move and update the distance threshold according to the last distance moved
			Vector3 oldPosition = transform.position;
			Vector3 movement = new Vector3(direction.x * Time.fixedDeltaTime * velocity,
			                               direction.y * Time.fixedDeltaTime * velocity,
			                               direction.z * Time.fixedDeltaTime * velocity);

			rigidbody.MovePosition(transform.position + movement);
			distanceThreshold = movement.magnitude * 1.5f;
		}
	}
}

