using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class PickaxeHickory : MonoBehaviour {

	Player hickoryScr;
	public float swingLength = 1f;
	float swingTime;
	public AudioClip PickHit;
	public GameObject hickoryPickAxe;

	bool swinging = false;


	void Start(){
		hickoryScr = GetComponent<Player> ();
	}

	void Update () {
		if (!hickoryScr.isDead) {
			if (!Player.keyboardActive) {
				if (hickoryScr.state.Buttons.B == ButtonState.Pressed && swinging == false&& swinging == false) {
					Swing ();
				}
			} else {
				if (Input.GetKeyDown (KeyCode.K) && swinging == false) {
					Swing ();
				}
			}

			if (swinging) {
				swingTime -= Time.deltaTime;
				if (swingTime <= 0f) {
					swinging = false;
					this.SendMessage ("CanMove", true);
				}
			}
		}

	}

	void Swing(){

		if (!swinging) {
			swingTime = swingLength;
			swinging = true;

			//Hit object infront. This should probably done a few seconds later timed with the animation when we have it.
			RaycastHit hit;
			if (transform.localScale.x <= 0) {
				if (Physics.Raycast (new Vector3(transform.position.x, transform.position.y - 7f, 0), -transform.right, out hit, 3f)) {
					hickoryScr.anim.SetBool ("Run", false);
					hickoryScr.anim.SetBool ("Idle", false);
					hickoryScr.anim.SetTrigger ("Swing");
					this.SendMessage ("CanMove", false);
					AudioSource.PlayClipAtPoint (PickHit, transform.position);
					hit.collider.gameObject.SendMessage ("PickHit", SendMessageOptions.DontRequireReceiver);
					hit.collider.gameObject.SendMessageUpwards ("PickHit", SendMessageOptions.DontRequireReceiver);
					//Particle effect
				} else if (Physics.Raycast (new Vector3(transform.position.x, transform.position.y - 1.5f, 0), -transform.right, out hit, 3f)) { //this is a lower raycast to check for crates
					hickoryScr.anim.SetBool ("Run", false);
					hickoryScr.anim.SetBool ("Idle", false);
					hickoryScr.anim.SetTrigger ("Swing");
					this.SendMessage ("CanMove", false);
					AudioSource.PlayClipAtPoint (PickHit, transform.position);
					hit.collider.gameObject.SendMessage ("PickHit", SendMessageOptions.DontRequireReceiver);
					hit.collider.gameObject.SendMessageUpwards ("PickHit", SendMessageOptions.DontRequireReceiver);
					//Particle effect
				}
			} else {
				if (Physics.Raycast (transform.position, transform.right, out hit, 3f)) {
					hickoryScr.anim.SetBool ("Run", false);
					hickoryScr.anim.SetBool ("Idle", false);
					hickoryScr.anim.SetTrigger ("Swing");
					this.SendMessage ("CanMove", false);
					AudioSource.PlayClipAtPoint (PickHit, transform.position);
					hit.collider.gameObject.SendMessage ("PickHit", SendMessageOptions.DontRequireReceiver);
					hit.collider.gameObject.SendMessageUpwards ("PickHit", SendMessageOptions.DontRequireReceiver);
					//Particle effect
				} else if (Physics.Raycast (new Vector3(transform.position.x, transform.position.y - 1.5f, 0), transform.right, out hit, 3f)) { //this is a lower raycast to check for crates
					hickoryScr.anim.SetBool ("Run", false);
					hickoryScr.anim.SetBool ("Idle", false);
					hickoryScr.anim.SetTrigger ("Swing");
					this.SendMessage ("CanMove", false);
					AudioSource.PlayClipAtPoint (PickHit, transform.position);
					hit.collider.gameObject.SendMessage ("PickHit", SendMessageOptions.DontRequireReceiver);
					hit.collider.gameObject.SendMessageUpwards ("PickHit", SendMessageOptions.DontRequireReceiver);
					//Particle effect
				}
			}


			if (hit.collider == null) {
				//Play swing and miss sound.
			}
		}

	}
}
