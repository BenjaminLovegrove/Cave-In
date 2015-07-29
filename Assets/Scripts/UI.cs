using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public Player playerOneScr;
	public Player playerTwoScr;

	public Texture uiRestart;
	public Texture uiSkip;
	
	void OnGUI() {
		if (Intro.introTimer > 0f){
			if (!playerOneScr.introSkipped && !playerOneScr.introSkipped){
				GUI.DrawTexture (new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width / 5, Screen.width / 5), uiSkip, ScaleMode.ScaleToFit);
			}
		}

		if (playerOneScr.isDead || playerTwoScr.isDead){
			GUI.DrawTexture (new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width / 5, Screen.width / 5), uiRestart, ScaleMode.ScaleToFit);
		}
	}
}
