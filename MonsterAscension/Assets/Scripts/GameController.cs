using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject hazard;

	//holds reference to tower prefab
	public GameObject tower;

	//holds location of tower spawn points
	public Transform towerOneSpawn, towerTwoSpawn;

	//holds location of tower destroy point and a float to hold y position of towerDestroy
	public Transform towerDestroy;
	[SerializeField]private float towerDestroyY;

	public float spawnHeight;
	public float spawnDistance;
	public int lanes;
	public int maxSpawnAttempts;
	public float spawnWait;
	public float startWait;
	public float spawnNoise;

	//controls speed scrolling for a tower
	public float towerSpeed;

	//holds references to two towers to control scrolling
	[SerializeField]private GameObject towerUp, towerDown;


	void Start () {
		StartTower ();
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
			int numSpawnAttempts = Mathf.FloorToInt(Random.Range(1, maxSpawnAttempts));
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

	/**
	 * Creates two tower models, then starts scrolling them
	 */
	private void StartTower() {
		towerDown = Instantiate (tower, towerOneSpawn);
		towerUp = Instantiate (tower, towerTwoSpawn);
		towerDestroyY = towerDestroy.position.y;

		StartCoroutine (TowerScrolling());
	}

	IEnumerator TowerScrolling () {
		while (true) {

			if (towerDown.transform.position.y <= towerDestroyY) {
				float newY = towerDown.transform.position.y + 60;
				Vector3 newPos = new Vector3(0f, newY, 0f);

				Destroy (towerDown);
				towerDown = towerUp;
				towerUp = Instantiate (tower, newPos, towerDown.transform.rotation);
			}

			towerDown.transform.position += Vector3.down * towerSpeed;
			towerUp.transform.position += Vector3.down * towerSpeed;

			yield return new WaitForFixedUpdate ();
		}
	}
}
