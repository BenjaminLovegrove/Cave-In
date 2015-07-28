using UnityEngine;
using System.Collections;

public class Mineable : MonoBehaviour {

	public int hitsReqd = 8;
	public AudioClip rockBreak;

	public GameObject CaveIn;

	void PickHit(){
		hitsReqd --;
		if (hitsReqd <= 0) {
			//Play particle effect
			if (rockBreak != null){
				AudioSource.PlayClipAtPoint(rockBreak, transform.position);
			}
			if (CaveIn != null){
				CaveIn.SendMessage("StartGame");
			}
			Destroy (this.gameObject);
		}
	}
}
