using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour {
	public Slider Slider; 
	public int startingMonster = 0; 
	public int currentMonster = 0; 
	public int currentLvlMonster = 5; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(monsterCollected){
			currentMonster ++; 
			Silder.value = currentMonster; 
			if(monsterCollected == currentLvlMonster){
				currentMonster += 5; 
			}
		}
		

		if(hitByBoulder){
			currentMonster --; 
			Slider.value = currentMonster; 
			if(monsterCollected == 0){
				//ded
			}
		}


	}
}
