using UnityEngine;
using System.Collections;

public class ObjectHitsGround : MonoBehaviour {


	private bool hit;

	// Use this for initialization
	void Start () {
		hit = false;
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.tag == "Ground") {
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
