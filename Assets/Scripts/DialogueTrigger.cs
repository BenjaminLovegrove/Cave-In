using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	
	AudioSource jokeBox;
	public AudioClip dialogue;
	bool triggered = false;

	void Start(){
		jokeBox = GameObject.FindGameObjectWithTag ("JokeBox").GetComponent<AudioSource>();
	}

	void OnTriggerEnter (Collider col) {
	
		if (col.gameObject.tag == "Player" && triggered == false) {
			if (!jokeBox.isPlaying){
				AudioSource.PlayClipAtPoint (dialogue, transform.position);
				triggered = true;
				jokeBox.gameObject.SendMessage("ResetTimer");
			} else {
				jokeBox.gameObject.SendMessage("QueueDialogue", dialogue);
			}
		}

	}
}
