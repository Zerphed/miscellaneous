using UnityEngine;
using System.Collections;

public class CoinPickUp : MonoBehaviour {
	
	
	private bool hit;

	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player") {
			hit = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hit && !audio.isPlaying) {
			audio.Play();
			hit = false;
		}
		
	}
}
