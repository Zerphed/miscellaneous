using UnityEngine;
using System.Collections;

public class SplashGameOverGUI : MonoBehaviour {
	
	public GUISkin customSkin = null;
	static public int buttonWidth = 300;
	static public int buttonHeight = 100;
	public int buttonPositionX = (Screen.width/2) - buttonWidth/2;
	public int buttonPositionY = (Screen.height/2) - buttonHeight/2;
	
	public void OnGUI() 
	{
		if (customSkin != null) 
		{
			GUI.skin = customSkin;
		}

		if (GUI.Button (new Rect (buttonPositionX, buttonPositionY, buttonWidth, buttonHeight), "Play again")) 
		{
			Application.LoadLevel ("Initial");		
		}
	}
}
