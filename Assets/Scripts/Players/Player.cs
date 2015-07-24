using UnityEngine;
using System.Collections;

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
	public bool rightFaced;
	public bool climbingLadder;
	private float h;
	private float v;

	void Start(){
		player = this.gameObject;
		playerRigid = GetComponent<Rigidbody>();
		movementForce = baseMovForce;
		jumpForce = baseJumpForce;
	}

	void Update(){


		if (movementForce < baseMovForce) {
			slowTimer -= Time.deltaTime;

			if (slowTimer <= 0){
				movementForce = baseMovForce;
				jumpForce = baseJumpForce;
			}
		}

	}

	void FixedUpdate ()
	{
		// Get and set control inputs
		if (playerOne) {
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");
		} else {
			h = Input.GetAxis ("Horizontal2");
			v = Input.GetAxis ("Vertical2");
		}

		// Function calls
		Grounded ();
		if (canMove) {
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
			print ("Grounded_M: "+ grounded);
		} 
		else 
		{
			grounded = false;
			print ("Grounded: "+ grounded);
		}

		//Debug.DrawRay (leftExtent, Vector3.down);
		//Debug.DrawRay (rightExtent, Vector3.down);
		//print ("Grounded: "+ grounded);

	}

	// Controls
	private void Controls()
	{
		// Right
		if (h > 0)
		{
			player.transform.Translate(Vector3.right * movementForce * Mathf.Abs (h) * Time.deltaTime);
			rightFaced = true;
			Flip ();
		}
		
		// Left
		if (h < 0)
		{
			player.transform.Translate(Vector3.left * movementForce * Mathf.Abs (h) * Time.deltaTime);
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
			if ((grounded == true) && Input.GetKeyDown (KeyCode.Joystick1Button0) && (!climbingLadder)) {
				print ("P1_Jumped");
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		} else {
			if ((grounded == true) && Input.GetKeyDown (KeyCode.Joystick2Button0) && (!climbingLadder)) {
				print ("P2_Jumped");
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		}

		// Climb
		if (climbingLadder)
		{
			playerRigid.useGravity = false;
			playerRigid.velocity = Vector3.zero;

			// Ascend
			if (v > 0)
			{
				player.transform.Translate(Vector3.up * (movementForce / 2.5f) * Mathf.Abs (v) * Time.deltaTime);
			}

			// Descend
			else player.transform.Translate(Vector3.down * (movementForce / 2.5f) * Mathf.Abs (v) * Time.deltaTime);
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
