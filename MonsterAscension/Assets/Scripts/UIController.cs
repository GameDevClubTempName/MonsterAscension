using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			SceneManager.LoadScene("PlayerAnimation" , LoadSceneMode.Single);
			Debug.Log("This part is working");	
		}
	}

	
	
}
