using UnityEngine;
using System.Collections;

public class CaveInDestroyOnLoad : MonoBehaviour {

	public float desTimer = 5f;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, desTimer);
	}
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		col.gameObject.SendMessage ("Crushed", SendMessageOptions.DontRequireReceiver);
	}
}
