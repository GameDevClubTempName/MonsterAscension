using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGravity : MonoBehaviour {

    public float baseSpeed = 5.0f;
    public float[] speedMultipliers = new float[] { 1.0f, 1.2f, 1.5f, 2.0f };
	public float destroyHeight = 0.0f;
	
	void FixedUpdate () {
		float x = transform.position.x;
		float y = transform.position.y - Time.fixedDeltaTime * baseSpeed * speedMultipliers[0];
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
