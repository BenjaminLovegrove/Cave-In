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
			if (!Player.keyboardActive)
			{
				if (sorekScr.xInput.OnButtonDownB && fireCDtimer <= 0 && !sorekScr.climbingLadder) {
					print ("fire placed");
					fireCDtimer = fireCD;
					Instantiate (fireSpread, transform.position, Quaternion.identity);
					AudioSource.PlayClipAtPoint (fireSFX, transform.position);
					this.SendMessage ("Slow");
				}
			}
			else{
				if (Input.GetKey (KeyCode.F) && fireCDtimer <= 0 && !sorekScr.climbingLadder) {
					fireCDtimer = fireCD;
					Instantiate (fireSpread, transform.position, Quaternion.identity);
					AudioSource.PlayClipAtPoint (fireSFX, transform.position);
					this.SendMessage ("Slow");
				}
			}
			
		}
		
	}
}
