using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public GameObject monster;
	public float spawnHeight;
	public float spawnDistance;
	public int lanes;
	public int maxSpawnAttempts;
	public float spawnWait;
	public float startWait;
	public float spawnNoise;
	public float baseSpeed = 5.0f;
    public float[] speedMultipliers = new float[] { 1.0f, 1.2f, 1.5f, 2.0f };
	public int monsterRange = 3;

	public int minWavesUntilNextMonster = 1;
	public int maxWavesUntilNextMonster = 5;
	private int wavesUntilNextMonster = 4;

	private PlayerController playerController;

	void Start () {
		StartCoroutine(SpawnObjects());
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			Debug.Log("Player not found!");
		}
		else
		{
			playerController = player.GetComponent<PlayerController>();
		}
	}

	public float GetSpeed ()
	{
		return baseSpeed * speedMultipliers[playerController.GetLevel()];
	}
	
	Vector3 GetSpawnPosition (float angle)
	{
		return new Vector3(-spawnDistance * Mathf.Sin(angle), spawnHeight, -spawnDistance * Mathf.Cos(angle));
	}

	IEnumerator SpawnObjectOnDelay(GameObject hazard, Vector3 spawnPosition, Quaternion spawnRotation, float delay)
	{
		yield return new WaitForSeconds(delay);
		Instantiate(hazard, spawnPosition, spawnRotation);
	}

	IEnumerator SpawnObjects ()
	{
		yield return new WaitForSeconds(startWait);
		bool alternator = false;
		while (true)
		{
			bool monsterSpawn = false;
			wavesUntilNextMonster--;
			if (wavesUntilNextMonster <= 0)
			{
				wavesUntilNextMonster = Mathf.FloorToInt(Random.Range(minWavesUntilNextMonster, maxWavesUntilNextMonster));
				monsterSpawn = true;
			}
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
				StartCoroutine(SpawnObjectOnDelay(hazard, GetSpawnPosition(angle), Quaternion.identity, Random.Range(0, spawnNoise)));
			}
			alternator = !alternator;
			if (monsterSpawn)
			{
				float angle = (2 * Mathf.PI / lanes) * (playerController.GetLane() + Mathf.FloorToInt(Random.Range(-monsterRange, monsterRange + 1)));
				StartCoroutine(SpawnObjectOnDelay(monster, GetSpawnPosition(angle), Quaternion.identity, spawnNoise / 2 + spawnWait / 2));
			}
			yield return new WaitForSeconds(spawnWait);
		}
	}
}
