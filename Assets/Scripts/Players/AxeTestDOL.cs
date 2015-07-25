using UnityEngine;
using System.Collections;

public class AxeTestDOL : MonoBehaviour {

	public Player hickoryScr;

	// Use this for initialization
	void Start () {
	//pickaxe destroy on load
		hickoryScr = GameObject.Find ("Hickory").GetComponent<Player> ();
		Destroy (gameObject,1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (hickoryScr.rightFaced) {
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
		} else {
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
		}

	}
}
