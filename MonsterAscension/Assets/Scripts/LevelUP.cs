using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelUP : MonoBehaviour {
	public bool levelUp;
	public Image levelUpImage;
	
	void Start () {
		levelUpImage.enabled = false; 
	}
	
	void Update () {
		if (levelUp) {
			levelUpImage.enabled = true; 
		}
	}
}
