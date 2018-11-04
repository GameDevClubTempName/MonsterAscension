using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	// In degrees:
	private float rotation = 0f;

	// In set {-1, 0, 1}, from Input.GetAxisRaw()
	private int rotationInput;

	// Calculated as 360 / towerSides.
	private float rotationIncrement;

	// In range 0-3 (for 4 levels):
	private int level = 0;

	// Counts up to monsterLevels[level], then levels-up:
	private int monstersCollected = 0;

	// Number of lanes to the tower, derived from GameController
	private int lanes;
	
	private SliderController sliderController;

	public AudioClip levelUp;
	public AudioClip lv0Move;
	public AudioClip lv1Move;
	public AudioClip lv2Move;
	public AudioClip lv3Move;
	public AudioClip rockHit;
	public AudioClip monsterGet;

	public float[] moveSoundDelay = new float[] {.2f, .2f, .2f, 2f};

	public AudioSource aSource;

	private Transform cameraTransform;
	public Image GameOverImage; 
	public Image LevelUpImage;
	public Animator Animator;
	public AnimationClip[] playerAnimation1;
	public AnimationClip[] playerAnimation2;
	public AnimationClip[] playerAnimation3;
	public AnimationClip[] playerAnimation4;

	public HighScore highScore;
	public TimeScore timeScore;
	
	void Start()
	{	
		GameObject hs = GameObject.FindGameObjectWithTag("HighScore");
		if (hs == null)
		{
			Debug.Log("Game controller not found!");
		} else
		{
			highScore = hs.GetComponent<HighScore>();
		}

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
			lanes = game.GetComponent<GameController>().lanes;
		}

		GameObject slider = GameObject.FindGameObjectWithTag("Slider");
		if (slider == null)
		{
			Debug.Log("Slider not found!");
		} else
		{
			sliderController = slider.GetComponent<SliderController>();
		}
		
		LevelUpImage.enabled = false;
		UpdateTransforms();
		StartCoroutine (MoveSounds ());
	}

	private IEnumerator MoveSounds() {
		while (true) {
			aSource.Play ();
			yield return new WaitForSeconds (moveSoundDelay[level]);
		}
	}

	private void setMoveClip() {
		if (level <= 0) {
			aSource.clip = lv0Move;
		} else if (level == 1) {
			aSource.clip = lv1Move;
		} else if (level == 2) {
			aSource.clip = lv2Move;
		} else {
			aSource.clip = lv3Move;
		}
	}

	// Public accessor:
	public int GetLevel ()
	{
		return level;
	}

	// Public accessor: returns the lane the player is in
	public int GetLane ()
	{
		return (int) Mathf.Round((-rotation + 90) / 360 * lanes);
	}

	// Called when the final monster for this level has been collected.
	void LevelUp ()
	{
		if (level < 3)
		{
			level++;
			Animator.Play("playerAnimation" + level.ToString());

			LevelUpImage.enabled = true;
			sliderController.levelUpSlider(monsterLevels[level]);

			setMoveClip();
			aSource.PlayOneShot(levelUp);
			Animator.SetInteger("Level", level);
		}
	}

	// Called when player has hit a hazard on the lowest level.
	void GameOver ()
	{
		monstersCollected = 0;
		sliderController.levelUpSlider(monsterLevels[0]);
		sliderController.updateSlider(0);
		int playerScore = (int)timeScore.playerScore;
		highScore.CheckHighScore (playerScore);
		SceneManager.LoadScene("GameOver" , LoadSceneMode.Single);
		// Switch to game-over screen here
	}

	// Called whenever the player collects a Monster can.
	void CollectMonster()
	{
		monstersCollected++;
		aSource.PlayOneShot (monsterGet);
		if (monstersCollected >= monsterLevels[level])
		{
			monstersCollected = 0;
			LevelUp();
		}

		sliderController.updateSlider(monstersCollected);
	}

	// Called whenever the player hits a hazard.
	void HitHazard ()
	{
		level--;
		Animator.SetInteger ("Level", level);
		setMoveClip ();
		monstersCollected = 0;

		aSource.PlayOneShot (rockHit);

		if (level < 0)
		{
			level = 0;
			GameOver();
		}

		sliderController.updateSlider(0);
	}
	
	// Called when the player hits an object.
	// If object is a hazard, calls HitHazard() and destroys object.
	// If object is a Monster can, calls CollectMonster() and destroys object.
	void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag == "Hazard")
		{
			StartCoroutine(blinkPlayer());
			HitHazard();
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
		return (float) Math.Floor(rotation / 360 * lanes) * 360 / lanes;
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
