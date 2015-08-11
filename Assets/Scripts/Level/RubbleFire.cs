using UnityEngine;
using System.Collections;

public class RubbleFire : MonoBehaviour {

	public float lightTime = 1f;
	bool lit;
	public bool resetDifficulty = false;

	public Light fireLight;
	public SpriteRenderer fireSprite;
	AudioSource fireAudio;
	public Transform p1check;
	public Transform p2check;

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
			print ("cart has been lit");
			fireLight.enabled = true;
			fireSprite.enabled = true;
			fireAudio.enabled = true;
			lit = true;
			if (resetDifficulty){
				Intro.ci1difficulty = 1f;
				Intro.ci2difficulty = 1f;
			}
			if (p1check != null && p2check !=null)
			{
				CheckpointManager.checkpointSpawn = true;
				CheckpointManager.p1checkpoint = p1check.position;
				CheckpointManager.p2checkpoint = p2check.position;
			}
		}

	}
}
