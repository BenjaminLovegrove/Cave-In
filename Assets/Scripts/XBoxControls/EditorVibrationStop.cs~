using UnityEngine;
using System.Collections;

public class EditorVibrationStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!Application.isPlaying)
		{
			print ("resetting vibration");
			Controller.xInput.padVibration(Player.pone, 0,0);
			Controller.xInput.padVibration(Player.ptwo,0,0);
		}
	}
}
