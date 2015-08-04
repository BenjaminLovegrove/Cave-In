using UnityEngine;
using System.Collections;

public class FallingGround : MonoBehaviour {

	public float crumbleTimer = 1f;
	public AudioClip crumble;
	Rigidbody[] rocksRB;
	Collider[] rockCollider;
	bool first = true;

	void Start () {
		rocksRB = GetComponentsInChildren<Rigidbody> ();
		rockCollider = GetComponents<Collider> ();
	}
	
	void OnTriggerStay(Collider col){
	
		if (col.gameObject.tag == "Player"){

			//Standing time until they crumble
			crumbleTimer -= Time.deltaTime;

			//Crumble
			if (crumbleTimer <= 0f){
				if (first == true){
					if (crumble != null){
						AudioSource.PlayClipAtPoint(crumble, transform.position);
					}
					first = false;
				}

				rockCollider[0].enabled = false;
				rockCollider[1].enabled = false;
				Destroy (this.gameObject, 3f);
				foreach (Rigidbody rocks in rocksRB){
					rocks.isKinematic = false;
					rocks.useGravity = true;
					Destroy (rocks.gameObject, 10f);
				}
			}
		}
	}
	
}
