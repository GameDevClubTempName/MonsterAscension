using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour {
	public Image GameOverImage; 

	void Start () {
		GameOverImage.enabled = true;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			SceneManager.LoadScene("MainScene" , LoadSceneMode.Single);
		}
	}
}
