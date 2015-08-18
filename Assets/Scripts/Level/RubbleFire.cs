using UnityEngine;
using System.Collections;

public class RubbleFire : MonoBehaviour {

	float lightTime = 1f;
	bool lit;
	public bool resetDifficulty = false;

	public Light fireLight;
	public SpriteRenderer fireSprite;
	AudioSource fireAudio;
	public Transform p1check;
	public Transform p2check;

	float uiLerp = 0f;
	public SpriteRenderer checkpointUI;
	public SpriteRenderer flameSpr;

	void Start () {
		fireAudio = GetComponent<AudioSource> ();
	}

	void OnTriggerStay (Collider col) {
		if (col.collider.gameObject.tag == "Fire") {
			lightTime -= Time.deltaTime;
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.collider.gameObject.tag == "Player") {
			col.gameObject.SendMessage("EnterBurnable");
		}
	}
	void OnTriggerExit (Collider col) {
		if (col.collider.gameObject.tag == "Player") {
			col.gameObject.SendMessage("ExitBurnable");
		}
	}

	void Update(){

		if (lightTime < 0 && !lit) {
			flameSpr.enabled = true;
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
				uiLerp = 1f;
				CheckpointManager.checkpointSpawn = true;
				CheckpointManager.p1checkpoint = p1check.position;
				CheckpointManager.p2checkpoint = p2check.position;
			}
		}

		if (uiLerp > 0f){
			uiLerp -= Time.deltaTime / 3;

			checkpointUI.color = new Color (checkpointUI.color.r, checkpointUI.color.g, checkpointUI.color.b, uiLerp);
		}

	}


}
