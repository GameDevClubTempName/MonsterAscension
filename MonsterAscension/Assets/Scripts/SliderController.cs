using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SliderController : MonoBehaviour {
	public Slider Slider; 
 	public float monstersCollected1;

	// Use this for initialization
	void Start () {
		Slider.minValue = 0; 
		
	}
	
	// Update is called once per frame
	void Update () {
		Slider.value = monstersCollected1;
		
		}
	public void levelUpSlider(int monsterLevels){
		Slider.maxValue = monsterLevels;
		//Slider.value = 0; 
	}

	public void updateSlider(int monstersCollected){
		monstersCollected1 = (float) monstersCollected; 
	}
	}

