using UnityEngine;
using System.Collections;

public class NavMeshFollow : MonoBehaviour {
	
	public Transform 		target;
	NavMeshAgent 			agent;
	public float 			catchTreshhold;
	public float 			sensingRange;
	public float 			interestRange;
	public float 			forwardSpeed { get; private set; }
	public Animator 		animator;
	public Transform[] 		waypoints;
	public AudioClip		noticedClip;

	private int 			index;
	private bool 			chase = false;
	private bool 			patrolling;
	private Transform 		myTransform;
	private Transform 		currentTarget;
	private bool			noticed;

	private Vector3 originalPosition;
	private Quaternion originalRotation;

	// Use this for initialization
	void Start () 
	{
		originalPosition = transform.position;
		originalRotation = transform.rotation;
		currentTarget = target;
		index = 0;
		agent = GetComponent<NavMeshAgent> ();
		forwardSpeed = 0;
		patrolling = false;
	}

	void Reset ()
	{
		transform.position = originalPosition;
		transform.rotation = originalRotation;
	}

	// FixedUpdate is called at regular intervals regardless of frame rate
	void FixedUpdate () 
	{
		myTransform = transform;

		if (noticed && !audio.isPlaying) {
			AudioSource.PlayClipAtPoint (noticedClip, transform.position);
			noticed = false;
		}

		if (!chase && Vector3.Distance (myTransform.position, target.position) < sensingRange) 
		{
			currentTarget = target;
			chase = true;
			patrolling = false;
			noticed = true;
			MusicWhenChased.chasers++;
		}
		if (chase && Vector3.Distance (myTransform.position, target.position) < interestRange) 
		{
			agent.SetDestination (target.position);
			forwardSpeed = agent.speed;
		} 
		else 
		{
			if (chase)
				MusicWhenChased.chasers--;
			chase = false;
			if ( !patrolling && waypoints.Length > 0)
			{
				patrolling = true;
				currentTarget = waypoints[index];
				agent.SetDestination (waypoints[index].position);
				index = (index + 1) % waypoints.Length;
				
				forwardSpeed = agent.speed;
			}
			else if (!patrolling)
			{
				forwardSpeed = 0;
				agent.SetDestination( transform.position);
			}
		}


		if (Vector3.Distance (myTransform.position, currentTarget.position) < catchTreshhold) 
		{
			//chase = false;
			if (patrolling)
			{		
				patrolling = false;
			}
			else 
			{
				PlayerController.applyDamage(this.GetInstanceID(), 1.0f);
			}	
		}
		animator.SetFloat("Speed", forwardSpeed);
		
	}
}