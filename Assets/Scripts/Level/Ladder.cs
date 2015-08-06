using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	public bool ladderFall = false;
	public GameObject ladderParent;
	Collider ladderCol;
	Rigidbody ladderRb;
	bool triggered = false;

	void Start () {
		ladderCol = ladderParent.GetComponent<Collider> ();
		ladderRb = ladderParent.GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player" && !triggered) {
			ladderCol.enabled = false;
			ladderRb.isKinematic = false;
			ladderRb.useGravity = true;
		}
	}
}
