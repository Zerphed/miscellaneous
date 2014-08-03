using UnityEngine;
using System.Collections;

public class SplashWinGUI : MonoBehaviour 
{
	static public int buttonWidth = 300;
	static public int buttonHeight = 100;
	static public int uppoScreenWidth = 1024;
	static public int oculusScreenWidth = 1280;
	static public int buttonPositionX = oculusScreenWidth + (uppoScreenWidth/2) - buttonWidth/2;
	static public int buttonPositionY = (Screen.height/2) - buttonHeight/2;

	static public int textFieldWidth = 500;
	static public int textFieldHeight = 70;
	static public int textFieldPositionX = oculusScreenWidth + (uppoScreenWidth/2) - textFieldWidth/2;
	static public int textFieldPositionY = (Screen.height/2) - textFieldHeight/2;

	public GUIText scoreText;
	public string playerName = "Demo";
	public GUISkin customSkin = null;
	public RUISPSMoveWand PSMoveController;

	public void OnGUI() 
	{
		if (customSkin != null)
		{
			GUI.skin = customSkin;
		}

		scoreText.text = "Your score was: " + PlayerController.getScore ().ToString();

		//playerName = GUI.TextField (new Rect (textFieldPositionX, textFieldPositionY, textFieldWidth, textFieldHeight), playerName, 25);
		//bool pressed = GUI.Button (new Rect (buttonPositionX, buttonPositionY, buttonWidth, buttonHeight), "Add score");

		//if (playerName != "" && playerName != null && pressed)
		//{
		//	HighScoreManager._instance.SaveHighScore(playerName, PlayerController.getScore());
		//	Application.LoadLevel ("SplashHighScoreScreen");
		//}
	}
	
	void FixedUpdate() 
	{
		if (Input.GetKeyDown("space") || this.PSMoveController.triggerValue > 0)
		{
			HighScoreManager._instance.SaveHighScore(playerName, PlayerController.getScore());
			Application.LoadLevel ("SplashHighScoreScreen");		
		}
	}
}
