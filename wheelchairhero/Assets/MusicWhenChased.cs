using UnityEngine;
using System.Collections;

public class MusicWhenChased : MonoBehaviour {

	public static int chasers;
	// Use this for initialization
	void Start () {
		chasers = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (chasers > 0 && !audio.isPlaying) {
			audio.Play ();
		} 
		else if ( chasers == 0 )
			audio.Stop ();
	}
}
