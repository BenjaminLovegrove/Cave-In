using UnityEngine;
using System.Collections;

public class Test_CaveLightFlicker : MonoBehaviour {

	public float caveLightFlicker;
	private bool activeFlicker = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		caveLightFlicker = Random.Range(-5,5);


		if (!activeFlicker){
			StartCoroutine(Flicker());
		}

	}

	public IEnumerator Flicker ()
	{
		activeFlicker = true;
		this.light.range = caveLightFlicker;
		yield return new WaitForSeconds(0.2f);
		activeFlicker = false;

	}
}
