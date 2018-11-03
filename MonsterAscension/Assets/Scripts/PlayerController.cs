using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	// In degrees per second:
	public float rotationSpeed = 540.0f;

	// In units:
	public float playerDistanceFromTower = 5.0f;
	public float cameraDistanceFromTower = 10.0f;
	public float playerHeight = 5.0f;
	public float lightHeight = 20.0f;

	// How many sides are there to the tower?
	public int lanes = 8;

	// In degrees:
	private float rotation = 0f;

	// In set {-1, 0, 1}, from Input.GetAxisRaw()
	private int rotationInput;

	// Calculated as 360 / towerSides.
	private float rotationIncrement;

	private Transform cameraTransform;
	private Transform lightTransform;

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

		/*GameObject light = GameObject.FindGameObjectWithTag("Light");
		if (light == null)
		{
			Debug.Log("Light not found!");
		}
		else
		{
			//lightTransform = light.GetComponent<Transform>();
		}*/
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
		double rotationInRadians = rotation / 180 * Math.PI;
		Vector3 vector = new Vector3((float) Math.Cos(rotationInRadians), playerHeight, (float) Math.Sin(rotationInRadians));
		transform.position = new Vector3(-playerDistanceFromTower * (float) Math.Cos(rotationInRadians), playerHeight, -playerDistanceFromTower * (float) Math.Sin(rotationInRadians));
		cameraTransform.position = new Vector3(-cameraDistanceFromTower * (float) Math.Cos(rotationInRadians), playerHeight, -cameraDistanceFromTower * (float) Math.Sin(rotationInRadians));
		//lightTransform.position = new Vector3(-cameraDistanceFromTower * (float) Math.Cos(rotationInRadians), lightHeight, -cameraDistanceFromTower * (float) Math.Sin(rotationInRadians));

		transform.LookAt(cameraTransform);
		cameraTransform.LookAt(transform);
		//lightTransform.LookAt(transform);
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
		rotationInput = (int) Input.GetAxisRaw("Horizontal");
	}
}
