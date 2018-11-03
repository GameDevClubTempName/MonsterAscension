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
	
	void Start () {
		StartCoroutine(SpawnHazards());
	}
	
	IEnumerator SpawnHazards ()
	{
		yield return new WaitForSeconds(startWait);
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
				float x = spawnDistance * Mathf.Sin(angle);
				float z = spawnDistance * Mathf.Cos(angle);
				Vector3 spawnPosition = new Vector3(x, spawnHeight, z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
			}
			yield return new WaitForSeconds(spawnWait);
		}
	}
}
