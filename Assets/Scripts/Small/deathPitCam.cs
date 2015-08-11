using UnityEngine;
using System.Collections;

public class deathPitCam : MonoBehaviour {

	public bool pitCam;

	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
	
		if (col.gameObject.tag == "Player"){
			Camera.main.SendMessage ("DeathPit", pitCam);
		}

	}
}
