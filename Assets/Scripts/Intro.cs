using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public bool watchIntro;
	public Player sorek;
	public Player hickory;
	Camera cam;
	public float introTimer = 30f;
	float startIntroTimer;
	public float lerpTimer;
	bool caveInStarted = false;
	bool gameStarted = false;
	public GameObject caveInObj;
	public Transform caveInSpawnPos;

	Vector3 sorekCamPos;
	Vector3 hickoryCamPos;

	bool lerpToSorek = false; //This is just a hack to set the lerp timer to 0 only once.

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		startIntroTimer = introTimer;

		if (watchIntro){
			sorek.canMove = false;
			hickory.canMove = false;
			cam.SendMessage("Intro", true);
			lerpTimer = -1f;
		} else {
			gameStarted = true;
		}

		sorekCamPos = new Vector3 (sorek.transform.position.x, sorek.transform.position.y, this.transform.position.z);
		hickoryCamPos = new Vector3 (hickory.transform.position.x, hickory.transform.position.y, this.transform.position.z);
	}

	void Update () {

		/*SKIP*/
		 if (Input.GetKeyDown(KeyCode.Escape)){
			introTimer = startIntroTimer * 0.25f;
			cam.SendMessage("Intro", false);
		 }

		if (watchIntro){
			if (introTimer > startIntroTimer * 0.6f){
				transform.position = Vector3.Lerp(this.transform.position, hickoryCamPos, lerpTimer);
			}
			//Cam move to sorek holding lamp.
			if ((introTimer > startIntroTimer * 0.4f) && (introTimer < startIntroTimer * 0.6f)){
				if (lerpToSorek == false){
					lerpToSorek = true;
					lerpTimer = 0f;
				}
				transform.position = Vector3.Lerp(this.transform.position, sorekCamPos, lerpTimer);
			}
			//Cam gameplay on (set to normal cam)
			if ((introTimer > startIntroTimer * 0.3f) && (introTimer < startIntroTimer * 0.4f)){
				cam.SendMessage("Intro", false);
			}
			//Screen shake
			if ((introTimer > startIntroTimer * 0f) && (introTimer < startIntroTimer * 0.3f)){
				
			}
			//Cave in instantiate
			if (introTimer < startIntroTimer * 0.2f && !caveInStarted){
				CaveIn();
			}
			//Sorek and Hickory canMove;
			if (introTimer < 0f && !gameStarted){
				sorek.canMove = true;
				hickory.canMove = true;
				gameStarted = true;
			}


			introTimer -= Time.deltaTime;
			lerpTimer += Time.deltaTime / 5;
		}
	}

	void CaveIn(){
		caveInStarted = true;
		cam.SendMessage("Intro", false);
		Instantiate (caveInObj, caveInSpawnPos.position, Quaternion.identity);
	}
}
