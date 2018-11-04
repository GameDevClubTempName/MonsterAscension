using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour {
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			SceneManager.LoadScene("MainScene" , LoadSceneMode.Single);
			Debug.Log("Scene switched from UI to MainScene");
		}
	}
}
