using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure; // Required in C#
using CobaltMetrics;
using CobaltMetrics.DataTypes.Unity;

public class PlayerV2 : MonoBehaviour
{

	public Controller xInput; //grab the Controller script that handles all xbox movement
	// Script to handle player controls and most interactions
	
	public bool debugState = true;
	public bool isDead = false;
	public Animator anim;
	
	public GameObject player;
	public bool playerOne;
	public PlayerV2 otherPlayer; //this is so you can check if the other player is dead for start button restart
	public GameObject playerLight; //to be turned off when player dies
	public SpriteRenderer gravestone; //to be abled when player dies
	public SpriteRenderer playerSpr;
	
	public SpriteRenderer UIspr;
	public int UIdisable = 2;
	float UIDefaultScale;
	public bool introSkipped = false;

	Vector3 playerScreenPoint;
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
	bool startFlipped = false;
	
	//CI Rumble
	GameObject caveIn;
	float caveInDistance;
	float rumbleAmt;
	
	//SFX
	public AudioSource sfxRun;
	public AudioSource sfxJump;
	public AudioSource sfxLand;

	public GameObject waterSplash, waterExitSplash;

	#region Determine Player Index Variables
	//xInput
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;
	public static PlayerIndex pone = PlayerIndex.One;
	public static PlayerIndex ptwo = PlayerIndex.Two;
	#endregion
	
	public bool menuActive = false;
	private bool menuDefaultAnimation = false;
	private bool menuReadyAnimation = false;
	private bool swingInMenu = false;
	//private bool currentlyAnimating = false;
	public static bool keyboardActive = false;

	private float m_metricPushTimer = 2f; //Metrics


	void OnLevelWasLoaded(int level) {
		//print ("Level: " + level);
		
		if (level == 0)
		{
			menuActive = true;
		}
		
		if (level >= 1)
		{
			menuActive = false;
		}
		
	}

	void Awake()
	{
		anim = GetComponentInChildren<Animator> ();
	}

