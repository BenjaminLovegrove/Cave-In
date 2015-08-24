using UnityEngine;
using System.Collections;

public class FallingGround : MonoBehaviour {

	public float crumbleTimer = 1f;
	public AudioClip crumble;
	public int playersRequired = 2;
	Rigidbody[] rocksRB;
	Collider[] rockCollider;
	bool first = true;
	int playerCount = 0;

	void Start () {
		rocksRB = GetComponentsInChildren<Rigidbody> ();
		rockCollider = GetComponents<Collider> ();
	}

	void Update(){
		//Standing time until they crumble
		if (playerCount >= playersRequired) {
			crumbleTimer -= Time.deltaTime;
		}
		
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

	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Player"){
			playerCount ++;
		}
	}

	void OnTriggerExit(Collider col){
		
		if (col.gameObject.tag == "Player"){
			playerCount --;
		}
	}

}
