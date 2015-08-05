using UnityEngine;
using System.Collections;

public class RubbleFire : MonoBehaviour {

	public float lightTime = 1f;
	bool lit;

	public Light fireLight;
	public SpriteRenderer fireSprite;
	AudioSource fireAudio;

	void Start () {
		fireAudio = GetComponent<AudioSource> ();
	}

	void OnTriggerStay (Collider col) {
		if (col.collider.gameObject.tag == "Fire") {
			lightTime -= Time.deltaTime;
		}
	}

	void Update(){

		if (lightTime < 0 && !lit) {
			fireLight.enabled = true;
			fireSprite.enabled = true;
			fireAudio.enabled = true;
			lit = true;
		}

	}
}
