using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

	// In degrees per second:
	public float rotationSpeed = 540.0f;

	// In units:
<<<<<<< HEAD:MonsterAscension/Assets/_Scripts/PlayerController.cs
	public float playerDistanceFromTower = 0.9f;
	public float cameraDistanceFromTower = 1.0f;
=======
	public float playerDistance = 5.0f;
	public float cameraDistance = 10.0f;
	public float backgroundDistance = 15.0f;
>>>>>>> In-GameUi:MonsterAscension/Assets/Scripts/PlayerController.cs
	public float playerHeight = 5.0f;

	// How many sides are there to the tower?
	public int lanes = 8;

	// In degrees:
	private float rotation = 0f;

	// In set {-1, 0, 1}, from Input.GetAxisRaw()
	private int rotationInput;

	// Calculated as 360 / towerSides.
	private float rotationIncrement;

	private float size = 1.0f;

	private Transform cameraTransform;

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

		UpdateTransforms();
	}

	public int GetLevel ()
	{
		return 0;
	}

	public int GetLane ()
	{
		return (int) Mathf.Round(rotation / 360 * lanes);
	}

	// Temporary way to display level.
	void UpdateSize (float newSize)
	{
		size = newSize;
		transform.localScale = new Vector3(size, size, size);
	}

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log("Collision!");
		if (collision.gameObject.tag == "Hazard")
		{
			UpdateSize(size * 0.8f);
			Destroy(collision.gameObject);
		} else if (collision.gameObject.tag == "Monster")
		{
			UpdateSize(size * 1.25f);
			Destroy(collision.gameObject);
		}
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
		cameraTransform.position = new Vector3(-cameraDistance * x, playerHeight, -cameraDistance * z);
		
		transform.LookAt(cameraTransform);
		cameraTransform.LookAt(transform);
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
