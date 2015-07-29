using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool camTrigger = true; 
	public string triggerString;
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {

		if (col.tag == "Player") {
			col.SendMessage (triggerString, SendMessageOptions.DontRequireReceiver);
			Destroy (this.gameObject);
		}

		if (camTrigger){
			Camera.main.SendMessage (triggerString);
		}


	}
}
