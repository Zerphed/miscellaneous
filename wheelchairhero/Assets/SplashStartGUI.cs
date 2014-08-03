using UnityEngine;
using System.Collections;

public class SplashStartGUI : MonoBehaviour {
	
	public RUISPSMoveWand PSMoveController;
	
	void FixedUpdate() {
		if (Input.GetKeyDown("space") || this.PSMoveController.triggerValue > 0){
			Application.LoadLevel ("Initial");		
		}
	}
}
