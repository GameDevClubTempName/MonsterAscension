using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameOverController : MonoBehaviour {
	public bool gameOver; 
	public Image GameOverImage; 
	// Use this for initialization
	void Start () {
		GameOverImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver){
			GameOverImage.enabled = true; 
		}
	}
}
