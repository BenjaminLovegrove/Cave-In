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
	Rigidbody rb; //To stand still while swinging


	void Start(){
		rb = GetComponent<Rigidbody> ();
		hickoryScr = GetComponent<Player> ();
	}

	void Update () {
		if (hickoryScr.prevState.Buttons.B == ButtonState.Released && hickoryScr.state.Buttons.B == ButtonState.Pressed && swinging == false){
			Swing();
		}
		if (Input.GetKeyDown(KeyCode.P) && swinging == false){
			Swing();
		}

		if (swinging) {
			swingTime -= Time.deltaTime;

			if (swingTime <= 0f){
				swinging = false;
				this.SendMessage ("CanMove", true);
			}
		}

	}

	void Swing(){
		swingTime = swingLength;
		swinging = true;
		//play animation

		//Hit object infront. This should probably done a few seconds later timed with the animation when we have it.
		RaycastHit hit;
		if (transform.localScale.x <= 0) {
			if (Physics.Raycast (transform.position, -transform.right, out hit, 3f)) {
				this.SendMessage("CanMove", false);
				Instantiate (hickoryPickAxe, new Vector3 (gameObject.transform.position.x,gameObject.transform.position.y +1,-3), Quaternion.identity);
				AudioSource.PlayClipAtPoint(PickHit, transform.position);
				hit.collider.gameObject.SendMessage("PickHit", SendMessageOptions.DontRequireReceiver);
				//Particle effect
			}
		} else {
			if (Physics.Raycast (transform.position, transform.right, out hit, 3f)) {
				this.SendMessage("CanMove", false);
				Instantiate (hickoryPickAxe, new Vector3 (gameObject.transform.position.x,gameObject.transform.position.y +1,-3), Quaternion.identity);
				AudioSource.PlayClipAtPoint(PickHit, transform.position);
				hit.collider.gameObject.SendMessage("PickHit", SendMessageOptions.DontRequireReceiver);
				//Particle effect
			}
		}

		if (hit.collider == null) {
			//Play swing and miss sound.
		}

	}
}
