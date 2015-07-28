using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public AudioClip CaveInSFX;

	public float smallRumble, bigRumble;

	Vector3 sorekCamPos;
	Vector3 hickoryCamPos;

	//outro
	bool outro = false;
	Vector3 outroCamStart;
	public Transform outroCamAim;
	public Renderer FTW;
	float FTWalpha = 1f;

	List<GameObject> toggleObjs = new List<GameObject>();

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

	void FixedUpdate()
	{
		if (watchIntro)
		{

			//do xbox rumble here 
			if ((introTimer > startIntroTimer * 0.2f) && (introTimer < startIntroTimer * 0.3f))
			{
				smallRumble = Random.Range(2.0f, 3.0f);
				bigRumble = Random.Range (2.0f, 3.0f);
			}
			else if ((introTimer > startIntroTimer * 0f) && (introTimer < startIntroTimer * 0.2f))
			{
				smallRumble = 5.0f;
				bigRumble = 5.0f;
			}
			else 
			{
				smallRumble = 0f;
				bigRumble = 0f;
			}

			Player.padVibration(0,bigRumble,smallRumble);
			//Player.padVibration(1,bigRumble,smallRumble);
		}

	}

	void Update () {

		if (!gameStarted) {
			hickory.canMove = false; //had to do this every update because he could move after swinging his pick (canmove is enabled when pick swing ends).
		}

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
					ToggleAesthetics();
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
				Camera.main.SendMessage ("DoCollisionShake");
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

			if ((introTimer > startIntroTimer * 0.3f) && (introTimer < startIntroTimer * 0.825f)){
				hickory.gameObject.SendMessage("Swing");
			}


			//OUTRO//
			if (outro){
				transform.position = Vector3.Lerp(outroCamStart, outroCamAim.transform.position, lerpTimer / 5);
				FTWalpha = Mathf.Lerp(0,1,lerpTimer / 5);
				FTW.material.color = new Color (FTW.material.color.r, FTW.material.color.g, FTW.material.color.b, FTWalpha);

				if (lerpTimer > 6f) {
					Application.LoadLevel("MenuPlaceholder");
				}
			}


			introTimer -= Time.deltaTime;
			lerpTimer += Time.deltaTime / 5;
		}
	}

	void CaveIn(){
		caveInStarted = true;
		Player.stopPadVibration(0);
		cam.SendMessage("Intro", false);
		AudioSource.PlayClipAtPoint (CaveInSFX, transform.position);
		Instantiate (caveInObj, caveInSpawnPos.position, Quaternion.identity);
	}

	void ToggleMe(GameObject obj){
		toggleObjs.Add (obj);
	}

	void ToggleAesthetics(){
		foreach (GameObject obj in toggleObjs) {
			if (obj.activeSelf){
				obj.SetActive (false);
			} else {
				obj.SetActive (true);
			}
		}
	}

	void Outro(){
		cam.SendMessage("Intro", true);
		lerpTimer = -1f;
		outro = true;
		outroCamStart = transform.position;
	}
}
