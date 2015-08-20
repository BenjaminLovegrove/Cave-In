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
	public bool PauseMenu.quitCheck;
	float selectDely = 0.2f;


	// Use this for initialization
	void Start () {
		defaultCol = Color.white;
		hoverCol = Color.red;
		pressCol = Color.green;
		selecting = PauseMenu.selecting;
		//quitCheck = PauseMenu.quitCheck;
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
			if (!PauseMenu.quitCheck)
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
			else if (PauseMenu.quitCheck)
			{
				if (itemSelected == 0) 
				{
					quitYes.color = defaultCol;
					quitNo.color = hoverCol;
				}
				if (itemSelected == 1)
				{
					quitYes.color = hoverCol;
					quitNo.color = defaultCol;
				}
			}
		}
		if (this.name == "Resume")
		{

		}
		if (gameObject.name == "Reload")
		{

		}
		if (gameObject.name == "Exit")
		{

		}
		
		//quit options
		if (this.name == "Yes")
		{

		}
		if (gameObject.name == "No")
		{

		}

		//accept button
		if (Input.GetKeyDown (KeyCode.JoystickButton0) && !PauseMenu.quitCheck)
		{
			if (itemSelected == 0)  //Resume Game
			{
				StartCoroutine (Resuming());
//				PauseMenu.paused = false;	
//				Screen.showCursor = false;	
//				Time.timeScale = 1f;
//				itemSelected = 0;
//				PauseMenu.selecting = false;
			}
			if (itemSelected == 1) //Load Last Checkpoint
			{
				StartCoroutine (Reloading());
//				PauseMenu.paused = false;
//				Time.timeScale = 1f;
//				PauseMenu.restartedLevel = true;
//				Screen.showCursor = false;
//				itemSelected = 0;
//				PauseMenu.selecting = false;
//				Application.LoadLevel(1);
			} 
			if (itemSelected == 2) //Exit Game
			{
				StartCoroutine (Exiting());
//				quitCheck = true;
				itemSelected = 0;
			} 
		}
		else if (Input.GetKeyDown (KeyCode.JoystickButton0) && PauseMenu.quitCheck)
		{
			if (itemSelected == 0)  //NO
			{
				StartCoroutine(NoQuit());
//				quitCheck = false;
				itemSelected = 0;
			}
			if (itemSelected == 1) //YES
			{
				StartCoroutine(YesQuit());
//				Application.Quit();
				Debug.LogWarning ("Player Quit The Game");
			} 
		}
		
		//Back Buttons
		if (Input.GetKeyDown (KeyCode.Joystick1Button1) && !PauseMenu.quitCheck) // B on menu
		{
			PauseMenu.paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
			PauseMenu.selecting = false;
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button1) && PauseMenu.quitCheck) // B on quit menu
		{
			PauseMenu.quitCheck = false;
		}
		
		//		if (Input.GetKeyDown (KeyCode.Joystick1Button7) && !quitCheck) //start on menu
		//		{
		//			paused = false;	
		//			Screen.showCursor = false;	
		//			Time.timeScale = 1f;
		//		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button7) && PauseMenu.quitCheck) //start on quit menu
		{
			if (itemSelected == 0)  //NO
			{
				PauseMenu.quitCheck = false;
				itemSelected = 2;
			}
			if (itemSelected == 1) //YES
			{
				PauseMenu.quitCheck = false;
				Application.Quit();
				Debug.LogWarning ("Player Quit The Game");
			} 
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick1Button6) && !PauseMenu.quitCheck) // back on menu
		{
			print ("this running?");
			PauseMenu.paused = false;	
			Screen.showCursor = false;	
			Time.timeScale = 1f;
			itemSelected = 0;
			PauseMenu.selecting = false;
		}
		if (Input.GetKeyDown(KeyCode.Joystick1Button6) && PauseMenu.quitCheck) // back on quit menu
		{
			print (" or isthis running?");
			StartCoroutine(NoQuit());
			//quitCheck = false;
			itemSelected = 0;
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
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + buttonDelay)
		{
			resumeGame.color = pressCol;
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + buttonDelay)
		{
				PauseMenu.PauseGame();
			yield return null;
		}
	}
	private IEnumerator Reloading( )
	{
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + buttonDelay)
		{
			loadLastCheckpoint.color = pressCol;
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + buttonDelay)
		{
			PauseMenu.paused = false;
			Time.timeScale = 1f;
			PauseMenu.restartedLevel = true;
			Screen.showCursor = false;
			itemSelected = 0;
			PauseMenu.selecting = false;
			Application.LoadLevel(1);
			yield return null;
		}
	}
	private IEnumerator Exiting( )
	{
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + buttonDelay)
		{
			exitGame.color = pressCol;
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + buttonDelay)
		{
			PauseMenu.quitCheck = true;
			yield return null;
		}
	}

	private IEnumerator YesQuit()
	{
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + buttonDelay)
		{
			quitYes.color = pressCol;
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + buttonDelay)
		{
			Application.Quit();
			yield return null;
		}
	}
	private IEnumerator NoQuit()
	{
		float start = Time.realtimeSinceStartup;
		while ( Time.realtimeSinceStartup < start + buttonDelay)
		{
			quitNo.color = pressCol;
			yield return null;
		}
		if ( Time.realtimeSinceStartup > start + buttonDelay)
		{
			PauseMenu.quitCheck = false;
			yield return null;
		}
	}


	#endregion


	//xbox controls
	private IEnumerator SelectionIncrease()
	{
		//	print ("increase start");
		StartCoroutine (WaitForRealSeconds(selectDely, selecting));
		itemSelected ++;
		if (!PauseMenu.quitCheck)
		{
			if (itemSelected == 3)
			{
				itemSelected = 0;
			}
		}
		else if (PauseMenu.quitCheck)
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
		if (!PauseMenu.quitCheck)
		{
			if (itemSelected == -1)
			{
				itemSelected = 2;
			}
		}
		else if (PauseMenu.quitCheck)
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
