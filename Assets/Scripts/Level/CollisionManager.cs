using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour
{
	// Script to handle all collision interactions
	// ATTACH THIS SCRIPT TO ANY OBJECT THAT WILL HAVE A COLLISION INTERACTION
	// THEN SET THE APPROPRIATE BOOLEAN(S) TO 'TRUE" IN THE INSPECTOR

	public GameObject objectDestory;

	public int checkpointNumber;

	public bool checkpoint;

	public bool healthPickup;

	public bool damageCollider;

	public bool ladder;

	private bool functional; //to turn ladders off after they fall

	void OnTriggerEnter(Collider temp)
	{
		/*
		// Do action determined by bool setting
		if (temp.gameObject.tag == "Player")
		{
			Debug.Log ("A collision has occured!");
			// Sets current checkpoint
			if (checkpoint)
			{
				EventManager.instance.checkPoint = checkpointNumber;
				EventManager.instance.playerHPCheckpoint = EventManager.instance.playerHP;
				Debug.Log ("Check Point " + checkpointNumber + " activated");
			}

			// Health pickup
			else if (healthPickup)

			{
				EventManager.instance.playerHP += 25;
				Debug.Log ("Health Pickup");
			}

			// Damage collider
			else if (damageCollider && EventManager.instance.damageTaken == false)
			
			{
				EventManager.instance.playerHP -= 15;
				EventManager.instance.damageTaken = true;
				Debug.Log ("Ouch!");
			}

			// Ladder enter send message function
			else */

		if (ladder && functional)

			{
				temp.gameObject.SendMessage("Ladder", 1f, SendMessageOptions.DontRequireReceiver);
				//Debug.Log ("Climbing Ladder");
			}
		}

		/*
		// NPC kill collider
		if (temp.gameObject.tag == "PlayerFeet")
		{
			if (damageCollider)
			{
				Debug.Log (objectDestory + " Killed");
				GameObject.Destroy(objectDestory);
			}
		}
	}*/

	// Ladder exit send message function
	void OnTriggerExit(Collider temp)
	{
		if (ladder)
			
		{
			temp.gameObject.SendMessage("Ladder", 0f, SendMessageOptions.DontRequireReceiver);
			//Debug.Log ("Jumped off Ladder");
		}
	}

	void NotFunctional(){
		functional = false;
	}
	
}
