using UnityEngine;
using System.Collections;

public class Mineable : MonoBehaviour {

	public int hitsReqd = 8;
	public GameObject CaveIn;

	bool startGame;	

	void PickHit(){
		hitsReqd --;
		if (hitsReqd <= 0) {
			//Play particle effect
			CaveIn.SendMessage("StartGame");
			Destroy (this.gameObject);
		}
	}
}
