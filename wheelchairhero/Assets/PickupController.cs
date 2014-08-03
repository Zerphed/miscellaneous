using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour 
{

	public AudioClip 	collectSound;		// The sound to play when the object is picked up
	public int 			scorePerPickup;		// Score received from this pickup

	// Use this for initialization
	void Start () 
	{
		if (collectSound == null || scorePerPickup == null || transform.childCount != 1) 
		{
			throw new System.ArgumentException("Invalid initialization for Pickup", "Pickup");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Transform cube = transform.GetChild (0);
		cube.Rotate (new Vector3(15, 30, 45) * Time.deltaTime);
	}

	// Called when player touches trigger Collider
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
			Destroy(this.gameObject);
			PlayerController.addScore(scorePerPickup);
		}
	}
}
