using UnityEngine;
using System.Collections;

public class WheelColliderDirector : MonoBehaviour 
{

	public 	WheelCollider 	collider;
	public 	float 			angle;
	public 	int				rays;

	private float 			colliderRadius;
	private float			partAngle;

	// Use this for initialization
	void Start () 
	{
		colliderRadius = collider.radius * Mathf.Abs(collider.transform.localScale.y);
		partAngle = angle / rays;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.localRotation = Quaternion.Euler(new Vector3(0,90,0));
		Vector3 direction = -Vector3.up;
		for (int i = 0; i < rays; ++i) 
		{
			RaycastHit hit;
			Debug.DrawRay(transform.position, direction.normalized * 0.4f);
			if (Physics.Raycast(transform.position, direction, out hit, colliderRadius, 1<<9)) 
			{
				float rotationAngle = ((i%2 == 0) ? (partAngle * i) : (partAngle * -1.0f * i));
				transform.localRotation = Quaternion.Euler (new Vector3(rotationAngle, 90.0f, 0.0f));
				break;
			}
			direction = Quaternion.Euler(((i%2 == 0) ? (partAngle * i) : (partAngle * -1.0f * i)), 0.0f, 0.0f) * -Vector3.up;
		}
	}
}
