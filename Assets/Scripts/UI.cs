using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public PlayerV2 playerOneScr;
	public PlayerV2 playerTwoScr;

	public Texture uiRestart;
	public Texture uiSkipTwice, uiSkip;
	public Texture topUiBars, bottomUiBars;
	public static int displaySkip = 0; //display the second skip ui

	void OnGUI() {
		if (Intro.introTimer > 0f  && Intro.skipped == false){
			if (!playerOneScr.introSkipped && !playerOneScr.introSkipped && displaySkip == 1){
				GUI.DrawTexture (new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width / 5, Screen.width / 5), uiSkipTwice, ScaleMode.ScaleToFit);
			}
			else if (!playerOneScr.introSkipped && !playerOneScr.introSkipped && displaySkip == 2) 
			{
				GUI.DrawTexture (new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width / 5, Screen.width / 5), uiSkip, ScaleMode.ScaleToFit);
			}


		}

		if (Intro.introTimer > 0f  && Intro.introTimer < 29f && Intro.skipped == false){
			//bars for intro cutsene
			GUI.DrawTexture (new Rect(0, 0, Screen.width , Screen.height *3.3f), topUiBars, ScaleMode.ScaleToFit);
			GUI.DrawTexture (new Rect(0, 0, Screen.width *5 , Screen.height / 6f), bottomUiBars);
			//left top width height
		}

		if (playerOneScr.isDead || playerTwoScr.isDead){
			GUI.DrawTexture (new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width / 5, Screen.width / 5), uiRestart, ScaleMode.ScaleToFit);
		}
	}
}
