using UnityEngine;
using System.Collections;

public class Mineable : MonoBehaviour {

	public int hitsReqd = 8;
	public AudioClip rockBreak;

	public GameObject CaveIn;
	bool startGame = false;	//Only use this for the first cave in

	void PickHit(){
		hitsReqd --;
		if (hitsReqd <= 0) {
			//Play particle effect
			if (rockBreak != null){
				AudioSource.PlayClipAtPoint(rockBreak, transform.position);
			}
			CaveIn.SendMessage("StartGame");
			Destroy (this.gameObject);
		}
	}
}
