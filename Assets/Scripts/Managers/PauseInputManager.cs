using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// To do:
/// Mouse input perform actions
/// mouse input while controller in?
/// audio effects
/// xbox input delay for selcting
/// split the inputs for two controllers
/// 
/// coroutines need to not have yields and be replaced with time.realtimesincestart
/// </summary>

public class PauseInputManager : MonoBehaviour {
	
	public Color defaultCol, hoverCol, pressCol;
	public float buttonDelay = 0.25f;

	//Text gameobjects
	public Text 
		resumeGame,
		loadLastCheckpoint,
		exitGame,
		quitYes,
		quitNo;

	public int itemSelected = 0;

	public bool selecting;
	public bool quitCheck;
	float selectDely = 0.2f;


	// Use this for initialization
	void Start () {
		defaultCol = Color.white;
		hoverCol = Color.red;
		pressCol = Color.green;
		selecting = PauseMenu.selecting;
		quitCheck = PauseMenu.quitCheck;
	}

	void Update()
	{
		#region controller support for Pause Menu
		if (PauseMenu.paused && PlayerV2.keyboardActive == false)
		{
			print ("running pause menu");
			//Allow the player to switch between the menu
			if (Input.GetAxis ("Vertical") >= 0.5f && Input.GetAxis ("Vertical") <= 1.0f  && !PauseMenu.selecting)
			{
				PauseMenu.selecting = true;
				StartCoroutine(SelectionDecrease()); //selection moving Up
			}
			else if ( Input.GetAxis ("Vertical") <= -0.5f && Input.GetAxis ("Vertical") >= -1.0f && !PauseMenu.selecting)
			{
				PauseMenu.selecting = true;
				StartCoroutine(SelectionIncrease()); //selection moving Down
			}
			
			//menu choices
			if (!quitCheck)
			{
				if (itemSelected == 0) 
				{
					resumeGame.color = hoverCol;
					loadLastCheckpoint.color = defaultCol;
					exitGame.color = defaultCol;
				}
				if (itemSelected == 1)
				{
					resumeGame.color = defaultCol;
					loadLastCheckpoint.color = hoverCol;
					exitGame.color = defaultCol;
				}
				if (itemSelected == 2)
				{
					resumeGame.color = defaultCol;
					loadLastCheckpoint.color = defaultCol;
					exitGame.color = hoverCol;
				}
			}
			else if (quitCheck)
			{
				if (itemSelected == 0) 
				{
					quitNo.color = hoverCol;
				}
				if (itemSelected == 1)
				{
					quitYes.color = hoverCol;
				}
			}
		}


		//accept button
		if (Input.GetKeyDown (KeyCode.JoystickButton0) && !quitCheck)
		{
			if (itemSelected == 0)  //Resume Game
			{
				PauseMenu.paused = false;	
				Screen.showCursor = false;	
				Time.timeScale = 1f;
				itemSelected = 0;
				PauseMenu.selecting = false;
			}
			if (itemSelected == 1) //Load Last Checkpoint
			{
				PauseMenu.paused = false;
				Time.timeScale = 1f;
				PauseMenu.restartedLevel = true;
				Screen.showCursor = false;
				itemSelected = 0;
				PauseMenu.selecting = false;
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
				itemSelected = 0;
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
			PauseMenu.paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
			PauseMenu.selecting = false;
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
			PauseMenu.paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
			PauseMenu.selecting = false;
		}
		if (Input.GetKeyDown(KeyCode.Joystick1Button6) && quitCheck) // back on quit menu
		{
			quitCheck = false;
			itemSelected = 2;
		}
		#endregion
	}

	#region Keyboard Event Inputs for pause menu
	public void Hover()
	{
		//Main options
		if (this.name == "Resume")
		{
			resumeGame.color = hoverCol;
		}
		if (gameObject.name == "Reload")
		{
			loadLastCheckpoint.color = hoverCol;
		}
		if (gameObject.name == "Exit")
		{
			exitGame.color = hoverCol;
		}

		//quit options
		if (this.name == "Yes")
		{
			quitYes.color = hoverCol;
		}
		if (gameObject.name == "No")
		{
			quitNo.color = hoverCol;
		}

	}

	public void DefaultCol()
	{
		if (this.name == "Resume")
		{
			resumeGame.color = defaultCol;
		}
		if (gameObject.name == "Reload")
		{
			loadLastCheckpoint.color = defaultCol;
		}
		if (gameObject.name == "Exit")
		{
			exitGame.color = defaultCol;
		}

		//quit options
		if (this.name == "Yes")
		{
			quitYes.color = defaultCol;
		}
		if (gameObject.name == "No")
		{
			quitNo.color = defaultCol;
		}
	}

	public void SelectOption()
	{
		if (this.name == "Resume")
		{
			StartCoroutine (Resuming());
		}
		if (gameObject.name == "Reload")
		{
			StartCoroutine (Reloading());
		}
		if (gameObject.name == "Exit")
		{
			StartCoroutine (Exiting());
		}

		//quit options
		if (this.name == "Yes")
		{
			StartCoroutine(YesQuit());
		}
		if (gameObject.name == "No")
		{
			StartCoroutine(NoQuit());
		}
	}

	private IEnumerator Resuming( )
	{
		resumeGame.color = pressCol;
		yield return new WaitForSeconds (buttonDelay);
		PauseMenu.PauseGame();
	}
	private IEnumerator Reloading( )
	{
		loadLastCheckpoint.color = pressCol;
		yield return new WaitForSeconds (buttonDelay);
		print ("Reloading Game");
	}
	private IEnumerator Exiting( )
	{
		exitGame.color = pressCol;
		PauseMenu.quitCheck = true;
		yield return new WaitForSeconds (buttonDelay);
		PauseMenu.quitCheck = true;
	}

	private IEnumerator YesQuit()
	{
		quitYes.color = pressCol;
		yield return new WaitForSeconds (buttonDelay);
		Application.Quit();
	}
	private IEnumerator NoQuit()
	{
		quitNo.color = pressCol;
		yield return new WaitForSeconds (buttonDelay);
		quitCheck = false;
	}


	#endregion


	//xbox controls
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
//		print ("start realtime wait");
		selecting = PauseMenu.selecting;
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + delay)
		{
//			print ("Delaying Input");
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + delay)
		{
//			print ("end realtime wait");
			PauseMenu.selecting =false;
			yield return null;
		}
		
	}

	void OnGUI() {
		if (PauseMenu.paused && PlayerV2.keyboardActive == false)
		{
			GUI.Label(new Rect(10, 10, 100, 20), "Item Selected " + itemSelected); //quick debug of what the controller is slecting
		}
	}

}
