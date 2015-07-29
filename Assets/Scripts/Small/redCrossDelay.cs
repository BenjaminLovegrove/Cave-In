using UnityEngine;
using System.Collections;

public class redCrossDelay : MonoBehaviour {

	public SpriteRenderer spriteR;

	void Update () {
		if (Intro.introTimer < -1f) {
			spriteR.enabled = true;
		}
	}
}
