﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public static bool canPauseGame = false;
	public static bool restartedLevel = false;
	public static bool mainMenuLoop = false;

	public static bool paused;
	bool quitCheck = false;
	//public bool devPause;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (canPauseGame &&(Intro.introTimer < 0))
		{
			//print ("can pause");
			if (Input.GetKeyDown (KeyCode.Escape))
			{
				PauseGame();
			}
		}

	}

	public static void PauseGame()
	{
		if (!paused)
		{
			Screen.showCursor = true;
			paused = true;
			Time.timeScale = 0f;
		}
		else
		{
			Screen.showCursor = false;
			paused = false;
			Time.timeScale = 1f;
		}
	}

	void OnGUI() {
		if (paused)
		{
			if (!quitCheck)
			{
				GUI.Box (new Rect (Screen.width/2,Screen.height/2,200,150), "Game Paused");
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 25,150,30), "Resume Game"))
				{
					paused = false;
					Screen.showCursor = false;
					Time.timeScale = 1f;
				}

				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 60,150,30), "Restart Level"))
				{
					paused = false;
					Time.timeScale = 1f;
					restartedLevel = true;
					Screen.showCursor = false;
					Application.LoadLevel(1);
				}
	//			if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,150,30), "Return To Menu"))
	//			{
	//				CheckpointManager.checkpointSpawn = false;
	//				paused = false;
	//				Time.timeScale = 1f;
	//				mainMenuLoop = true;
	//				Intro.caveInStarted = false;
	//				Intro.gameStarted = false;
	//				Intro.skipNum = 0;
	//				Intro.watchIntro = false;
	//				Intro.introTimer = 30.0f;
	//				Screen.showCursor = false;
	//				Application.LoadLevel(0);
	//			}
		
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,150,30), "Quit Game"))
				{
					quitCheck = true;
				}
			}
			if (quitCheck)
			{
				GUI.Box (new Rect (Screen.width/2,Screen.height/2,200,150), "Are You Sure?");
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 60,150,30), "YES"))
				{
					Application.Quit();
					Debug.LogWarning ("Player Quit The Game");

				}
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,150,30), "NO"))
				{
					quitCheck = false;
				}
			}

		}
	}
}