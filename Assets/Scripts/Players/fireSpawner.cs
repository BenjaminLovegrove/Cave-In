using UnityEngine;
using System.Collections;

public class fireSpawner : MonoBehaviour {

	public int touchingFire = 0;
	float fireCooldown = 0f;
	float lifetime = 5f;
	public GameObject fireObj;
	float lifetimeSet;
	
	// Update is called once per frame
	void Update () {
		fireCooldown -= Time.deltaTime;
		lifetime -= Time.deltaTime;

		lifetimeSet = (lifetime / 5f) * 30f;
		RaycastHit rayHit;
		if (Physics.Raycast(transform.position, Vector3.down, out rayHit, 0.2f)){
			if (rayHit.collider.gameObject.tag == "Terrain"){
				if ((touchingFire <= 0) && (fireCooldown <= 0)){
					GameObject fire = Instantiate (fireObj, transform.position, Quaternion.identity) as GameObject;
					fire.SendMessage ("SetLifetime", lifetimeSet);
					fireCooldown = 1f;
				}
			}
		}

		if (lifetime <= 0){
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Fire"){
			col.SendMessage ("SetLifetime", lifetimeSet);
			touchingFire ++;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Fire"){
			touchingFire --;
		}
	}
}
