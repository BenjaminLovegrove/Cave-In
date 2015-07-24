using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class LampSorek : MonoBehaviour {

	Player sorekScr;
	public GameObject fireSpread;
	public AudioClip fireSFX;
	public float fireCD = 1f;
	float fireCDtimer;

	void Start (){
		sorekScr = GetComponent<Player> ();
	}

	void Update () {
		fireCDtimer -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.F) && fireCDtimer <= 0 && !sorekScr.climbingLadder){
			fireCDtimer = fireCD;
			Instantiate (fireSpread, transform.position, Quaternion.identity);
			AudioSource.PlayClipAtPoint (fireSFX, transform.position);
			this.SendMessage("Slow");
		}

		if (Input.GetKeyDown(KeyCode.Joystick1Button1) && fireCDtimer <= 0 && !sorekScr.climbingLadder){
			fireCDtimer = fireCD;
			Instantiate (fireSpread, transform.position, Quaternion.identity);
			AudioSource.PlayClipAtPoint (fireSFX, transform.position);
			this.SendMessage("Slow");
		}

	}
}
