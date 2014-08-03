using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Score : IComparable<Score>
{
	public string name;
	public int score;

	public Score (string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	public int CompareTo(Score comparePart) 
	{
		if (comparePart == null) 
		{
			return 1;
		}

		return this.score.CompareTo (comparePart.score);
	}
}

public class HighScoreManager : MonoBehaviour 
{
	private static HighScoreManager m_instance;
	private const int LeaderBoardLength = 5;
	private const string HashPrefix = "HS";

	public static HighScoreManager _instance 
	{
		// Create automatic getters for instance
		get 
		{
			if (m_instance == null) 
			{
				m_instance = new GameObject("HighScoreManager").AddComponent<HighScoreManager>();
			}
			return m_instance;
		}
	}

	public void Awake () 
	{
		if (m_instance == null) 
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy gameObject and its children between scenes
		DontDestroyOnLoad(gameObject);
	}

	// When the application is closed, save the player preferences.
	public void OnApplicationQuit() 
	{
		PlayerPrefs.Save ();
	}

	// Use this method to retrieve high scores from the database
	public List<Score> GetHighScore () 
	{
		List<Score> HighScores = new List<Score> ();

		// Retrieve the high scores leaderboard, which is stored locally in the player preferences.
		// Player preferences last between game sessions.
		for (int i = 0; i < LeaderBoardLength && PlayerPrefs.HasKey (HashPrefix+i+"score"); ++i) 
		{
			HighScores.Add ( new Score(PlayerPrefs.GetString (HashPrefix+i+"name"), PlayerPrefs.GetInt (HashPrefix+i+"score")) );
		}

		return HighScores;
	}

	// Use this method to store high scores to the database
	public void SaveHighScore (string name, int score) 
	{
		List<Score> HighScores = GetHighScore();

		// Was this the first high score or are there less than LeaderBoardLength scores?
		// Or was this score lower than the existing last score?
		// If so add it to the HighScores list.
		if (HighScores.Count < LeaderBoardLength || score < HighScores[HighScores.Count-1].score) 
		{
			Score _temp = new Score(name, score);
			HighScores.Add (_temp);
		}

		// Sort the list before saving
		HighScores.Sort ();

		// Store the current leaderboard to PlayerPrefs
		for (int i = 0; i < HighScores.Count && i < LeaderBoardLength; ++i) 
		{
			PlayerPrefs.SetString (HashPrefix+i+"name", HighScores[i].name);
			PlayerPrefs.SetInt (HashPrefix+i+"score", HighScores[i].score);
		}

		PlayerPrefs.Save ();
	}
	
	public void ClearLeaderboard() 
	{
		List<Score> HighScores = GetHighScore ();

		for (int i = 0; i < HighScores.Count; ++i) 
		{
			PlayerPrefs.DeleteKey (HashPrefix+i+"name");
			PlayerPrefs.DeleteKey (HashPrefix+i+"score");
		}
	}
}
