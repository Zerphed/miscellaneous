using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplashHighScoreGUI : MonoBehaviour 
{
	public GUISkin customSkin = null;
	
	static public int buttonWidth = 300;
	static public int buttonHeight = 100;
	public int buttonPositionX = 488;
	public int buttonPositionY = 558;

	// I am so.. so.. sorry... I just didn't know what else to do..
	public GUIText rank_1;
	public GUIText rank_2;
	public GUIText rank_3;
	public GUIText rank_4;
	public GUIText rank_5;

	public GUIText name_1;
	public GUIText name_2;
	public GUIText name_3;
	public GUIText name_4;
	public GUIText name_5;

	public GUIText score_1;
	public GUIText score_2;
	public GUIText score_3;
	public GUIText score_4;
	public GUIText score_5;

	public void Start() 
	{
		rank_1.text = "";
		rank_2.text = "";
		rank_3.text = "";
		rank_4.text = "";
		rank_5.text = "";

		name_1.text = "";
		name_2.text = "";
		name_3.text = "";
		name_4.text = "";
		name_5.text = "";

		score_1.text = "";
		score_2.text = "";
		score_3.text = "";
		score_4.text = "";
		score_5.text = "";
	}

	public void OnGUI() 
	{
		if (customSkin != null)
		{
			GUI.skin = customSkin;
		}

		// Print the high scores
		List<Score> HighScores = HighScoreManager._instance.GetHighScore();

		if (HighScores.Count >= 1) 
		{
			rank_1.text = "1.";
			name_1.text = HighScores[0].name;
			score_1.text = HighScores[0].score.ToString();
		}
		if (HighScores.Count >= 2) 
		{
			rank_2.text = "2.";
			name_2.text = HighScores[1].name;
			score_2.text = HighScores[1].score.ToString();
		}
		if (HighScores.Count >= 3) 
		{
			rank_3.text = "3.";
			name_3.text = HighScores[2].name;
			score_3.text = HighScores[2].score.ToString();
		}
		if (HighScores.Count >= 4) 
		{
			rank_4.text = "4.";
			name_4.text = HighScores[3].name;
			score_4.text = HighScores[3].score.ToString();
		}
		if (HighScores.Count >= 5) 
		{
			rank_5.text = "5.";
			name_5.text = HighScores[4].name;
			score_5.text = HighScores[4].score.ToString();
		}
	
	}
	void Update () {
		if(Input.GetKeyDown (KeyCode.Space)){
			Application.LoadLevel ("Initial");
		}
	}
}
