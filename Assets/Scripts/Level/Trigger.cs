using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public string triggerString;
	
	// Update is called once per frame
	void OnTriggerEnter () {
		Camera.main.SendMessage (triggerString);
	}
}
