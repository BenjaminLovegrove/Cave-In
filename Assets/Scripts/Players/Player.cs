using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class Player : MonoBehaviour
{
	// Script to handle player controls and most interactions

	public GameObject player;
	public bool playerOne;

	Rigidbody playerRigid;
	float movementForce;
	float jumpForce;

	public bool canMove = true;
	public float baseMovForce = 10f;
	public float baseJumpForce = 500f;
	public float slowTimer;
	private bool grounded = false;
	public bool rightFaced = true;
	public bool climbingLadder;
	private float h;
	private float v;

	//xInput
	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;

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
		player = this.gameObject;
		playerRigid = GetComponent<Rigidbody>();
		movementForce = baseMovForce;
		jumpForce = baseJumpForce;
		Flip ();
	}

	void Update(){

		if (movementForce < baseMovForce) {
			slowTimer -= Time.deltaTime;

			if (slowTimer <= 0){
				movementForce = baseMovForce;
				jumpForce = baseJumpForce;
			}
		}

		///
		//xInput detecting gamepad
		///

		prevState = state;
		state = GamePad.GetState(playerIndex);

		// Find a PlayerIndex, for a single player game
		if ( !playerIndexSet || !prevState.IsConnected ) {
			;
			for ( int i = 0; i < 4; ++i ) {

				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				switch ( i ) {
				case 0:
					testPlayerIndex = PlayerIndex.One;
					break;
				case 1:
					testPlayerIndex = PlayerIndex.Two;
					break;
				case 2:
					testPlayerIndex = PlayerIndex.Three;
					break;
				case 3:
					testPlayerIndex = PlayerIndex.Four;
					break;
				}
				
				GamePadState testState = GamePad.GetState ( testPlayerIndex );
				if ( testState.IsConnected ) {
					Debug.Log ( "GamePad found {0}" + testPlayerIndex) ;
					playerIndex = playerIndex;
					playerIndexSet = true;
				}
			}
		}
		state = GamePad.GetState ( playerIndex );

		h1 = state.ThumbSticks.Left.X;
		v1 = state.ThumbSticks.Left.Y;
		
		h2 = state.ThumbSticks.Right.X;
		v2 = state.ThumbSticks.Right.Y;
		
		buttonA = ( state.Buttons.A == ButtonState.Pressed );
		buttonB = ( state.Buttons.B == ButtonState.Pressed );
		buttonX = ( state.Buttons.X == ButtonState.Pressed );
		buttonY = ( state.Buttons.Y == ButtonState.Pressed );
		
		dpadUp = ( state.DPad.Up == ButtonState.Pressed );
		dpadDown = ( state.DPad.Down == ButtonState.Pressed );
		dpadLeft = ( state.DPad.Left == ButtonState.Pressed );
		dpadRight = ( state.DPad.Right == ButtonState.Pressed );
		
		buttonStart = ( state.Buttons.Start == ButtonState.Pressed );
		buttonBack = ( state.Buttons.Back == ButtonState.Pressed );
		
		shoulderL = ( state.Buttons.LeftShoulder == ButtonState.Pressed );
		shoulderR = ( state.Buttons.RightShoulder == ButtonState.Pressed );
		
		stickL = ( state.Buttons.LeftStick == ButtonState.Pressed );
		stickR = ( state.Buttons.RightStick == ButtonState.Pressed );
		
		triggerL = state.Triggers.Left;
		triggerR = state.Triggers.Right;



		// Detect if a button was pressed this frame
		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
		{

		}
		// Detect if a button was released this frame
		if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
		{

		}
		// Set vibration according to triggers
		GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);



		if (menuActive)
		{
			if (state.Buttons.Start  == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
			{
				//Player 1 toggle
				if (GameObject.Find("P1").GetComponent<Toggle>().isOn == false)
				{
					GameObject.Find("P1").GetComponent<Toggle>().isOn = true;
				}
				else
				{
					GameObject.Find("P1").GetComponent<Toggle>().isOn = false;
				}

				//Player 2 toggle
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


	public static void  padVibration (  PlayerIndex playerIndex ,   float big ,   float small   ){
		GamePad.SetVibration ( playerIndex, big, small );
	}
	
	public static void  stopPadVibration (  PlayerIndex playerIndex   ){
		GamePad.SetVibration( playerIndex, 0, 0 );
	}

	void FixedUpdate ()
	{
//		// Get and set control inputs
//		if (playerOne) {
//			h = Input.GetAxis ("Horizontal");
//			v = Input.GetAxis ("Vertical");
//		} else {
//			h = Input.GetAxis ("Horizontal2");
//			v = Input.GetAxis ("Vertical2");
//		}

		// Function calls
		Grounded ();
		if (canMove && !menuActive) {
			Controls ();
		}

		Mathf.Clamp(playerRigid.velocity.magnitude,0,10);
		//print("Magnitude: "+ rigidbody.velocity.magnitude);
	
	}

	// Flip sprite depending on direction
	private void Flip()
	{
		if (rightFaced)
		{
			Vector3 spriteScale = transform.localScale;
			spriteScale.x = 1;
			transform.localScale = spriteScale;
		}

		if (!rightFaced)
		{
			Vector3 theScale = transform.localScale;
			theScale.x = -1;
			transform.localScale = theScale;
		}

		
	}

	// Grounded check
	private void Grounded()
	{
		Vector3 leftExtent = new Vector3 (this.transform.position.x - collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);
		Vector3 rightExtent = new Vector3 (this.transform.position.x + collider.bounds.size.x / 2, this.transform.position.y, this.transform.position.z);

		if (Physics.Raycast (this.transform.position, Vector3.down, 1.5f) || Physics.Raycast (leftExtent, Vector3.down,  1.5f)|| Physics.Raycast (rightExtent, Vector3.down,  1.5f))
		{
			grounded = true;
			//print ("Grounded_M: "+ grounded);
		} 
		else 
		{
			grounded = false;
			//print ("Grounded: "+ grounded);
		}

		//Debug.DrawRay (leftExtent, Vector3.down);
		//Debug.DrawRay (rightExtent, Vector3.down);
		//print ("Grounded: "+ grounded);

	}

	// Controls
	private void Controls()
	{
		//Keyboard
		//Right
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (1) * Time.deltaTime);
			rightFaced = true;
			print ("move right");
			Flip ();
		}
			
		// Left
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (-1) * Time.deltaTime);
			rightFaced = false;
			print ("move left");
			Flip ();
		}


		//Xbox support
		// Right
		//print (state.ThumbSticks.Left.X);
		if (state.ThumbSticks.Left.X > 0.3f || state.DPad.Right == ButtonState.Pressed)
		{
			player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (state.ThumbSticks.Left.X) * Time.deltaTime);
			rightFaced = true;
			Flip ();
		}
		
		// Left
		if (state.ThumbSticks.Left.X < -0.3f)
		{
			player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (state.ThumbSticks.Left.X) * Time.deltaTime);
			rightFaced = false;
			Flip ();
		}
		
		// Jump
		if ((grounded == true) && Input.GetKeyDown(KeyCode.Space) && (!climbingLadder))
		{
			print("space pressed");
			playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
		//Jump xbox controls
		if (playerOne) {
			if ((grounded == true)&& prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && (!climbingLadder)) {
				print ("P1_Jumped_xbox_p1");
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		} else {
			if ((grounded == true)&& prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && (!climbingLadder)) {
				print ("P2_Jumped_xbox_p2");
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		}

		// Climb
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

			//xbox
			//Ascend
			if (state.ThumbSticks.Left.Y > 0.1f)
			{
				player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (state.ThumbSticks.Left.Y) * Time.deltaTime);
			}
			// Descend
			else if (state.ThumbSticks.Left.Y < 0.1f)
			{
				player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (state.ThumbSticks.Left.Y) * Time.deltaTime);
			}
		}
		
		else playerRigid.useGravity = true;

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
		movementForce = baseMovForce / 2;
		jumpForce = baseJumpForce / 2;

		slowTimer = 1f;
	}
}
