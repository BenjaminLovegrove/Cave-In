using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	public bool ladderFall = false;
	public GameObject ladderParent;
	Collider ladderCol;
	Rigidbody ladderRb;
	bool triggered = false;

	public GameObject p1,p2;

	void Start () {
		ladderCol = ladderParent.GetComponent<Collider> ();
		ladderRb = ladderParent.GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player" && !triggered) {
			Destroy (ladderCol);
			ladderRb.isKinematic = false;
			ladderRb.useGravity = true;
			p1.SendMessage("Ladder", 0f);
			p2.SendMessage("Ladder", 0f);
			Destroy (this.gameObject);
		}
	}
}
