using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesSpawner : MonoBehaviour {

    public GameObject bees;
    public Transform spawnPoint;

	// Use this for initialization
	void Start () {
        Instantiate(bees, spawnPoint.position, spawnPoint.rotation);
	}
	
	
		
	
}
