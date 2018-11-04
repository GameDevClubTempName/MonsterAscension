using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// References to hazard, tower, and monster prefabs:
	public GameObject hazard, tower, monster;
	
	// Location of tower spawn points:
	public Transform towerOneSpawn, towerTwoSpawn;

	// Location of tower destroy point:
	public Transform towerDestroy;

	// How high do objects spawn, in units? (should be out of view)
	public float spawnHeight;

	// How far away from the tower are objects, in units?
	public float spawnDistance;

	// Number of sides to the tower:
	public int lanes = 16;

	// Theoretical maximum for # hazards spawned in a wave.
	// Likely lower, because hazards will try to spawn on top of each other.
	public int maxSpawnAttempts = 14;

	// Distance, in units, between waves:
	public float waveDistance = 10.0f;

	// Time to wait at start of game before spawning first wave.
	// This is not done via distance because a specified time is more useful.
	public float startWait = 1.0f;

	// Random variation, in units, between hazards in the same wave:
	public float spawnNoise = 2.0f;
	
	// References to two towers to control scrolling
	private GameObject towerUp, towerDown;
	
	// The speed of the player, and the speed of objects falling, in units per second:
	public float basePlayerSpeed = 5.0f;
	public float baseObjectSpeed = 5.0f;

	// Multipliers on basePlayerSpeed, determined by the current player level:
	public float[] speedMultipliers = new float[] { 1.0f, 1.5f, 2.0f, 3.0f };

	// Maximum # lanes to the left or the right of the player that Monster cans may spawn.
	public int monsterRange = 3;

	public int minWavesUntilNextMonster = 1;
	public int maxWavesUntilNextMonster = 5;
	private int wavesUntilNextMonster = 4;

	private PlayerController playerController;

	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			Debug.Log("Player not found!");
		}
		else
		{
			Debug.Log("Player found!");
			playerController = player.GetComponent<PlayerController>();
		}

		StartTower();
		StartCoroutine(SpawnObjects());
	}

	// How fast is the player moving?
	// This is equivalent to how fast the tower is moving downwards.
	public float GetPlayerSpeed ()
	{
		return basePlayerSpeed * speedMultipliers[playerController.GetLevel()];
	}

	// How fast do objects fall at?
	// Relative to the tower, they will always fall at baseObjectSpeed.
	public float GetObjectSpeed()
	{
		return GetPlayerSpeed() + baseObjectSpeed;
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
				StartCoroutine(SpawnObjectOnDelay(monster, GetSpawnPosition(angle), Quaternion.identity, (waveDistance / 2 + spawnNoise / 2) / GetPlayerSpeed()));
			}
			yield return new WaitForSeconds(waveDistance / GetPlayerSpeed());
		}
	}

	/**
	 * Creates two tower models, then starts scrolling them
	 */
	private void StartTower() {
		towerDown = Instantiate (tower, towerOneSpawn);
		towerUp = Instantiate (tower, towerTwoSpawn);

		StartCoroutine (TowerScrolling());
	}
	
	IEnumerator TowerScrolling () {
		while (true) {

			if (towerDown.transform.position.y <= towerDestroy.position.y) {
				float newY = towerDown.transform.position.y + 60;
				Vector3 newPos = new Vector3(0f, newY, 0f);

				Destroy (towerDown);
				towerDown = towerUp;
				towerUp = Instantiate (tower, newPos, towerDown.transform.rotation);
			}
			
			Vector3 addVector = Vector3.down * GetPlayerSpeed() * Time.fixedDeltaTime;
			towerDown.transform.position += addVector;
			towerUp.transform.position += addVector;

			yield return new WaitForFixedUpdate ();
		}
	}
}
