﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public GameObject player;
    public int levelIndex = 0;
    public bool levelReady = false;

    private GameObject levelImage;

    private Text levelText;

    void Awake ()
    {
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        loadLevel(levelIndex);
    }

    //Update is called every frame.
    void loadLevel(int levelIndex)
    {
        levelReady = false;
        //Set the text of levelText to the string "Day" and append the current level number.
        if (levelIndex == 0)
        {
            levelText.text = "Pushovers";
        }
        else
        {
            int levelNumber = levelIndex + 1;
            levelText.text = "Level " + levelNumber;
        }

        //Set levelImage to active blocking player's view of the game board during setup.
        levelImage.SetActive(true);
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
        Invoke("HideLevelImage", levelStartDelay);
    }

    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);
        levelReady = true;
    }

    public void levelCompleted()
    {
        SceneManager.UnloadScene(levelIndex);
        levelIndex += 1;
        loadLevel(levelIndex);
    }
}
