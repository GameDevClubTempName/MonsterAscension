using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScore : MonoBehaviour {

    public float playerScore = 0;

    public GameController gameController;

    private void Start()
    {
        GameObject game = GameObject.FindGameObjectWithTag("GameController");
        if (game == null)
        {
            Debug.Log("Game controller not found!");
        } else
        {
            gameController = game.GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void Update () {

        playerScore += Time.deltaTime * gameController.GetPlayerSpeed();

	}

    void OnGUI ()
    { 

        GUI.Label (new Rect(10, 10, 200, 20), "score: " + (int)(playerScore));
     
    }
}
