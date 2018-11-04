using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSliderController : MonoBehaviour {

	private Slider slider;
	
	void Start () {
		slider = gameObject.GetComponent<Slider>();
	}
	
	void Update () {
		slider.value = 1;
	}
}
