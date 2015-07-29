using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class LampSorek : MonoBehaviour {

	Player sorekScr;
	public GameObject fireSpread;
	public AudioClip fireSFX;
	public float fireCD = 0.5f;
	float fireCDtimer;

	void Start (){
		sorekScr = GetComponent<Player> ();
	}

	void Update () {
		fireCDtimer -= Time.deltaTime;

		if (!sorekScr.isDead && !sorekScr.menuActive) 
		{
			if (!Player.keyboardActive)
			{
				if (sorekScr.state.Buttons.B == ButtonState.Pressed && fireCDtimer <= 0 && !sorekScr.climbingLadder) {
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
