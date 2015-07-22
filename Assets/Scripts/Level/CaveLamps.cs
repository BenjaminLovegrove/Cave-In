using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	public float destroyTimer;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
