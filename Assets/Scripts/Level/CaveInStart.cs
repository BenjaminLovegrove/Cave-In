using UnityEngine;
using System.Collections;

public class CaveInStart : MonoBehaviour {

	public float StartTimer = 5f;
	public GameObject caveInPrefab;
	bool spawned = false;

	void Update () {
		StartTimer -= Time.deltaTime;
		if (StartTimer <= 0f && spawned == false){
			Instantiate (caveInPrefab, transform.position, Quaternion.identity);
			spawned = true;
		}
	}
}
