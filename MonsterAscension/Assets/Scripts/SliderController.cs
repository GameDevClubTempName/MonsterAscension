using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SliderController : MonoBehaviour {
	public Slider Slider; 
	public int startingMonster = 0; 
	public int currentMonster = 1; 
	public int currentLvlMonster = 5; 
	public bool monsterCollected; 
	public bool hitByBoulder; 

	// Use this for initialization
	void Start () {
		Slider.minValue = 0; 
	}
	
	// Update is called once per frame
	void Update () {
		Slider.maxValue = currentLvlMonster; 

		if(monsterCollected){
			currentMonster ++; 
			Slider.value = currentMonster; 
			if(currentMonster == currentLvlMonster){
				currentMonster += 5; 
			}
		}
		


		if(hitByBoulder){
			currentMonster --; 
			Slider.value = currentMonster; 
			if(currentMonster == 0 && currentLvlMonster == 5){
				//ded
				Slider.value = 0; 
			}
		}

	}
}
