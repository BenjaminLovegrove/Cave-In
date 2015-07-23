using UnityEngine;
using System.Collections;

public class CaveInStart : MonoBehaviour {

	public float StartTimer = 3f;
	public GameObject caveInPrefab;
	bool spawned = false;
	bool startGame = false;

	void Update () {
		print ("Game Start: " +startGame);
		if (startGame) {
			StartTimer -= Time.deltaTime;
			if (StartTimer <= 0f && spawned == false) {
				Instantiate (caveInPrefab, transform.position, Quaternion.identity);
				spawned = true;
			}
		}
	}

	void StartGame(){
		startGame = true;
	}
}
