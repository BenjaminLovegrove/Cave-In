using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public static bool canPauseGame = false;
	public static bool restartedLevel = false;
	public static bool mainMenuLoop = false;

	public static bool paused;
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
			paused = true;
			Time.timeScale = 0f;
		}
		else
		{
			paused = false;
			Time.timeScale = 1f;
		}
	}

	void OnGUI() {
		if (paused)
		{
			GUI.Box (new Rect (Screen.width/2,Screen.height/2,200,150), "Game Paused");
			if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 25,150,30), "Resume Game"))
			{
				paused = false;
				Time.timeScale = 1f;
			}

			if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 60,150,30), "Restart Level"))
			{
				paused = false;
				Time.timeScale = 1f;
				restartedLevel = true;
				Application.LoadLevel(1);
			}
			if (GUI.Button(new Rect((Screen.width/2)+ 25,(Screen.height/2) + 95,150,30), "Return To Menu"))
			{
				paused = false;
				Time.timeScale = 1f;
				mainMenuLoop = true;
				Intro.caveInStarted = false;
				Intro.gameStarted = false;
				Intro.skipNum = 0;
				Intro.watchIntro = false;
				Application.LoadLevel(0);
			}
		}
	}
}
