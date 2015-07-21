using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// Script to handle player controls and most interactions

	public GameObject player;

	public Rigidbody playerRigid;

	public float movementForce = 10;

	public float jumpForce = 50;

	public int playerHP;

	private bool grounded = false;

	private bool rightFaced;

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
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		// Function calls
		Grounded ();
		Controls ();
		//Pause ();

		// Sync player HP
		//playerHP = EventManager.instance.playerHP;
	
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
		if (Physics.Raycast (this.transform.position, Vector3.down, 2))
		{
			grounded = true;
		}
		
		else grounded = false;
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
		if ((grounded == true) && (v > 0) && (!climbingLadder))
		{
			playerRigid.AddForce(Vector3.up * jumpForce);
		}

		// Climb
		if (climbingLadder)
		{
			playerRigid.useGravity = false;

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

	/*
	// Pause and Unpause Game
	private void Pause()
	{
		// Pause
		if ((Input.GetKeyDown (KeyCode.P) && EventManager.instance.paused == false))
		{
			EventManager.instance.Pause ();
			//EventManager.instance.paused = true;
		}
		
		// Unpause
		if ((Input.GetKeyDown (KeyCode.P) && EventManager.instance.pauseDelay == true))
		{
			Debug.Log ("Unpaused");
			EventManager.instance.Unpause ();
			//EventManager.instance.paused = false;
		}
	}*/

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
