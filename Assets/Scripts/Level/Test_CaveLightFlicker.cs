using UnityEngine;
using System.Collections;

public class Test_CaveLightFlicker : MonoBehaviour {

	private float caveLightFlicker;
	private bool activeFlicker = false;
	public float minRange, maxRange;
	public float flickerTiming;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		caveLightFlicker = Random.Range(minRange,maxRange);


		if (!activeFlicker){
			StartCoroutine(Flicker());
		}

	}

	public IEnumerator Flicker ()
	{
		activeFlicker = true;
		this.light.range = caveLightFlicker;
		yield return new WaitForSeconds(flickerTiming);
		activeFlicker = false;

	}
}
