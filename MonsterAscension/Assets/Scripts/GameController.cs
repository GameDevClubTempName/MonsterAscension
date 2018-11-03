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
            int lane = Mathf.FloorToInt(Random.Range(0, lanes));
            Debug.Log(lane);
            float angle = (2 * Mathf.PI / lanes) * lane;
            float x = spawnDistance * Mathf.Cos(angle);
            float z = spawnDistance * Mathf.Sin(angle);
            Vector3 spawnPosition = new Vector3(x, spawnHeight, z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(hazard, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }
}
