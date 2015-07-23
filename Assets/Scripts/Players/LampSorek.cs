using UnityEngine;
using System.Collections;

public class LampSorek : MonoBehaviour {
	

	public GameObject fireSpread;

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F)){
			Instantiate (fireSpread, transform.position, Quaternion.identity);
		}

	}
}
