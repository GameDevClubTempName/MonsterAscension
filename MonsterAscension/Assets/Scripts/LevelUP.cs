using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelUP : MonoBehaviour {
	public bool levelUp; 
	public Image levelUpImage; 
	// Use this for initialization
	void Start () {
		levelUpImage.enabled = false; 
	}
	
	// Update is called once per frame
	void Update () {
		if(levelUp){
			levelUpImage.enabled = true; 
		}
	}
}
