using UnityEngine;
using System.Collections;

public class startAfterIntro : MonoBehaviour {


	AudioSource rumble;

	void Start(){
		rumble = GetComponent<AudioSource> ();
	}

	void Update () {
		if (!rumble.isPlaying) {
			if (Intro.introTimer < Intro.startIntroTimer * 0.35f) {
				rumble.Play ();
			}
		}

	}
}
