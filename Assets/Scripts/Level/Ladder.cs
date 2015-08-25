using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	public bool ladderFall = false;
	public GameObject ladderParent;
	public Collider ladderCol;
	public AudioClip ladderFallSFX;
	Rigidbody ladderRb;
	bool triggered = false;


	//public GameObject p1,p2;

	void Start () {
	//	ladderCol = ladderParent.GetComponent<Collider> ();
		ladderRb = ladderParent.GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player" && !triggered) {
			AudioSource.PlayClipAtPoint(ladderFallSFX, transform.position);
			this.BroadcastMessage("NotFunctional");
			ladderRb.isKinematic = false;
			ladderRb.useGravity = true;
			ladderCol.enabled = false;
			col.gameObject.SendMessage("Ladder", 0f, SendMessageOptions.DontRequireReceiver);
			triggered = true;
		}
	}
}
