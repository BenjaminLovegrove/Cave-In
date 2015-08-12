using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {

	public AudioClip dialogue;
	bool triggered = false;

	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
	
		if (col.gameObject.tag == "Player" && triggered == false) {
			AudioSource.PlayClipAtPoint (dialogue, transform.position);
			triggered = true;
		}

	}
}
