using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class Player : MonoBehaviour
{
	// Script to handle player controls and most interactions

	public bool debugState = true;
	public bool isDead = false;

	public GameObject player;
	public bool playerOne;
	public Player otherPlayer; //this is so you can check if the other player is dead for start button restart
	public GameObject playerLight; //to be turned off when player dies
	public SpriteRenderer gravestone; //to be abled when player dies

	public SpriteRenderer UIspr;
	public int UIdisable = 2;
	float UIDefaultScale;
	public bool introSkipped = false;

	Rigidbody playerRigid;
	float movementForce;
	float jumpForce;

	public bool canMove = true;
	public float baseMovForce = 10f;
	public float baseJumpForce = 500f;
	public float slowTimer;
	public bool grounded = false;
	public bool rightFaced = true;
	public bool climbingLadder;
	private float h;
	private float v;
	private bool jumpingNow = false;

	//CI Rumble
	GameObject caveIn;
	float caveInDistance;
	float rumbleAmt;

	//SFX
	public AudioSource sfxRun;
	public AudioSource sfxJump;
	public AudioSource sfxLand;

	//xInput
//	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;

	public static PlayerIndex pone = PlayerIndex.One;
	public static PlayerIndex ptwo = PlayerIndex.Two;

	// Static variables for
	public 	static float h1 = 0.0f;
	public static float v1 = 0.0f;
	
	public static float h2 = 0.0f;
	public static float v2 = 0.0f;
	
	public static bool  buttonA = false;
	public static bool  buttonB = false;
	public static bool  buttonX = false;
	public static bool  buttonY = false;
	
	public static bool  dpadUp = false;
	public static bool  dpadDown = false;
	public static bool  dpadLeft = false;
	public static bool  dpadRight = false;
	
	public static bool  buttonStart = false;
	public static bool  buttonBack = false;
	
	public static bool  shoulderL = false;
	public static bool  shoulderR = false;
	
	public static bool  stickL = false;
	public static bool  stickR = false;
	
	public static float triggerL = 0.0f;
	public static float triggerR = 0.0f;

	public bool menuActive = false;
	public static bool keyboardActive = false;


	void OnLevelWasLoaded(int level) {
		print ("Level: " + level);

		if (level == 0)
		{
			menuActive = true;
			print ("Menu Loaded");
		}

		if (level >= 1)
		{
			menuActive = false;
		}
		
	}


	void Start(){
		UIDefaultScale = UIspr.transform.localScale.x;
		player = this.gameObject;
		playerRigid = GetComponent<Rigidbody>();
		movementForce = baseMovForce;
		jumpForce = baseJumpForce;
		Flip ();

		if (debugState)
		{
			Debug.LogWarning ("Debug state = true");
		}
	}

	void Update(){
		//Disable UI after 2 button press
		if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released){
			UIdisable --;
		}
		if (UIdisable <= 0){
			UIspr.enabled = false;
		}

		//Play cave in audio if cave in has started
		if (caveIn != null){
			CaveInRumble();
		}

		//Reset soreks movement to normal speed after lighting fire
		if (movementForce < baseMovForce) {
			slowTimer -= Time.deltaTime;

			if (slowTimer <= 0){
				movementForce = baseMovForce;
				jumpForce = baseJumpForce;
			}
		}

		//Get last updates input states, then refresh to new
		prevState = state;
		state = GamePad.GetState(playerIndex);
		state = GamePad.GetState ( playerIndex );
		
		// Set vibration according to triggers
		if (playerIndex == pone)
		{
			padVibration(pone, state.Triggers.Left, state.Triggers.Right);
		}
		if (playerIndex == ptwo){
			padVibration(ptwo, state.Triggers.Left, state.Triggers.Right);
		}

		//Menu input. This is here because the player scripts are required for xinput to work.
		if (menuActive)
		{
			if (state.Buttons.Start  == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
			{
				print ("start in menu");
				if (playerIndex == pone)
				{
					print ("P1, start in menu");
					if (GameObject.Find("P1").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = true;
					}
					else
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = false;
					}
				}

				if (playerIndex == ptwo){
					//Player 2 toggle
					print ("P2, start in menu");
					if (GameObject.Find("P2").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = true;
					}
					else
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = false;
					}
				}
			}
		}

		//START BUTTON
		if ( !menuActive && Intro.watchIntro == true && state.Buttons.Start  == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
		{
			if (Intro.introTimer > 0f && !introSkipped){
				if (!otherPlayer.introSkipped){
					Intro.Skip();
					introSkipped = true;
				}
			}
		}

		//Restart level if other player isdead. Go to menu if neither are dead. Dead player cant press start.
		if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released) {
			if (Intro.introTimer < -2f){
				if (otherPlayer.isDead) {
					Application.LoadLevel("Level_1");
				} else if (!otherPlayer.isDead && !isDead){
					Application.LoadLevel("MenuPlaceholder");
				}
			}
		}

		//Stop running sound instantly when jumping or climbing
		if (!grounded && sfxRun.isPlaying) {
			sfxRun.Stop ();
		}
		
	}


	public static void  padVibration (  PlayerIndex playerIndex ,   float big ,   float small   ){
		GamePad.SetVibration ( playerIndex, big, small );
	}
	
	public static void  stopPadVibration (  PlayerIndex playerIndex   ){
		GamePad.SetVibration( playerIndex, 0, 0 );
	}

	void FixedUpdate ()
	{

		// Function calls
		Grounded ();
		if (canMove && !menuActive) {
			if (!isDead){
				Controls ();
			}
		}

		if (debugState)
		{
			Debug.Log ("Player Grounded: "+ grounded);
		}
	}

	// Flip sprite depending on direction
	private void Flip()
	{
		if (rightFaced)
		{
			Vector3 spriteScale = transform.localScale;
			spriteScale.x = 1;
			transform.localScale = spriteScale;
			UIspr.transform.localScale = new Vector3 (-UIDefaultScale, UIspr.transform.localScale.y, UIspr.transform.localScale.z);
		}

		if (!rightFaced)
		{
			Vector3 theScale = transform.localScale;
			theScale.x = -1;
			transform.localScale = theScale;
			UIspr.transform.localScale = new Vector3 (UIDefaultScale, UIspr.transform.localScale.y, UIspr.transform.localScale.z);
		}

	}

	// Grounded check
	private void Grounded()
	{
		Vector3 leftExtent = new Vector3 (this.transform.position.x - collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);
		Vector3 rightExtent = new Vector3 (this.transform.position.x + collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);

		if (Physics.Raycast (this.transform.position, Vector3.down, 1.5f) || Physics.Raycast (leftExtent, Vector3.down,  1.5f)|| Physics.Raycast (rightExtent, Vector3.down,  1.5f))
		{
			if (!grounded){
				sfxLand.Play();
			}
			grounded = true;
		} 
		else 
		{
			grounded = false;
		}


	}

	// Controls
	private void Controls()
	{
				//---------------------------------------------------------------------------------//

		///
		//Everything keyboard controls related
		///
		if (keyboardActive)
		{
			//Keyboard
			//Right
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			} 
			// Left
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (-1) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}

			// Jump
			if ((grounded == true) && Input.GetKey(KeyCode.Space) && (!climbingLadder))
			{
				playerRigid.AddForce(Vector3.up * 100, ForceMode.Impulse);
				sfxJump.Play();
			}

			//climb
			if (climbingLadder)
			{
				playerRigid.useGravity = false;
				playerRigid.velocity = Vector3.zero;
				//Keyboard
				// Ascend
				if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.UpArrow))
				{
					player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
				}
				// Descend
				else if (Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.DownArrow))
				{
					player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (-1) * Time.deltaTime);
				}
			}
			else playerRigid.useGravity = true;

		}//end of keyboard controls


				//---------------------------------------------------------------------------------//

		///
		//Xbox controls in here
		///
		if (!keyboardActive)
			{
			//Xbox support
			// Right
			//print (state.ThumbSticks.Left.X);
			if (state.ThumbSticks.Left.X > 0.3f )
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (state.ThumbSticks.Left.X) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (state.DPad.Right == ButtonState.Pressed)
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			
			// Left
			if (state.ThumbSticks.Left.X < -0.3f)
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (state.ThumbSticks.Left.X) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (state.DPad.Left == ButtonState.Pressed)
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}

			
			//Jump xbox controls
			if ((grounded == true) && !jumpingNow && state.Buttons.A == ButtonState.Pressed  && (!climbingLadder) ||
			    (grounded == true) && !jumpingNow && state.DPad.Up == ButtonState.Pressed && (!climbingLadder)) 
			{
				StartCoroutine(JumpCoolDown());
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
				sfxJump.Play();
			}
			if (debugState)
			{
				Debug.Log("Jumping "+jumpingNow);
				
				//			if (playerIndex == pone)
				//				Debug.Log("Player one jumped");
				//
				//			if (playerIndex == ptwo)
				//				Debug.Log("Player two jumped");
			}

			// Climb
			if (climbingLadder)
			{
				playerRigid.useGravity = false;
				playerRigid.velocity = Vector3.zero;
				//xbox
				//Ascend
				if (state.ThumbSticks.Left.Y > 0.1f)
				{
					player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (state.ThumbSticks.Left.Y) * Time.deltaTime);
				}
				else if (state.DPad.Up == ButtonState.Pressed)
				{
					player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
				}
				
				
				// Descend
				if (state.ThumbSticks.Left.Y < 0.1f)
				{
					player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (state.ThumbSticks.Left.Y) * Time.deltaTime);
				}
				if (state.DPad.Down == ButtonState.Pressed)
				{
					print ("this doesnt work and is not needed for playtest, leaving for later");
					player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (-1) * Time.deltaTime);
				}
			}
			else playerRigid.useGravity = true;

		}//end of xbox controls


	}//end of controls void

	public IEnumerator JumpCoolDown()
	{
		jumpingNow = true;
		float delay = 0.5f;
		yield return new WaitForSeconds(delay);
		jumpingNow = false;
	}

	// Sendmessage receiver for ladder state
	void Ladder(float ladderState)
	{
		float i = ladderState;

		if (i == 1)
		{
			climbingLadder = true;
		}

		else climbingLadder = false;

		if (!climbingLadder)
		{
			i = 0;
		}
	}

	void CanMove(bool move){
		if (move == false) {
			canMove = false;
		} else {
			canMove = true;
		}           
	}

	//Generally used for sorek when using his lamp
	void Slow(){
		movementForce = baseMovForce / 2.5f;
		jumpForce = baseJumpForce / 1.25f;

		slowTimer = 0.75f;
	}

	void CaveInStart(GameObject caveInObj){
		caveIn = caveInObj;
	}

	void CaveInRumble(){
		caveInDistance = Vector3.Distance (transform.position, caveIn.transform.position);
		rumbleAmt = Mathf.Lerp (1f, 0f, (caveInDistance / 20));

		if (caveInDistance > 10){
			if (playerIndex == pone){
				padVibration(pone, 0, rumbleAmt);
			}
			if (playerIndex == ptwo){
				padVibration(ptwo, 0, rumbleAmt);
			}
		} else {
			if (playerIndex == pone){
				padVibration(pone, rumbleAmt, rumbleAmt);
			}
			if (playerIndex == ptwo){
				padVibration(ptwo, rumbleAmt, rumbleAmt);
			}
		}
	}

	void UIenable(){
		UIspr.enabled = true;
	}

	void Crushed(){
		isDead = true;
		canMove = false;
		playerLight.SetActive (false);
		gravestone.enabled = true;
	}
}
