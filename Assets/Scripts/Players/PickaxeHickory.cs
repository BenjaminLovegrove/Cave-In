using UnityEngine;
using System.Collections;

public class PickaxeHickory : MonoBehaviour {

	public float swingTime;
	public AudioClip PickHit;

	bool swinging = false;
	Rigidbody rb; //To stand still while swinging


	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Joystick2Button1) && swinging == false){
			Swing();
		}
		if (Input.GetKeyDown(KeyCode.P) && swinging == false){
			Swing();
		}

		if (swinging) {
			swingTime -= Time.deltaTime;

			if (swingTime <= 0f){
				swinging = false;
			}
		}

	}

	void Swing(){
		swingTime = 1f;
		swinging = true;
		//play animation

		//Hit object infront. This should probably done a few seconds later timed with the animation when we have it.
		RaycastHit hit;
		if (transform.localScale.x <= 0) {
			if (Physics.Raycast (transform.position, -transform.right, out hit, 3f)) {
				AudioSource.PlayClipAtPoint(PickHit, transform.position);
				hit.collider.gameObject.SendMessage("PickHit", SendMessageOptions.DontRequireReceiver);
				//Particle effect
			}
		} else {
			if (Physics.Raycast (transform.position, transform.right, out hit, 3f)) {
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
