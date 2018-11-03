using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public float spawnHeight;
	public float spawnDistance;
	public int lanes;
	public float spawnWait;
	public float startWait;
	public float spawnNoise;
	
	void Start () {
		StartCoroutine(SpawnHazards());
	}
	
	IEnumerator SpawnHazardOnDelay (GameObject hazard, Vector3 spawnPosition, Quaternion spawnRotation, float delay)
	{
		yield return new WaitForSeconds(delay);
		Instantiate(hazard, spawnPosition, spawnRotation);
	}

	IEnumerator SpawnHazards ()
	{
		yield return new WaitForSeconds(startWait);
		bool alternator = false;
		while (true)
		{
			bool[] spawns = new bool[lanes];
			int numSpawnAttempts = Mathf.FloorToInt(Random.Range(1, lanes - 1));
			for (int i = 0; i < numSpawnAttempts; i++)
			{
				int lane = Mathf.FloorToInt(Random.Range(0, lanes));
				spawns[lane] = true;
			}
			for (int lane = 0; lane < lanes; lane++)
			{
				if (!spawns[lane])
				{
					continue;
				}
				float angle = (2 * Mathf.PI / lanes) * lane;
				if (alternator)
				{
					// 1/2 of a lane distance: alternate between spawning in lanes and in-between lanes
					angle += Mathf.PI / lanes;
				}
				float x = spawnDistance * Mathf.Sin(angle);
				float z = spawnDistance * Mathf.Cos(angle);
				Vector3 spawnPosition = new Vector3(x, spawnHeight, z);
				Quaternion spawnRotation = Quaternion.identity;
				StartCoroutine(SpawnHazardOnDelay(hazard, spawnPosition, spawnRotation, Random.Range(0, spawnNoise)));
			}
			alternator = !alternator;
			yield return new WaitForSeconds(spawnWait);
		}
	}
}
