using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

	public float destroyHeight = 0.0f;

	private GameController gameController;
	private float speed;
	
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
		float y = transform.position.y - Time.fixedDeltaTime * gameController.GetObjectSpeed();
		float z = transform.position.z;

		if (y < destroyHeight)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.position = new Vector3(x, y, z);
		}
	}
}
