using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SliderController : MonoBehaviour {
	public Slider slider;

	void Start () {
		slider = gameObject.GetComponent<Slider>();
		slider.minValue = 0;
	}
	
	public void levelUpSlider (int monsterLevels) {
		slider.maxValue = monsterLevels;
		updateSlider(0);
	}

	public void updateSlider (int monstersCollected) {
		slider.value = monstersCollected;
	}
}
