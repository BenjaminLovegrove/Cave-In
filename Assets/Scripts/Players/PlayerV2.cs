﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class PlayerV2 : MonoBehaviour
{

	public Controller xInput;
	// Script to handle player controls and most interactions
	
	public bool debugState = true;
	public bool isDead = false;
	public Animator anim;
	
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
	float groundedRayDist = 1.5f; //Need to set this variable different as they have different heights now.
	
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

	#region Determine Player Index Variables
	//xInput
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;
	public static PlayerIndex pone = PlayerIndex.One;
	public static PlayerIndex ptwo = PlayerIndex.Two;
	#endregion
	
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
		xInput = new Controller();// makes it easier to write code similar to unity's Input. stuff
		//Change raycast distance for taller player
		if (!playerOne) {
			groundedRayDist = 2.2f;
			xInput.playerIndex = PlayerIndex.Two;
//			xInput.currDebug = Controller.Debug_Types.BUTTON;
//			Debug.Log ("Starting player two");
		}
		else
		{
			xInput.playerIndex = PlayerIndex.One;
//			xInput.currDebug = Controller.Debug_Types.BUTTON;
//			Debug.Log ("Starting player one");
		}
		
		if (!menuActive)
		{
			UIDefaultScale = UIspr.transform.localScale.x;
			PauseMenu.canPauseGame = true;
		}
		player = this.gameObject;
		playerRigid = GetComponent<Rigidbody>();
		movementForce = baseMovForce;
		jumpForce = baseJumpForce;
		Flip ();
		
		if (debugState)
		{
			Debug.LogWarning ("Debug state = true");
		}
		
		anim = GetComponentInChildren<Animator> ();
	}
	
	void Update(){
		//Disable UI after 2 button press
		if (xInput.OnButtonDownB){
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

		#region States are now handled in the Controller script
//		//Get last updates input states, then refresh to new
//		prevState = state;
//		state = GamePad.GetState(playerIndex);
//		state = GamePad.GetState ( playerIndex );
		#endregion

		#region Ready State Toggle For Menu
		//Menu input. This is here because the player scripts are required for xinput to work.
		if (menuActive)
		{
			if (xInput.OnButtonDownStart && !keyboardActive)
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
			if (Input.GetKeyDown (KeyCode.Return) && keyboardActive)
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
		#endregion

		#region Various Start Button Actions Throughout The Game
		//START BUTTON
		if (!keyboardActive && !menuActive && Intro.watchIntro == true && xInput.OnButtonDownStart && Intro.skipNum <= 2)
		{
			print("Start to skip");
			Intro.skipNum ++;
			UI.displaySkip ++;
			if (Intro.skipNum >= 2 && Intro.introTimer > 0f && !introSkipped){
				if (!otherPlayer.introSkipped){
					Intro.Skip();
					introSkipped = true;
				}
			}
		}
		//Restart level if other player isdead. Go to menu if neither are dead. Dead player cant press start.
		if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released || Input.GetKeyDown(KeyCode.Escape)) {
			if (Intro.introTimer < -2f){
				if (otherPlayer.isDead) {
					Application.LoadLevel("Level_1");
				} 
			}
		}
		#endregion
		
		//Stop running sound instantly when jumping or climbing
		if (!menuActive && !grounded && sfxRun.isPlaying) {
			sfxRun.Stop ();
		}
		
	}

	#region Vibration is now handled by the Controller script
	//call this to set vibration on a platers controller
	/*public static void  padVibration (  PlayerIndex playerIndex ,   float big ,   float small   ){
		GamePad.SetVibration ( playerIndex, big, small );
	}
	
	//call this to stop all vibration
	public static void  stopPadVibration (  PlayerIndex playerIndex   ){
		GamePad.SetVibration( playerIndex, 0, 0 );
	}*/
	#endregion
	
	void FixedUpdate ()
	{
		xInput.FixedUpdate();
		Animate ();

		#region Calls the controls function which contains all movement
		if (!menuActive)
		{
			// Function calls
			Grounded ();
			if (canMove) {
				if (!isDead){
					Controls ();
				}
			}
			
			if (debugState)
			{
				Debug.Log ("Player Grounded: "+ grounded);
			}
		}
		#endregion
	}

	#region Flip character sprite
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
	#endregion

	#region Grounded Check
	// Grounded check
	private void Grounded()
	{
		Vector3 leftExtent = new Vector3 (this.transform.position.x - collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);
		Vector3 rightExtent = new Vector3 (this.transform.position.x + collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);
		
		if (Physics.Raycast (this.transform.position, Vector3.down, groundedRayDist) || Physics.Raycast (leftExtent, Vector3.down,  groundedRayDist)|| Physics.Raycast (rightExtent, Vector3.down,  groundedRayDist))
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
	#endregion
	
	// Controls
	private void Controls()
	{
		//---------------------------------------------------------------------------------//
		///
		#region Everything keyboard controls related
		///
		if (keyboardActive)
		{
			if (player.name == "Sorek_Jarred")
			{
				//Keyboard
				//Right
				if (Input.GetKey(KeyCode.D))
				{
					player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
					rightFaced = true;
					Flip ();
					if (grounded && !sfxRun.isPlaying){
						sfxRun.Play();
					}
				} 
				// Left
				if (Input.GetKey(KeyCode.A))
				{
					player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (-1) * Time.deltaTime);
					rightFaced = false;
					Flip ();
					if (grounded && !sfxRun.isPlaying){
						sfxRun.Play();
					}
				}
				
				// Jump
				if ((grounded == true) && Input.GetKey(KeyCode.LeftShift) && (!climbingLadder))
				{
					print ("P1_Jumped_LeftShift");
					player.rigidbody.AddForce(Vector3.up * 100, ForceMode.Impulse);
					sfxJump.Play();
				}
				
				//climb
				if (climbingLadder)
				{
					playerRigid.useGravity = false;
					playerRigid.velocity = Vector3.zero;
					//Keyboard
					// Ascend
					if (Input.GetKey(KeyCode.W))
					{
						player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
					}
					// Descend
					else if (Input.GetKey(KeyCode.S))
					{
						player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (-1) * Time.deltaTime);
					}
				}
				else playerRigid.useGravity = true;
			}
			
			if (player.name == "Hickory")
			{
				//Keyboard
				//Right
				if (Input.GetKey(KeyCode.Quote))
				{
					print ("quote - move right");
					player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
					rightFaced = true;
					Flip ();
					if (grounded && !sfxRun.isPlaying){
						sfxRun.Play();
					}
				} 
				// Left
				if (Input.GetKey(KeyCode.L))
				{
					print ("L - move left");
					player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (-1) * Time.deltaTime);
					rightFaced = false;
					Flip ();
					if (grounded && !sfxRun.isPlaying){
						sfxRun.Play();
					}
				}
				
				// Jump
				if ((grounded == true) && Input.GetKey(KeyCode.RightShift) && (!climbingLadder))
				{
					print ("P2_Jumped_RightShift");
					player.rigidbody.AddForce(Vector3.up * 100, ForceMode.Impulse);
					sfxJump.Play();
				}
				
				//climb
				if (climbingLadder)
				{
					playerRigid.useGravity = false;
					playerRigid.velocity = Vector3.zero;
					//Keyboard
					// Ascend
					if (Input.GetKey(KeyCode.P))
					{
						print ("P - acsend climb");
						player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
					}
					// Descend
					else if (Input.GetKey(KeyCode.Semicolon))
					{
						print ("semicolon - decend climb");
						player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (-1) * Time.deltaTime);
					}
				}
				else playerRigid.useGravity = true;
			}
			
			
			
		}//end of keyboard controls
		#endregion
		
		//---------------------------------------------------------------------------------//
		
		///
		#region Xbox controls in here
		///
		if (!keyboardActive)
		{
			//Xbox support
			// Right
			//print (state.ThumbSticks.Left.X);
			if (xInput.ThumbStickL_X > 0.3f )
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (xInput.ThumbStickL_X) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (xInput.OnButton_DpadRight)
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			
			// Left
			if (xInput.ThumbStickL_X < -0.3f)
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (xInput.ThumbStickL_X) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (xInput.OnButton_DpadLeft)
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			
			
			//Jump xbox controls
			if ((grounded == true) && !jumpingNow && xInput.OnButtonDownA && (!climbingLadder) ||
			    (grounded == true) && !jumpingNow && xInput.OnButtonDown_DpadUp && (!climbingLadder)) 
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
				if (xInput.ThumbStickL_Y > 0.1f)
				{
					player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (xInput.ThumbStickL_Y) * Time.deltaTime);
				}
				else if (xInput.OnButton_DpadUp)
				{
					player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
				}
				
				
				// Descend
				if (xInput.ThumbStickL_Y < 0.1f)
				{
					player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (xInput.ThumbStickL_Y) * Time.deltaTime);
				}
				if (xInput.OnButton_DpadDown)
				{
					player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (-1) * Time.deltaTime);
				}
			}
			else playerRigid.useGravity = true;
			
		}//end of xbox controls
		#endregion
		
	}//end of controls void
	
	public IEnumerator JumpCoolDown()
	{
		jumpingNow = true;
		float delay = 0.55f;
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
		
		slowTimer = 1f;
	}
	
	void CaveInStart(GameObject caveInObj){
		caveIn = caveInObj;
	}
	
	void CaveInRumble(){
		caveInDistance = Vector3.Distance (transform.position, caveIn.transform.position);
		rumbleAmt = Mathf.Lerp (1f, 0f, (caveInDistance / 20));

		if (caveInDistance > 10){
			if (playerIndex == pone){
				xInput.padVibration(pone, 0, rumbleAmt);
			}
			if (playerIndex == ptwo){
				xInput.padVibration(ptwo, 0, rumbleAmt);
			}
		} else {
			if (playerIndex == pone){
				xInput.padVibration(pone, rumbleAmt, rumbleAmt);
			}
			if (playerIndex == ptwo){
				xInput.padVibration(ptwo, rumbleAmt, rumbleAmt);
			}
		}
	}
	
	void UIenable(){
		if (!keyboardActive)
		{
			UIspr.enabled = true;
		}
	}
	
	void Crushed(){
		isDead = true;
		canMove = false;
		playerLight.SetActive (false);
		gravestone.enabled = true;
	}
	
	void Animate(){
		//Soreks animations (note the player one bool)
		if (Intro.introTimer < 0f && playerOne) {
			if (!grounded && !climbingLadder) {
				anim.SetBool ("Jump", true);
			} else {
				anim.SetBool ("Jump", false);
			}
			
			if (grounded) {
				if (xInput.ThumbStickL_X < -0.3f || xInput.ThumbStickL_X > 0.3f || xInput.OnButton_DpadLeft || xInput.OnButton_DpadRight) {
					anim.SetBool ("Idle", false);
					if (slowTimer > 0f) {
						anim.SetBool ("Run", false);
						anim.SetBool ("Walkandpour", true);
						anim.SetBool ("Idlepour", false);
					} else {
						anim.SetBool ("Walkandpour", false);
						anim.SetBool ("Run", true);
						anim.SetBool ("Idlepour", false);
					}
				} else {
					if (slowTimer > 0f) {
						anim.SetBool ("Run", false);
						anim.SetBool ("Idle", false);
						anim.SetBool ("Jump", false);
						anim.SetBool ("Walkandpour", false);
						anim.SetBool ("Idlepour", true);
					} else {
						anim.SetBool ("Run", false);
						anim.SetBool ("Idle", true);
						anim.SetBool ("Walkandpour", false);
						anim.SetBool ("Idlepour", false);
					}
				}
			} else {
				anim.SetBool ("Run", false);
				anim.SetBool ("Idle", false);
				anim.SetBool ("Walkandpour", false);
				anim.SetBool ("Idlepour", false);
			}
			
			if (climbingLadder) {
				anim.SetBool ("Run", false);
				anim.SetBool ("Idle", true);
				anim.SetBool ("Jump", false);
				anim.SetBool ("Walkandpour", false);
				anim.SetBool ("Idlepour", false);
			}
		}
		
		//Hickorys animations (note the player one bool)
		if (!menuActive && !playerOne) {
			if (!grounded && !climbingLadder) {
				anim.SetBool ("Idle", true); //to be replaced with a jump
			} else {
				//anim.SetBool ("Jump", false);
			}
			
			if (grounded) {
				if (xInput.ThumbStickL_X < -0.3f || xInput.ThumbStickL_X > 0.3f || xInput.OnButton_DpadLeft || xInput.OnButton_DpadRight) {
					anim.SetBool ("Run", true);
					anim.SetBool ("Idle", false);
					if (xInput.OnButtonB) {
						anim.SetBool ("Run", false);
						//anim.SetTrigger ("Swing"); set by pickaxe script
					}
				} else {
					anim.SetBool ("Run", false);
					anim.SetBool ("Idle", true);
				}
			} else {
				anim.SetBool ("Run", false);
				anim.SetBool ("Idle", false);
			}
			
			if (climbingLadder) {
				anim.SetBool ("Run", false);
				anim.SetBool ("Idle", true);
				//anim.SetBool ("Jump", false);
			}
		}
		
		
		
		
	}
}
