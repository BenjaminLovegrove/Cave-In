using UnityEngine;
using System.Collections;

public class IntroToggle : MonoBehaviour {

	public bool intro;

	// Use this for initialization
	void Start () {
		Camera.main.SendMessage ("ToggleMe", this.gameObject);
		if (!intro) {
			this.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
