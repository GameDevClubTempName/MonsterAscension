using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {

	public int finalScore;
	public int highScore = 0;

	// Use this for initialization
	void Start () {

		GameObject hs = GameObject.FindGameObjectWithTag("HighScore");
		if (hs != null)
		{
			if (!hs.Equals (this.gameObject)) {
				Destroy (this.gameObject);
			}
		}
		
		DontDestroyOnLoad (this.gameObject);
	}

	public void CheckHighScore(int score) {
		finalScore = score;
		if (finalScore > highScore) {
			highScore = finalScore;
		}
	}
}
