using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour {
	public Image GameOverImage; 
	// Use this for initialization
	void Start () {
		GameOverImage.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			SceneManager.LoadScene("MainScene" , LoadSceneMode.Single);
		}
	}
}