	void Start(){

		xInput = new Controller();// Creates a new instance of the Controller script

		#region Setup each instance of the Controller script according to each player
		if (!playerOne) {
			groundedRayDist = 2.2f;//Change raycast distance for taller player
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
		#endregion

		if (menuActive) //if menu is active
		{
			anim.SetBool ("Run", true);
			rightFaced = false;
			grounded = true;
			menuDefaultAnimation = true;
		}
		else if (!menuActive) //if menu is NOT active
		{
			UIDefaultScale = UIspr.transform.localScale.x;
			menuDefaultAnimation = false;
			grounded = false;
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
	}
	
	void Update(){

		if (isDead){
			Controller.xInput.stopPadVibration(pone);
			Controller.xInput.stopPadVibration(ptwo);
		}

		//Metrics Begin
//		if(canMove)
//		{
//			m_metricPushTimer -= Time.deltaTime;
//
//			if(m_metricPushTimer <= 0f)
//			{
//				m_metricPushTimer = 5f;
//
//				if(playerOne)
//				{
//					CMetricVector.Vector3("p1Pos", transform.position);
//				}
//				else
//				{
//					CMetricVector.Vector3("p2Pos", transform.position);
//				}
//			}
//		}
		//Metrics End

		//DeathStuff
		if (isDead) {
			playerSpr.enabled = false;
		} else {
			playerSpr.enabled = true;
		}
		if (otherPlayer.isDead && canMove) {
			canMove = false;
			//Metrics Begin
			//CMetricVector.Vector3("p1DeathPos", transform.position);
			//CMetricVector.Vector3("p2DeathPos", transform.position);
			//Metrics End
		}


		//Flip Hickory when rock falls
		if (Intro.introTimer <= (Intro.startIntroTimer * 0.09f) && startFlipped == false && !playerOne){
			rightFaced = false;
			Flip ();
			startFlipped = true;
		}

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


		#region Ready State Toggle For Menu -Keyboard-
		//Menu input. This is here because the player scripts are required for xinput to work.
		if (menuActive)
		{
			if (Input.GetKeyDown (KeyCode.Return) && keyboardActive || Input.GetKeyDown (KeyCode.KeypadEnter)) //keyboard controls for menu ready
			{
				if (playerOne)
				{
					print ("Players hit start");
					if (GameObject.Find("P1").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = true;
						menuDefaultAnimation = false;
						menuReadyAnimation = true;
					}
					else
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = false;
						menuDefaultAnimation = true;
						menuReadyAnimation = false;
					}

					//Player 2 toggle
					if (GameObject.Find("P2").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = true;
						menuDefaultAnimation = false;
						menuReadyAnimation = true;
					}
					else
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = false;
						menuDefaultAnimation = true;
						menuReadyAnimation = false;
					}
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
		xInput.FixedUpdate(); //call the new instance of the Controller script FixedUpdate
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

		#region Ready State Toggle For Menu -Xbox-
		//Menu input. This is here because the player scripts are required for xinput to work.
		if (menuActive)
		{
			if (xInput.OnButtonDownStart && !keyboardActive)//xbox controls for menu
			{
				if (playerOne)
				{
					if (GameObject.Find("P1").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = true;
						menuDefaultAnimation = false;
						menuReadyAnimation = true;
						StartCoroutine(MenuFireAnimation());
				//make it rumble for feedback
					}
					else
					{
						GameObject.Find("P1").GetComponent<Toggle>().isOn = false;
						menuDefaultAnimation = true;
						menuReadyAnimation = false;
						if (!swingInMenu){anim.SetBool ("Run", true);}
					}
				}
				
				if (!playerOne){
					//Player 2 toggle
					if (GameObject.Find("P2").GetComponent<Toggle>().isOn == false)
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = true;
						menuDefaultAnimation = false;
						menuReadyAnimation = true;
						//make it rumble for feedback

					}
					else
					{
						GameObject.Find("P2").GetComponent<Toggle>().isOn = false;
						menuDefaultAnimation = true;
						menuReadyAnimation = false;
						anim.SetBool ("Run", true);
					}
				}
			}
		}
		#endregion

		#region Various Start Button Actions Throughout The Game
		//START BUTTON
		if (!keyboardActive && !menuActive && Intro.watchIntro == true && xInput.OnButtonDownStart && Intro.skipNum <= 3 && PauseMenu.canPauseGame == false)
		{
			Intro.skipNum ++;
			UI.displaySkip ++;
			if (Intro.skipNum >= 3 && Intro.introTimer > 0f && !introSkipped){
				Intro.Skip();
				if (!otherPlayer.introSkipped){
					Intro.Skip();
					introSkipped = true;
				}
			}
		}
		//Restart level if other player isdead. Go to menu if neither are dead. Dead player cant press start.
		if (xInput.OnButtonDownStart && PauseMenu.canPauseGame == false || Input.GetKeyDown(KeyCode.Escape) && PauseMenu.canPauseGame == false) {
				if (otherPlayer.isDead) {
					Application.LoadLevel(1);
				} 
			
		}


		//pause the game
		if (xInput.OnButtonDownStart && PauseMenu.canPauseGame == true &&(Intro.introTimer < 0))
		{
			PauseMenu.PauseGame();
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
		playerScreenPoint = Camera.main.WorldToScreenPoint (transform.position);
		//---------------------------------------------------------------------------------//
		///
		#region Keyboard controls
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
				if ((grounded == true) && !jumpingNow && Input.GetKey(KeyCode.LeftShift) && (!climbingLadder))
				{

					player.rigidbody.AddForce(Vector3.up * 400, ForceMode.Impulse);
					StartCoroutine(JumpCoolDown());
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
					player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (-1) * Time.deltaTime);
					rightFaced = false;
					Flip ();
					if (grounded && !sfxRun.isPlaying){
						sfxRun.Play();
					}
				}
				
				// Jump
				if ((grounded == true) && !jumpingNow && Input.GetKey(KeyCode.RightShift) && (!climbingLadder))
				{
					player.rigidbody.AddForce(Vector3.up * 400, ForceMode.Impulse);
					StartCoroutine(JumpCoolDown());
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
						player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (1) * Time.deltaTime);
					}
					// Descend
					else if (Input.GetKey(KeyCode.Semicolon))
					{
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
			if (xInput.ThumbStickL_X > 0.3f && playerScreenPoint.x < (Screen.width * 0.95))
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (xInput.ThumbStickL_X) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (xInput.OnButton_DpadRight && playerScreenPoint.x < (Screen.width * 0.95))
			{
				player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = true;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			
			// Left
			if (xInput.ThumbStickL_X < -0.3f && playerScreenPoint.x > (Screen.width * 0.05))
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (xInput.ThumbStickL_X) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			else if (xInput.OnButton_DpadLeft && playerScreenPoint.x > (Screen.width * 0.05))
			{
				player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (1) * Time.deltaTime);
				rightFaced = false;
				Flip ();
				if (grounded && !sfxRun.isPlaying){
					sfxRun.Play();
				}
			}
			
			
			//Jump xbox controls
			if ((grounded == true) && !jumpingNow && xInput.OnButtonDownA && (!climbingLadder)) 
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

	#region Jump cooldown
	public IEnumerator JumpCoolDown()
	{
		jumpingNow = true;
		float delay = 0.55f;
		yield return new WaitForSeconds(delay);
		jumpingNow = false;
	}
	#endregion

	#region Climb Ladder State
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
	#endregion
	
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
		if (!otherPlayer.isDead && !isDead){
			Intro.ci1difficulty = Intro.ci1difficulty * 0.9f;
			Intro.ci2difficulty = Intro.ci2difficulty * 0.9f;
		}

		isDead = true;
		PauseMenu.canPauseGame = false;
		canMove = false;
		playerLight.SetActive (false);
		gravestone.enabled = true;
		Intro.gameStarted = false;
		Intro.caveInStarted = false;
		Intro.introTimer = 15;
		Camera.main.SendMessage("Death", new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z));
	}

	#region Animation
	void Animate(){

		//Hickory menu animtion
		if (menuActive && !menuDefaultAnimation && menuReadyAnimation && !playerOne)
		{
			anim.SetBool ("Run", false);
			anim.SetTrigger ("Swing");
		}
		else if (menuActive && menuDefaultAnimation && !menuReadyAnimation && playerOne && !swingInMenu)
		{
			anim.SetBool ("Run", true);
		}


		//Soreks animations (note the player one bool)
		if (Intro.introTimer < 0f && playerOne) {
			if (!grounded && !climbingLadder) {
				anim.SetBool ("Jump", true);
			} else {
				anim.SetBool ("Jump", false);
			}
		

			if (grounded) {
				if (xInput.ThumbStickL_X < -0.3f && !keyboardActive  || xInput.ThumbStickL_X > 0.3f && !keyboardActive || 
				    xInput.OnButton_DpadLeft && !keyboardActive|| xInput.OnButton_DpadRight && !keyboardActive||
				    Input.GetKey(KeyCode.D) && keyboardActive || Input.GetKey(KeyCode.A) && keyboardActive) {
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
		if (!menuActive && !playerOne && Intro.introTimer < 0f) {
			if (!grounded && !climbingLadder) {
				anim.SetBool ("Idle", true); //to be replaced with a jump
			} else {
				//anim.SetBool ("Jump", false);
			}
			
			if (grounded) {
				if (xInput.ThumbStickL_X < -0.3f && !keyboardActive|| xInput.ThumbStickL_X > 0.3f && !keyboardActive|| 
				    xInput.OnButton_DpadLeft && !keyboardActive|| xInput.OnButton_DpadRight&& !keyboardActive
				    || Input.GetKey(KeyCode.L) && keyboardActive|| Input.GetKey(KeyCode.Quote) && keyboardActive) {
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

	IEnumerator MenuFireAnimation()
	{
		swingInMenu = true;
		anim.SetBool ("Run", false);
		anim.SetBool ("Walkandpour", true); 
		yield return new WaitForSeconds(1.0f);
		anim.SetBool ("Walkandpour", false); 
		swingInMenu = false;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "WaterTrigger")
		{
			Instantiate (waterSplash,this.transform.position, Quaternion.identity);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.name == "WaterTrigger")
		{
			Instantiate (waterExitSplash,this.transform.position, Quaternion.identity);
		}
	}

	void OnDestroy(){
		//stop all vibrations as soon as the game is stopped
		if (!keyboardActive && !menuActive)
		{
			Controller.xInput.stopPadVibration(pone);
			Controller.xInput.stopPadVibration(ptwo);
		}
	}

	#endregion
}
