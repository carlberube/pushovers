using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
    private GameManager gameManager;
    public List<GameObject> goals;
    public PushoverController masterController;
    public Transform spawn;

	// Use this for initialization
	void Start () {
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
            GameObject masterControllerObject = gameManager.player;
            masterController = masterControllerObject.GetComponent<PushoverController>();
            masterController.startLevel(spawn.position);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (goals.TrueForAll(goalIsOn)) { 
            //Level is completed
            masterController.activated = false;
            if (gameManager == null)
            {
                levelCompleted();
            }
            else
            {
                gameManager.levelCompleted();
            }
        }
    }

    void levelCompleted()
    {
        print("LEVEL COMPLETED!!");
    }

    private static bool goalIsOn(GameObject goal)
    {
        GoalController goalController = goal.GetComponent<GoalController>();
        return goalController.activated;
    }
}
