using UnityEngine;
using System.Collections;

public class RumbleSFX : MonoBehaviour {

	AudioSource rumble;
	float maxVol;
	float targVol;
	bool introComplete;

	void Start(){
		rumble = GetComponent<AudioSource> ();
		maxVol = rumble.volume;
		targVol = maxVol / 2;
	}

	void Update () {
		if (!rumble.isPlaying) {
			if (Intro.introTimer < Intro.startIntroTimer * 0.35f) {
				rumble.Play ();
			}
		}

		if (Intro.introTimer <= 0 && !introComplete) {
			rumble.volume = Mathf.Lerp (rumble.volume, targVol, Time.deltaTime / 3);
		}

		if (Intro.introTimer <= -8f) {
			introComplete = true;
		}

	}
}
