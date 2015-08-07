using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	public Transform player1;
	public Transform player2;

	bool gamePlay = true;
	Vector3 p1Pos;
	Vector3 p2Pos;
	Vector3 playerCenter;
	Vector3 camCenter;
	bool deathCam = false;
	Vector3 deathCamPos;

	float p1Dist;
	float p2Dist;

	float distThreshold;
	float maxSize = 100.0f; //how far apart is the maximum for the othorgraphic size of the camera
	
	void FixedUpdate () {

		if (gamePlay){
			//Keep cam between players
			playerCenter = Vector3.Lerp (player1.position, player2.position, 0.5f);
			this.transform.position = new Vector3(playerCenter.x, playerCenter.y, transform.position.z);

			//Get player positions in world space and camera center
			p1Pos = Camera.main.WorldToScreenPoint (player1.position);
			p2Pos = Camera.main.WorldToScreenPoint (player2.position);
			camCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
			distThreshold = Screen.width * 0.2f;

			//Player distance from screen center
			p1Dist = Vector3.Distance (p1Pos, camCenter);
			p2Dist = Vector3.Distance (p2Pos, camCenter);

			//Resize camera to match
			if (p1Dist > distThreshold && Camera.main.orthographicSize < maxSize || p2Dist > distThreshold && Camera.main.orthographicSize < maxSize){
				Camera.main.orthographicSize += Time.deltaTime * 4;
			}

			if (Camera.main.orthographicSize > 10f) {
				if (p1Dist < distThreshold * 0.9f || p2Dist < distThreshold * 0.9f) {
					Camera.main.orthographicSize -= Time.deltaTime * 4;
				}
			}
		}

		if (deathCam) {
			transform.position = Vector3.Lerp(transform.position, deathCamPos, Time.deltaTime/3);
			Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, 8f, Time.deltaTime / 5);
		}

	}

	void Intro (bool playIntro){
		if (playIntro){
			gamePlay = false;
		} else {
			gamePlay = true;
			deathCam = false;
		}
	}

	void Death (Vector3 deathPos){

		gamePlay = false;
		deathCam = true;
		deathCamPos = deathPos;

	}
}
