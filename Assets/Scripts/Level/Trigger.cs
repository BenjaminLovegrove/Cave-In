using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool camTrigger = true; 
	public string triggerString;
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {

		col.SendMessage (triggerString);

		if (camTrigger){
			Camera.main.SendMessage (triggerString);
		}

		Destroy (this.gameObject);
	}
}
