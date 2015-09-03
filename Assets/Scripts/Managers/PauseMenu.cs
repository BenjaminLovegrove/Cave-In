using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public static bool canPauseGame = false;
	public static bool restartedLevel = false;
	public static bool mainMenuLoop = false;

	public static bool paused;
	public static bool quitCheck = false;
	//public bool devPause;

	public static bool selecting = false;


	//Text gameobjects
	public GameObject 
		canvasObj,
		mainOptions,
		quitOptions;

	// Use this for initialization
	void Awake () {
		//canvasObj.SetActive(false);
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

		//show or hide the pause menu options
		if (paused && !quitCheck)
		{
			//print ("main options");
			canvasObj.SetActive(true);
			mainOptions.SetActive(true);
			quitOptions.SetActive(false);
		}
		if (!paused && !quitCheck) 
		{
			//print ("Menu hidden");
			canvasObj.SetActive(false);
			quitOptions.SetActive(false);
		}

		if (paused && quitCheck)
		{
			//print ("quit options");
			mainOptions.SetActive(false);
			quitOptions.SetActive(true);
		}


		




	}


	public static void PauseGame()
	{
		if (!paused)
		{
			Screen.showCursor = true;
			paused = true;
			Time.timeScale = 0f;
			Controller.xInput.stopPadVibration(Player.pone);
			Controller.xInput.stopPadVibration(Player.ptwo);

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
			//placeholder menu code, keep incase new one breaks
			/*	
			if (!quitCheck)
			{
				GUI.Box (new Rect (Screen.width/2,Screen.height/2,300,175), "Game Paused");
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 25,250,30), "Resume Game"))
				{
					paused = false;
					Screen.showCursor = false;
					Time.timeScale = 1f;
				}

				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 60,250,30), "Return Last checkpoint"))
				{
					paused = false;
					Time.timeScale = 1f;
					restartedLevel = true;
					Screen.showCursor = false;
					Application.LoadLevel(1);
				}
//				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,250,30), "Back To Menu"))
//				{
////					CheckpointManager.checkpointSpawn = false;
////					paused = false;
////					Time.timeScale = 1f;
////					mainMenuLoop = true;
////					Intro.caveInStarted = false;
////					Intro.gameStarted = false;
////					Intro.skipNum = 0;
////					Intro.watchIntro = false;
////					Intro.introTimer = 25.0f;
////					Screen.showCursor = false;
////					Application.LoadLevel(0);
//				}
		
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,250,30), "Exit Game"))
				{
					quitCheck = true;
				}
			}
			if (quitCheck)
			{
				GUI.Box (new Rect (Screen.width/2,Screen.height/2,300,175), "Are You Sure?");
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 60,250,30), "YES"))
				{
					Application.Quit();
					Debug.LogWarning ("Player Quit The Game");

				}
				if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,250,30), "NO"))
				{
					quitCheck = false;
				}
			}*/

		}
	}	
}
