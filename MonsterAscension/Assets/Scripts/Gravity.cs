using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

	public float destroyHeight = 0.0f;

	private GameController gameController;

	void Start ()
	{
		GameObject game = GameObject.FindGameObjectWithTag("GameController");
		if (game == null)
		{
			Debug.Log("Game not found!");
		}
		else
		{
			gameController = game.GetComponent<GameController>();
		}

	}

	void FixedUpdate () {
		float x = transform.position.x;
		float y = transform.position.y - Time.fixedDeltaTime * gameController.GetSpeed();
		float z = transform.position.z;

		if (y < 0)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.position = new Vector3(x, y, z);
		}
	}
}
