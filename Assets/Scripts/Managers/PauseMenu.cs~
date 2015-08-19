using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public static bool canPauseGame = false;
	public static bool restartedLevel = false;
	public static bool mainMenuLoop = false;

	public static bool paused;
	bool quitCheck = false;
	//public bool devPause;

	public int itemSelected = 0;
	public static bool selecting = false;
	float selectDely = 0.2f;

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

		
		#region controller support for Pause Menu
		if (PauseMenu.paused)
		{
			//Allow the player to switch between the menu
			if (Input.GetAxis ("Vertical") >= 0.5f && Input.GetAxis ("Vertical") <= 1.0f  && !selecting)
			{
				selecting = true;
				StartCoroutine(SelectionDecrease()); //selection moving Up
			}
			else if ( Input.GetAxis ("Vertical") <= -0.5f && Input.GetAxis ("Vertical") >= -1.0f && !selecting)
			{
				selecting = true;
				StartCoroutine(SelectionIncrease()); //selection moving Down
			}
			
			//menu choices
			if (!quitCheck)
			{
				if (itemSelected == 0) 
				{
					//print ("Resume");
				}
				if (itemSelected == 1)
				{
					//print ("Return Last Checkpoint");
				}
				if (itemSelected == 2)
				{
					//print ("Exit Game");
				}
			}
			else if (quitCheck)
			{
				if (itemSelected == 0) 
				{
					//print ("NO");
				}
				if (itemSelected == 1)
				{
					//print ("YES");
				}
			}
		}

		//accept button
		if (Input.GetKeyDown (KeyCode.JoystickButton0) && !quitCheck)
		{
			if (itemSelected == 0)  //Resume Game
			{
				paused = false;	
				Screen.showCursor = false;	
				Time.timeScale = 1f;
				itemSelected = 0;
			}
			if (itemSelected == 1) //Load Last Checkpoint
			{
				paused = false;
				Time.timeScale = 1f;
				restartedLevel = true;
				Screen.showCursor = false;
				itemSelected = 0;
				Application.LoadLevel(1);
			} 
			if (itemSelected == 2) //Exit Game
			{
				quitCheck = true;
				itemSelected = 0;
			} 
		}
		else if (Input.GetKeyDown (KeyCode.JoystickButton0) && quitCheck)
		{
			if (itemSelected == 0)  //NO
			{
				quitCheck = false;
				itemSelected = 2;
			}
			if (itemSelected == 1) //YES
			{
				Application.Quit();
				Debug.LogWarning ("Player Quit The Game");
			} 
		}

		//Back Buttons
		if (Input.GetKeyDown (KeyCode.Joystick1Button1) && !quitCheck) // B on menu
		{
			paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button1) && quitCheck) // B on quit menu
		{
			quitCheck = false;
		}

//		if (Input.GetKeyDown (KeyCode.Joystick1Button7) && !quitCheck) //start on menu
//		{
//			paused = false;	
//			Screen.showCursor = false;	
//			Time.timeScale = 1f;
//		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button7) && quitCheck) //start on quit menu
		{
			if (itemSelected == 0)  //NO
			{
				quitCheck = false;
				itemSelected = 2;
			}
			if (itemSelected == 1) //YES
			{
				Application.Quit();
				Debug.LogWarning ("Player Quit The Game");
			} 
		}

		if (Input.GetKeyDown (KeyCode.Joystick1Button6) && !quitCheck) // back on menu
		{
			paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
		}
		if (Input.GetKeyDown(KeyCode.Joystick1Button6) && quitCheck) // back on quit menu
		{
			quitCheck = false;
			itemSelected = 2;
		}
		#endregion

	}

	private IEnumerator SelectionIncrease()
	{
	//	print ("increase start");
		StartCoroutine (WaitForRealSeconds(selectDely, selecting));
		itemSelected ++;
		if (!quitCheck)
		{
			if (itemSelected == 3)
			{
				itemSelected = 0;
			}
		}
		else if (quitCheck)
		{
			if (itemSelected == 2)
			{
				itemSelected = 0;
			}
		}
		yield return null;
	//	print ("increase end");
	}
	private IEnumerator SelectionDecrease()
	{
	//	print ("decrease start");
		StartCoroutine (WaitForRealSeconds(selectDely, selecting));
		itemSelected --;
		if (!quitCheck)
		{
			if (itemSelected == -1)
			{
				itemSelected = 2;
			}
		}
		else if (quitCheck)
		{
			if (itemSelected == -1)
			{
				itemSelected = 1;
			}
		}
		yield return null;
	//	print ("decrease end");
	}
	public static IEnumerator WaitForRealSeconds( float delay, bool selecting )
	{
		selecting = PauseMenu.selecting;
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + delay)
		{
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + delay)
		{
			PauseMenu.selecting =false;
			yield return null;
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
			GUI.Label(new Rect(10, 10, 100, 20), "Item Selected " + itemSelected); //quick debug of what the controller is slecting
		
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
			}

		}
	}
}
