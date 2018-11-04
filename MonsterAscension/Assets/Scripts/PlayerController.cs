using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	//contains position for player to look at
	public Transform center;

	//holds the particle system
	public ParticleSystem parWings1;
	public ParticleSystem parWings2;

	// In degrees per second:
	public float rotationSpeed = 180.0f;

	// In units:
	public float playerDistanceFromTower = 0.9f;
	public float cameraDistanceFromTower = 1.0f;
	public float playerDistance = 5.0f;
	public float cameraDistance = 10.0f;
	public float backgroundDistance = 15.0f;
	public float playerHeight = 5.0f;
	public float cameraHeight = 6.0f;
	
	// How many monsters need to be collected, per level, to get to the next?
	public int[] monsterLevels = new int[] { 5, 10, 20 };
	
	// The time a player gets boosted when they collect a Monster when they're max level, in seconds:
	public float boostTime = 2.0f;

	// The multiplier on how fast the player goes while boosting.
	public float boostMultiplier = 1.2f;

	// In degrees:
	private float rotation = 0f;

	// In set {-1, 0, 1}, from Input.GetAxisRaw()
	private int rotationInput;
	
	// In range 0-3 (for 4 levels):
	private int level = 0;

	// Counts up to monsterLevels[level], then levels-up:
	private int monstersCollected = 0;

	private GameController gameController;

	private Transform cameraTransform;
	public Image GameOverImage; 
	public Animator Animator;
	public AnimationClip[] playerAnimation1;
	public AnimationClip[] playerAnimation2;
	public AnimationClip[] playerAnimation3;
	public AnimationClip[] playerAnimation4;
	void Start()
	{
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		if (camera == null)
		{
			Debug.Log("Camera not found!");
		}
		else
		{
			cameraTransform = camera.GetComponent<Transform>();
		}

		GameObject game = GameObject.FindGameObjectWithTag("GameController");
		if (game == null)
		{
			Debug.Log("Game not found!");
		}
		else
		{
			gameController = game.GetComponent<GameController>();
		}

		///GameOverImage.enabled = false;
		UpdateTransforms();
	}

	// Public accessor:
	public int GetLevel ()
	{
		return level;
	}

	// Public accessor: returns the lane the player is in
	public int GetLane ()
	{
		return (int) Mathf.Round((-rotation + 90) / 360 * gameController.lanes);
	}

	// Called when the final monster for this level has been collected.
	void LevelUp ()
	{
		level++;
		if(level == 1){
			Animator.Play("playerAnimation1");
		}else if (level ==2){
			Animator.Play("playerAnimation2");
		}else if(level == 3){
			Animator.Play("playerAnimation3");
		}else if(level == 4){
			Animator.Play("playerAnimation4");
		}
		// Animation-switching code here
	}

	// Called when player has hit a hazard on the lowest level.
	void GameOver ()
	{
		//GameOverImage.enabled = true;
		// Switch to game-over screen here
	}

	// Called whenever the player collects a Monster can.
	void CollectMonster()
	{
		monstersCollected++;
		if (level >= monsterLevels.Length)
		{
			gameController.Boost(boostTime);
		} else if (monstersCollected >= monsterLevels[level])
		{
			monstersCollected = 0;
			LevelUp();
		}
	}

	// Called whenever the player hits a hazard.
	void HitHazard ()
	{
		level--;
		monstersCollected = 0;
		if (level < 0)
		{
			level = 0;
			GameOver();
		}
	}
	
	// Called when the player hits an object.
	// If object is a hazard, calls HitHazard() and destroys object.
	// If object is a Monster can, calls CollectMonster() and destroys object.
	void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag == "Hazard")
		{
			HitHazard();
			StartCoroutine(blinkPlayer());
			Destroy(collider.gameObject);
		} else if (collider.gameObject.tag == "Monster")
		{
			CollectMonster();
			Destroy(collider.gameObject);
			parWings1.Play ();
			parWings2.Play ();
		}
	}

	private IEnumerator blinkPlayer() {
		bool blinking = false;
		GetComponent<BoxCollider> ().enabled = false;
		for (int i = 0; i < 8; i++) {
			GetComponent<SpriteRenderer> ().enabled = blinking;
			blinking = !blinking;
			yield return new WaitForSeconds (.25f);
		}
		GetComponent<BoxCollider> ().enabled = true;
	}

	float ConstrainRotation (float rotation)
	{
		while (rotation > 360)
		{
			rotation -= 360;
		}
		while (rotation < 0)
		{
			rotation += 360;
		}
		return rotation;
	}

	float RoundToNearestLane (float rotation)
	{
		return (float) Math.Floor(rotation / 360 * gameController.lanes) * 360 / gameController.lanes;
	}

	void UpdateTransforms ()
	{
		float rotationInRadians = rotation / 180 * Mathf.PI;
		float x = Mathf.Cos(rotationInRadians);
		float z = Mathf.Sin(rotationInRadians);
		transform.position = new Vector3(-playerDistance * x, playerHeight, -playerDistance * z);
		cameraTransform.position = new Vector3(-cameraDistance * x, cameraHeight, -cameraDistance * z);

		//transform.rotation = Quaternion.Euler(0, -rotation, 0);
		transform.LookAt(center);
		cameraTransform.rotation = Quaternion.Euler(0, -rotation + 90, 0);
	}

	void FixedUpdate ()
	{
		if (rotationInput != 0)
		{
			rotation += Time.fixedDeltaTime * rotationSpeed * rotationInput;
			rotation = ConstrainRotation(rotation);
			UpdateTransforms();
		}
	}

	void Update ()
	{
		rotationInput = (int)Input.GetAxisRaw("Horizontal");
	}
}
