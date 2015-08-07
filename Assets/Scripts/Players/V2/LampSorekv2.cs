using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class LampSorekv2 : MonoBehaviour {
	
	PlayerV2 sorekScr;
	public GameObject fireSpread;
	public AudioClip fireSFX;
	public float fireCD = 0.5f;
	float fireCDtimer;
	

	void Start (){
		sorekScr = GetComponent<PlayerV2> ();
	}
	
	void Update () {
		fireCDtimer -= Time.deltaTime;
		
		if (!sorekScr.isDead && !sorekScr.menuActive && Intro.introTimer < 0f) 
		{
			if (!PlayerV2.keyboardActive)
			{
				if (sorekScr.xInput.OnButtonB && fireCDtimer <= 0 && !sorekScr.climbingLadder && SorekLanternCD.diminishAmt < 0.9f) {
					SorekLanternCD.diminishAmt += 0.05f;
					SorekLanternCD.replenishTimer = 1.0f;
					fireCDtimer = fireCD;
					Instantiate (fireSpread, transform.position, Quaternion.identity);
					AudioSource.PlayClipAtPoint (fireSFX, transform.position);
					this.SendMessage ("Slow");
				}
			}
			else{
				if (Input.GetKey (KeyCode.F) && fireCDtimer <= 0 && !sorekScr.climbingLadder && SorekLanternCD.diminishAmt < 0.9f) {
					SorekLanternCD.diminishAmt += 0.05f;
					SorekLanternCD.replenishTimer = 1.0f;
					fireCDtimer = fireCD;
					Instantiate (fireSpread, transform.position, Quaternion.identity);
					AudioSource.PlayClipAtPoint (fireSFX, transform.position);
					this.SendMessage ("Slow");
				}
			}
			
		}
		
	}
}
