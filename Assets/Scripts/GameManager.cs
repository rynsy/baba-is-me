﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f;							//Delay between each Player turn.
    public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
    [HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
    
    
    private Text levelText;									//Text to display current level number.
    private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
    private BoardManager boardScript;        
    private int level = 1;									//Current level number, expressed in game as "Day 1".
    private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
    
    
    
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);	
        
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (instance != null) { 
            instance.level++;
            instance.InitGame();
        }
    }

    
    //Initializes the game for each level.
    void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;

        boardScript.SetupScene(0);
        
        //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
        Invoke("HideLevelImage", levelStartDelay);
        
    }
    
    
    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        //levelImage.SetActive(false);
        
        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }
    
    //Update is called every frame.
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if(playersTurn || doingSetup)
            
            //If any of these are true, return and do not start MoveEnemies.
            return;

        //Start moving enemies.
        //        StartCoroutine (MoveEnemies ());
        playersTurn = true;
    }
    
    
    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        //levelText.text = "After " + level + " days, you starved.";
        
        //Enable black background image gameObject.
        //levelImage.SetActive(true);
        
        //Disable this GameManager.
        enabled = false;
    }
    
}
