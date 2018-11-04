using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour {

	public HighScore highScore;
	public int finalScore = 0;
	public int topScore = 0;

	private void Start()
	{
		GameObject hs = GameObject.FindGameObjectWithTag("HighScore");
		if (hs == null)
		{
			Debug.Log("Game controller not found!");
		} else
		{
			highScore = hs.GetComponent<HighScore>();
			finalScore = highScore.finalScore;
			topScore = highScore.highScore;
		}
	}

	void OnGUI ()
	{ 

		GUI.Label (new Rect(650, 175, 200, 20), "Final Score: " + finalScore);
		GUI.Label (new Rect(650, 200, 200, 20), "High Score: " + topScore);

	}
}
