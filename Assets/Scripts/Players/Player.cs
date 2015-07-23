using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// Script to handle player controls and most interactions

	public GameObject player;

	public bool playerOne;

	public Rigidbody playerRigid;

	public float movementForce = 10;

	public float jumpForce = 120;

	public int playerHP;

	private bool grounded = false;

	public static bool rightFaced;

	public bool climbingLadder;

	private float h;

	private float v;

	void Start(){
		player = this.gameObject;
		playerRigid = GetComponent<Rigidbody>();
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
		Controls ();
	
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

		if (Physics.Raycast (this.transform.position, Vector3.down, 2))
		{
			grounded = true;
		} 
		else if (Physics.Raycast (leftExtent, Vector3.down, 2))
		{
			grounded = true;
		} 
		else if (Physics.Raycast (rightExtent, Vector3.down, 2))
		{
			grounded = true;
		} 

		else 
		{
			grounded = false;
		}

		//Debug.DrawRay (leftExtent, Vector3.down);
		//Debug.DrawRay (rightExtent, Vector3.down);

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
			playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
		if (playerOne) {
			if ((grounded == true) && Input.GetKeyDown (KeyCode.Joystick1Button0) && (!climbingLadder)) {
				playerRigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		} else {
			if ((grounded == true) && Input.GetKeyDown (KeyCode.Joystick2Button0) && (!climbingLadder)) {
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
}
