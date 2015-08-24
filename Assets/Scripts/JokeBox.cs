using UnityEngine;
using System.Collections;

public class JokeBox : MonoBehaviour {

	public AudioClip[] madJokesYo;
	public float jokeTimer = 13f;
	public float counter = 0f;
	AudioSource jokeBox;
	AudioClip dialogueQueue;
	int audioSelect;

	void Start () {
		jokeBox = GetComponent<AudioSource> ();
		audioSelect = madJokesYo.Length - 1;
	}

	void Update () {
		if (Intro.introTimer < -5f) {
			counter += Time.deltaTime;
		}

		if (counter > jokeTimer && !jokeBox.isPlaying) {
			counter = 0f;

			jokeBox.PlayOneShot(madJokesYo[audioSelect]);
			audioSelect --;
			counter = 0f;
		}

		if (audioSelect < 0) {
			audioSelect = madJokesYo.Length - 1;
		}

		if (dialogueQueue != null && !jokeBox.isPlaying) {
			jokeBox.PlayOneShot(dialogueQueue);
			dialogueQueue = null;
			counter = 0f;
		}
	}

	void QueueDialogue(AudioClip queue){
		dialogueQueue = queue;
	}

	void ResetTimer(){
		counter = 0f;
	}
}
