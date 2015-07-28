using UnityEngine;
using System.Collections;

public class fireSpawner : MonoBehaviour {

	public int touchingFire = 0;
	float fireCooldown = 0.5f;
	float lifetime = 5f;
	public GameObject fireObj;
	float lifetimeSet;
	
	// Update is called once per frame
	void Update () {
		fireCooldown -= Time.deltaTime;
		lifetime -= Time.deltaTime;

		lifetimeSet = (lifetime / 5f) * 10f;
		if ((touchingFire <= 0) && (fireCooldown <= 0)){
			GameObject fire = Instantiate (fireObj, transform.position, Quaternion.identity) as GameObject;
			fire.SendMessage ("SetLifetime", lifetimeSet);
			fireCooldown = 1f;
		}

		if (lifetime <= 0){
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Fire"){
			col.SendMessage ("SetLifetime", lifetimeSet);
			touchingFire ++;
			print ("fire hit");
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Fire"){
			touchingFire --;
		}
	}
}
