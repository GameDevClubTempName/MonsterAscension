using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGravity : MonoBehaviour {

    public float baseSpeed;
    public float[] speedMultipliers = new float[4];
	public float destroyHeight;

	
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
