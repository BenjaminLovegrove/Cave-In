using UnityEngine;
using System.Collections;

public class Mineable : MonoBehaviour {

	public int hitsReqd = 8;
	public AudioClip rockBreak;

	public GameObject CaveIn;
	public float destroyTime = 0f;


	void PickHit(){
		hitsReqd --;
		if (hitsReqd <= 0) {
			//Play particle effect
			if (rockBreak != null){
				AudioSource.PlayClipAtPoint(rockBreak, transform.position);
			}
			if (CaveIn != null){
				GameObject newCaveIn = Instantiate (CaveIn) as GameObject;
				newCaveIn.SendMessage("StartCaveIn");
			}
			Destroy (this.gameObject, destroyTime);
		}
	}
}
