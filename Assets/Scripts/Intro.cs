using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intro : MonoBehaviour {

	public static bool watchIntro = true;
	public static int skipNum = 0; 
	public PlayerV2 sorek;
	public PlayerV2 hickory;
	public static Camera cam;
	public static float introTimer = 30f;
	public static float startIntroTimer;
	public float lerpTimer;
	public static bool caveInStarted = false;
	public static bool gameStarted = false;
	public GameObject caveInObj;
	public Transform caveInSpawnPos;
	public AudioClip CaveInSFX;
	public static bool skipped = false;

	//Intro and outro aesthetics (to be switched after intro)
	public GameObject introStuff;
	public GameObject outroStuff;

	public float smallRumble, bigRumble;

	Vector3 sorekCamPos;
	Vector3 hickoryCamPos;

	//outro
	bool outro = false;
	Vector3 outroCamStart;
	public Transform outroCamAim;
	public Renderer FTW;
	float FTWalpha = 1f;

	//List<GameObject> toggleObjs = new List<GameObject>();

	bool lerpToSorek = false; //This is just a hack to set the lerp timer to 0 only once.
	void Awake()
	{
		BeginIntro();
	}

	// Use this for initialization
	public void BeginIntro() {
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


		#region Force Intro to work from pause menu
		if (PauseMenu.restartedLevel == true ) //force the intro to restart from pause menu
		{
			PauseMenu.restartedLevel = false;
			gameStarted = false;
			caveInStarted = false;
			introTimer = 1;
		}
		if (PauseMenu.mainMenuLoop == true)
		{
			print ("running menu loop");
			sorek.canMove = false;
			hickory.canMove = false;
			cam.SendMessage("Intro", true);
			lerpTimer = -1f;
			introTimer = 30;
			PauseMenu.mainMenuLoop = false;
			cam.SendMessage("Intro", true);
			lerpToSorek = false;
			gameStarted = false;
			caveInStarted = false;
			print ("running menu loop end");
		}
		#endregion

		if (CheckpointManager.checkpointSpawn) {
			watchIntro = false;
			Skip ();
			sorek.gameObject.transform.position = CheckpointManager.p1checkpoint;
			hickory.gameObject.transform.position = CheckpointManager.p2checkpoint;
		}
	}

	void FixedUpdate()
	{



		if (watchIntro)
		{

			//do xbox rumble here 
			if ((introTimer > startIntroTimer * 0.25f) && (introTimer < startIntroTimer * 0.5f))
			{
				smallRumble = Random.Range(0.1f, 0.2f);
				bigRumble = Random.Range (0.0f, 0.0f);
			}
			else if ((introTimer > startIntroTimer * 0f) && (introTimer < startIntroTimer * 0.249f))
			{
				smallRumble = 0.9f;
				bigRumble = 0.9f;
			}
			else 
			{
				smallRumble = 0f;
				bigRumble = 0f;
			}

			Controller.xInput.padVibration(Player.pone,bigRumble,smallRumble);
			Controller.xInput.padVibration(Player.ptwo,bigRumble,smallRumble);
		}

	}

	void Update () {
		//OUTRO//
		if (outro) {
			transform.position = Vector3.Lerp (outroCamStart, outroCamAim.transform.position, lerpTimer / 5);
			FTWalpha = Mathf.Lerp (0, 1, lerpTimer / 3);
			FTW.material.color = new Color (FTW.material.color.r, FTW.material.color.g, FTW.material.color.b, FTWalpha);
			
			if (lerpTimer > 6f) {
				Application.LoadLevel ("MenuPlaceholder");
			}
		} else {

			//Double check that these mother fuckers are active/inactive
			if (introTimer < startIntroTimer * 0.6f) {
				if (introStuff != null) {
					if (introStuff.activeSelf) {
						ToggleAesthetics ();
						introStuff.SetActive (false);
						outroStuff.SetActive (true);
					}
				}
			}

			introTimer -= Time.deltaTime;
			if (!gameStarted) {
				hickory.canMove = false; //had to do this every update because he could move after swinging his pick (canmove is enabled when pick swing ends).
			}

			/*SKIP*/
			if (Input.GetKeyDown (KeyCode.Escape) && skipNum <= 2) {
				skipNum ++;
				UI.displaySkip ++;
				if (skipNum >= 3) {
					Skip ();
				}
			}


			if (watchIntro) {
				if (introTimer > startIntroTimer * 0.6f) {
					transform.position = Vector3.Lerp (this.transform.position, hickoryCamPos, lerpTimer);
				}
				//Cam move to sorek holding lamp.
				if ((introTimer > startIntroTimer * 0.4f) && (introTimer < startIntroTimer * 0.6f)) {
					if (lerpToSorek == false) {
						ToggleAesthetics ();
						lerpToSorek = true;
						lerpTimer = 0f;
					}
					transform.position = Vector3.Lerp (this.transform.position, sorekCamPos, lerpTimer);
				}
				//Cam gameplay on (set to normal cam)
				if ((introTimer > startIntroTimer * 0.3f) && (introTimer < startIntroTimer * 0.4f)) {
					cam.SendMessage ("Intro", false);
				}
				//Screen shake
				if ((introTimer > startIntroTimer * 0f) && (introTimer < startIntroTimer * 0.3f)) {
					Camera.main.SendMessage ("DoCollisionShake");
				}
				//Cave in instantiate
				if (introTimer < startIntroTimer * 0.1f && !caveInStarted) {
					CaveIn ();
				}


				//Make hickory swing his pick
				if ((introTimer > startIntroTimer * 0.15f) && (introTimer < startIntroTimer * 0.825f)) {
					hickory.gameObject.SendMessage ("Swing");
				}
			}

			//Sorek and Hickory canMove;
			if (introTimer < 0f && !gameStarted) {
				sorek.canMove = true;
				hickory.canMove = true;
				gameStarted = true;
				PauseMenu.canPauseGame = true;
				cam.SendMessage ("Intro", false);
			}

		}



			lerpTimer += Time.deltaTime / 5;


	}

	public void CaveIn(){
		caveInStarted = true;
		Player.stopPadVibration(0);
		cam.SendMessage("Intro", false);
		AudioSource.PlayClipAtPoint (CaveInSFX, transform.position);
		Instantiate (caveInObj, caveInSpawnPos.position, Quaternion.identity);
	}

	void ToggleMe(GameObject obj){
		//toggleObjs.Add (obj);
	}

	void ToggleAesthetics(){
		//This list was not working in builds
		/*foreach (GameObject obj in toggleObjs) {
			if (obj.activeSelf){
				obj.SetActive (false);
			} else {
				obj.SetActive (true);
			}
		}*/

		introStuff.SetActive (false);
		outroStuff.SetActive (true);
	}

	void Outro(){
		cam.SendMessage("Intro", true);
		lerpTimer = -0.1f;
		outro = true;
		outroCamStart = transform.position;
	}

	public static void Skip(){
		if (!skipped) {
			introTimer = startIntroTimer * 0.3f;
			cam.SendMessage ("Intro", false);
			Intro.skipped = true;
			PauseMenu.canPauseGame = true;
		}
	}
}
