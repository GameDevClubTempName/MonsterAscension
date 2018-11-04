using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAnimation : MonoBehaviour {
	public int characterlevel; 
	public AnimationClip[] playerAnimation1;
	public AnimationClip[] playerAnimation2;
	public AnimationClip[] playerAnimation3;
	public AnimationClip[] playerAnimation4;
	public AnimatorOverrideController ZombieAnimator; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(characterlevel == 1){
			ZombieAnimator[playerAnimation1];
		}else if (characterlevel ==2){
			ZombieAnimator[playerAnimation2];
		}else if(characterlevel == 3){
			ZombieAnimator[playerAnimation3];
		}else if(characterlevel == 4){
			ZombieAnimator[playerAnimation4];
		}
	}
}
